﻿// Copyright (C) 2016 by David Jeske, Barend Erasmus and donated to the public domain
// Further modified for Nucleus.Gaming

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Nucleus.Gaming.Web {
    public class HttpServer {
        public int Port {
            get; private set;
        }

        private TcpListener Listener;
        private HttpProcessor Processor;
        private bool IsActive = true;
        private Thread serverThread;

        public HttpServer(int port, List<Route> routes, string path) {
            this.Port = port;
            this.Processor = new HttpProcessor(path);

            foreach (var route in routes) {
                this.Processor.AddRoute(route);
            }
        }

        public void Listen() {
            this.Listener = new TcpListener(IPAddress.Any, this.Port);
            this.Listener.Start();

            serverThread = new Thread(ServerThread);
            serverThread.Start();
        }

        /// <summary>
        /// Disable try-catch
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Lock concurrent threads
        /// </summary>
        public bool DebugLock { get; set; }

        private object DebugLocker = new object();

        private void ServerThread(object state) {
            while (this.IsActive) {
                if (Debug) {
                    TcpClient s = this.Listener.AcceptTcpClient();
                    Thread thread = new Thread(() => {
                        if (DebugLock) {
                            lock (DebugLocker) {
                                this.Processor.HandleClient(s);
                            }
                        } else {
                            this.Processor.HandleClient(s);
                        }
                    });
                    thread.Start();
                    Thread.Sleep(1);

                } else {
                    try {
                        TcpClient s = this.Listener.AcceptTcpClient();
                        Thread thread = new Thread(() => {
                            try {
                                this.Processor.HandleClient(s);
                            } catch { }
                        });
                        thread.Start();
                        Thread.Sleep(1);
                    } catch (Exception ex) {
                        var startColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Exception " + ex);
                        Console.ForegroundColor = startColor;
                    }
                }
            }
        }
    }
}



