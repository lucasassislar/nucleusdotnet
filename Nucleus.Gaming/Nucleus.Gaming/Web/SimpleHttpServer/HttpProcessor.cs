// Copyright (C) 2016 by David Jeske, Barend Erasmus and donated to the public domain
using SimpleHttpServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace SimpleHttpServer {
    public class HttpProcessor {
        private static int MAX_POST_SIZE = 10 * 1024 * 1024; // 10MB

        private List<Route> Routes = new List<Route>();

        public HttpProcessor() {
        }

        public void HandleClient(TcpClient tcpClient) {
            Stream inputStream = GetInputStream(tcpClient);
            Stream outputStream = GetOutputStream(tcpClient);
            HttpRequest request = GetRequest(inputStream, outputStream);

            // route and handle the request...
            HttpResponse response = RouteRequest(inputStream, outputStream, request);

            Console.WriteLine("{0} {1}", response.StatusCode, request.Url);
            // build a default response for errors
            if (response.Content == null) {
                if (response.StatusCode != "200") {
                    response.ContentAsUTF8 = string.Format("{0} {1} <p> {2}", response.StatusCode, request.Url, response.ReasonPhrase);
                }
            }

            WriteResponse(outputStream, response);

            outputStream.Flush();
            outputStream.Close();
            outputStream = null;

            inputStream.Close();
            inputStream = null;

        }

        // this formats the HTTP response...
        private static void WriteResponse(Stream stream, HttpResponse response) {
            if (response.Content == null) {
                response.Content = new byte[] { };
            }

            // default to text/html content type
            if (!response.Headers.ContainsKey("Content-Type") &&
                response.Content.Length > 0) {
                response.Headers["Content-Type"] = "text/html";
            }

            response.Headers["Content-Length"] = response.Content.Length.ToString();

            Write(stream, string.Format("HTTP/1.0 {0} {1}\r\n", response.StatusCode, response.ReasonPhrase));
            Write(stream, string.Join("\r\n", response.Headers.Select(x => string.Format("{0}: {1}", x.Key, x.Value))));
            Write(stream, "\r\n\r\n");

            stream.Write(response.Content, 0, response.Content.Length);
        }

        public void AddRoute(Route route) {
            this.Routes.Add(route);
        }

        private static string Readline(Stream stream) {
            int next_char;
            string data = "";
            while (true) {
                next_char = stream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\r') { continue; }
                if (next_char == -1) { Thread.Sleep(1); continue; };
                data += Convert.ToChar(next_char);
            }
            return data;
        }

        private static void Write(Stream stream, string text) {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
        }

        protected virtual Stream GetOutputStream(TcpClient tcpClient) {
            return tcpClient.GetStream();
        }

        protected virtual Stream GetInputStream(TcpClient tcpClient) {
            return tcpClient.GetStream();
        }

        protected virtual HttpResponse RouteRequest(Stream inputStream, Stream outputStream, HttpRequest request) {
            List<Route> routes = this.Routes.Where(x => Regex.Match(request.Url, x.UrlRegex).Success).ToList();

            if (!routes.Any()) {
                return HttpBuilder.NotFound(request.Url);
            }

            Dictionary<string, string> headers = new Dictionary<string, string>();
            if (request.Headers.ContainsKey("Origin")) {
                headers.Add("Access-Control-Allow-Origin", request.Headers["Origin"]);
            }
            headers.Add("Access-Control-Allow-Headers", "authorization, X-PINGOTHER, Content-Type");
            headers.Add("Access-Control-Max-Age", "86400");
            headers.Add("Keep-Alive", "timeout=2, max=100");
            headers.Add("Connection", "Keep-Alive");

            headers.Add("X-DNS-Prefetch-Control", "off");
            headers.Add("X-Frame-Options", "SAMEORIGIN");
            headers.Add("Strict-Transport-Security", "max-age=15552000; includeSubDomains");
            headers.Add("X-Download-Options", "noopen");
            headers.Add("X-Content-Type-Options", "nosniff");
            headers.Add("X-XSS-Protection", "1; mode=block");
            headers.Add("Date", DateTime.Now.ToLongDateString());


            //X-DNS-Prefetch-Control →off
            //X-Frame-Options →SAMEORIGIN
            //Strict-Transport-Security →max-age=15552000; includeSubDomains
            //X-Download-Options →noopen
            //X-Content-Type-Options →nosniff
            //X-XSS-Protection →1; mode=block
            //Access-Control-Allow-Origin →*
            //Date →Tue, 16 Jul 2019 19:09:07 GMT
            //Connection →keep-alive
            //Content-Length →0

            if (request.Method == "OPTIONS") {

                string allRoutes = "";
                for (int i = 0; i < routes.Count; i++) {
                    string r = routes[i].Method;
                    allRoutes += r;
                    if (i != routes.Count - 1) {
                        allRoutes += ", ";
                    }
                }

                headers.Add("Access-Control-Allow-Methods", allRoutes);

                return new HttpResponse() {
                    ReasonPhrase = "",
                    StatusCode = "204",
                    Headers = headers
                };
            }

            Route route = routes.SingleOrDefault(x => x.Method == request.Method);

            if (route == null) {
                Console.WriteLine("Method Not Allowed");
                return new HttpResponse() {
                    ReasonPhrase = "Method Not Allowed",
                    StatusCode = "405",
                };
            }

            // extract the path if there is one
            var match = Regex.Match(request.Url, route.UrlRegex);
            if (match.Groups.Count > 1) {
                request.Path = match.Groups[1].Value;
            } else {
                request.Path = request.Url;
            }

            // trigger the route handler...
            request.Route = route;
            try {
                HttpResponse response = route.Callable(request);

                if (response.Headers == null) {
                    response.Headers = headers;
                } else {
                    foreach (var header in headers) {
                        if (!response.Headers.ContainsKey(header.Key)) {
                            response.Headers.Add(header.Key, header.Value);
                        }
                    }
                }

                return response;
            } catch (Exception ex) {
                //log.Error(ex);
                return HttpBuilder.InternalServerError();
            }
        }

        private HttpRequest GetRequest(Stream inputStream, Stream outputStream) {
            //Read Request Line
            string request = Readline(inputStream);

            string[] tokens = request.Split(' ');
            if (tokens.Length != 3) {
                throw new Exception("invalid http request line");
            }
            string method = tokens[0].ToUpper();
            string url = tokens[1];
            string protocolVersion = tokens[2];

            //Read Headers
            Dictionary<string, string> headers = new Dictionary<string, string>();
            string line;
            while ((line = Readline(inputStream)) != null) {
                if (line.Equals("")) {
                    break;
                }

                int separator = line.IndexOf(':');
                if (separator == -1) {
                    throw new Exception("invalid http header line: " + line);
                }
                string name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' ')) {
                    pos++;
                }

                string value = line.Substring(pos, line.Length - pos);
                headers.Add(name, value);
            }

            string content = null;
            string contentLengthHeader = null;
            if (headers.TryGetValue("Content-Length", out contentLengthHeader) ||
                headers.TryGetValue("content-length", out contentLengthHeader)) {
                int totalBytes = Convert.ToInt32(contentLengthHeader);
                int bytesLeft = totalBytes;
                byte[] bytes = new byte[totalBytes];

                while (bytesLeft > 0) {
                    byte[] buffer = new byte[bytesLeft > 1024 ? 1024 : bytesLeft];
                    int n = inputStream.Read(buffer, 0, buffer.Length);
                    buffer.CopyTo(bytes, totalBytes - bytesLeft);

                    bytesLeft -= n;
                }

                content = Encoding.ASCII.GetString(bytes);
            }

            return new HttpRequest() {
                Method = method,
                Url = url,
                Headers = headers,
                Content = content
            };
        }
    }
}
