using System.Web.Mvc;
using System.Web.Routing;

namespace MandelbrotSet
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("MandelbrotDrawing", "mandelbrot/drawing/"
                + "{width}-{height}/"
                + "({realFrom},{imaginaryFrom})-({realTo},{imaginaryTo})/"
                + "({maxIterationDepth},{threshold})",
                new { controller = "mandelbrot", action = "drawing" }
            );

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
