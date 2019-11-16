using System;

namespace Nucleus.Gaming.Web {
    [AttributeUsage(AttributeTargets.Method)]
    public class RouteAttribute : Attribute {
        public Route Route { get; set; }

        public RouteAttribute(string method, string urlRegex) {
            Route = new Route();
            Route.Method = method;
            Route.UrlRegex = urlRegex;
            Route.Name = urlRegex;
        }
    }
}
