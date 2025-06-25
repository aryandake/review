using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fiction2Fact
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
            routes.IgnoreRoute("{*allasmx}", new { allasmx = @".*\.asmx(/.*)?" });
            routes.MapRoute(
                name: "Contract",
                url: "ContractManagement/{controller}/{action}/{id}",
                defaults: new { controller = "ContractManagement/Contract", action = "Id", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "AdminMasters",
                url: "Masters/{controller}/{action}/{id}",
                defaults: new { controller = "Masters/AdminMasters", action = "MenuIndex", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
