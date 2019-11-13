using Nucleus.Gaming;
using SimpleHttpServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Gaming.Web {
    public class RouteBuilder {
        public List<Route> BuildRoute(Assembly baseAssembly) {
            List<Route> routes = new List<Route>();
            Type[] allTypes = baseAssembly.GetTypes();

            List<object> managers = new List<object>();

            for (int i = 0; i < allTypes.Length; i++) {
                Type type = allTypes[i];
                object[] attributes = type.GetCustomAttributes(true);

                for (int j = 0; j < attributes.Length; j++) {
                    object attribute = attributes[j];

                    if (attribute is RouteManagerAttribute) {
                        RouteManagerAttribute manager = (RouteManagerAttribute)attribute;
                        object routeManager = Activator.CreateInstance(type);
                        managers.Add(routeManager);

                        string baseUrl = manager.UrlRegex;
                        ConsoleU.WriteLine($"Route Manager: {baseUrl}", ConsoleColor.Green);

                        MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                        for (int k = 0; k < methods.Length; k++) {
                            MethodInfo method = methods[k];
                            object[] mAttributes = method.GetCustomAttributes(true);

                            for (int u = 0; u < mAttributes.Length; u++) {
                                object mAttribute = mAttributes[u];

                                if (mAttribute is RouteAttribute) {
                                    RouteAttribute routeAtt = (RouteAttribute)mAttribute;
                                    Route route = routeAtt.Route;
                                    string urlRegex = route.UrlRegex;
                                    route.UrlRegex = $"{baseUrl}/{urlRegex}";
                                    route.Callable = (HttpRequest request) => {
                                        return (HttpResponse)method.Invoke(routeManager, new object[] { request });
                                    };
                                    routes.Add(route);

                                    ConsoleU.WriteLine($"   Route: [{route.Method}] /{urlRegex}", ConsoleColor.Yellow);
                                }
                            }

                        }

                    } 
                }
            }

            return routes;
        }
    }
}
