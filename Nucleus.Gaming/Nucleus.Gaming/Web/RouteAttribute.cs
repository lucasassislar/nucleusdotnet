using SimpleHttpServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
