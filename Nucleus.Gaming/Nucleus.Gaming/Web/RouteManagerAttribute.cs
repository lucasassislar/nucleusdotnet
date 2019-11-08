using SimpleHttpServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Gaming.Web {
    [AttributeUsage(AttributeTargets.Class)]
    public class RouteManagerAttribute : Attribute {
        public string UrlRegex { get; set; }

        public RouteManagerAttribute(string urlRegex) {
            UrlRegex = urlRegex;
        }
    }
}
