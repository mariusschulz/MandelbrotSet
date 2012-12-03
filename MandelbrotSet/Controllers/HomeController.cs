using System.Web.Mvc;

namespace MandelbrotSet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
