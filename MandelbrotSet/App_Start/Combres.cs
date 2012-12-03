[assembly: WebActivator.PreApplicationStartMethod(typeof(MandelbrotSet.App_Start.Combres), "PreStart")]
namespace MandelbrotSet.App_Start {
	using System.Web.Routing;
	using global::Combres;
	
    public static class Combres {
        public static void PreStart() {
            RouteTable.Routes.AddCombresRoute("Combres");
        }
    }
}