/*
 * This is a route *provider*. Such providers are very common in Orchard: to hook into some services, to extend some 
 * functionality you have to implement an interface. Because of the extensible nature of Orchard and because one should 
 * be able to switch modules on and off route registration happens a bit different than in standard ASP.NET MVC 
 * applications: we use IRouteProvider.
 */

using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Routes;

namespace OrchardHUN.TrainingDemo
{
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    public class Routes : IRouteProvider
    {
        // This is an obsolete method, but we have to also implement it for now.
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        // The method really needed.
        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[]
            {
                // We use the standard MVC routing-related types here
                new RouteDescriptor
                {
                    Route = new Route(
                        // We'll be able to access the action from under the short ~/Admin/PersonListDashboard route.
                        // It's good practice to re-route your admin controllers to under Admin/.
                        "Admin/PersonListDashboard",
                        // Remember? We used these values to set the editor route for Person List items.
                        new RouteValueDictionary
                        {
                            {"area", "OrchardHUN.TrainingDemo"},
                            {"controller", "ContentsAdmin"},
                            {"action", "PersonListDashboard"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", "OrchardHUN.TrainingDemo"}
                        },
                        new MvcRouteHandler())
                }
            };
        }

        // NEXT STATION: Let's head back to Controllers/ContentsAdminController!
    }
}