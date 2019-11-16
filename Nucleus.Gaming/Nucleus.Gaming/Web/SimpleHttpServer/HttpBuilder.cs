using System.IO;

namespace Nucleus.Gaming.Web {
    class HttpBuilder {
        public static HttpResponse InternalServerError() {
            string content = File.ReadAllText("Resources/Pages/500.html");

            return new HttpResponse() {
                ReasonPhrase = "InternalServerError",
                StatusCode = "500",
                ContentAsUTF8 = content
            };
        }

        public static HttpResponse NotFound(string route) {
            string content = File.ReadAllText("Resources/Pages/404.html");
            content = content.Replace("{{ROUTE}}", route);

            return new HttpResponse() {
                ReasonPhrase = "NotFound",
                StatusCode = "404",
                ContentAsUTF8 = content
            };
        }
    }
}
