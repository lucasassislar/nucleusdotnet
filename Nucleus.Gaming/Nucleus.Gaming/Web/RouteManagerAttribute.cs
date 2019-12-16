using System;

namespace Nucleus.Gaming.Web {
    [AttributeUsage(AttributeTargets.Class)]
    public class RouteManagerAttribute : Attribute {
        public string UrlRegex { get; set; }

        public RouteManagerAttribute(string urlRegex) {
            UrlRegex = urlRegex;
        }
    }
}
