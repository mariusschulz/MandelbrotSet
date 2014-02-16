using System;
using System.Drawing;
using System.Web.Mvc;
using MandelbrotSet.Models;

namespace MandelbrotSet.Controllers
{
    public class MandelbrotController : Controller
    {
        public ActionResult Drawing(int width, int height, int maxIterationDepth, double threshold,
            double realFrom, double realTo, double imaginaryFrom, double imaginaryTo)
        {
            var imageSize = GetImageSize(width, height);

            var topLeft = new ComplexNumber(realFrom, imaginaryFrom);
            var bottomRight = new ComplexNumber(realTo, imaginaryTo);

            var mandelbrotDrawer = new MandelbrotDrawer();
            byte[] mandelbrotBytes = mandelbrotDrawer.Draw(imageSize, topLeft, bottomRight, maxIterationDepth, threshold);

            return File(mandelbrotBytes, "image/png");
        }

        private static Size GetImageSize(int width, int height)
        {
            return new Size(Math.Abs(width), Math.Abs(height));
        }
    }
}
