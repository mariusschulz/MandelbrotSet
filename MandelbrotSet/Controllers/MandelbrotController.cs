using System.Drawing;
using System.Web.Mvc;
using MandelbrotSet.Models;

namespace MandelbrotSet.Controllers
{
    public class MandelbrotController : Controller
    {
        public ActionResult Drawing(int width, int height, ComplexNumber topLeft, ComplexNumber bottomRight)
        {
            var imageSize = GetImageSize(width, height);
            
            var mandelbrotDrawer = new MandelbrotDrawer();
            mandelbrotDrawer.Draw(imageSize, topLeft, bottomRight);

            return File(mandelbrotDrawer.ImageBytes, "image/png");
        }

        private static Size GetImageSize(int width, int height)
        {
            if (width < 0)
            {
                width = -width;
            }

            if (height < 0)
            {
                height = -height;
            }

            return new Size(width, height);
        }
    }
}