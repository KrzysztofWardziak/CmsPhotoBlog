using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CmsPhotoBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", 
                "", 
                new {controller = "Pages", action = "Index"}, new[] { "CmsPhotoBlog.Controllers" });     
            
            routes.MapRoute(
                "Pages", 
                "{page}", 
                new {controller = "Pages", action = "Index"}, new[] { "CmsPhotoBlog.Controllers" });

            routes.MapRoute(
                "PagesMenuPartial",
                "Pages/PagesMenuPartial",
                new { controller = "Pages", action = "PagesMenuPartial" }, new[] { "CmsPhotoBlog.Controllers" });

            routes.MapRoute(
                "SideBarPartial",
                "Pages/SideBarPartial",
                new { controller = "Pages", action = "SideBarPartial" }, new[] { "CmsPhotoBlog.Controllers" });

            routes.MapRoute(
                "Blog",
                "Blog/{action}/{name}",
                new { controller = "Blog", action = "Index", name = UrlParameter.Optional }, new[] { "CmsPhotoBlog.Controllers" });

            routes.MapRoute(
                "Account",
                "Account/{action}/{id}",
                new { controller = "Account", action = "Index", id = UrlParameter.Optional }, new[] { "CmsPhotoBlog.Controllers" });


            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
