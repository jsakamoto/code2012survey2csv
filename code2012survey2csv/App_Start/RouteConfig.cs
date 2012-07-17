using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace code2012survey2csv
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "Replies",
                routeTemplate: "Replies/{action}",
                defaults: new { controller = "Replies", action = "Replies", id = RouteParameter.Optional }
            );
        }
    }
}