﻿using System.Drawing;
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
            mandelbrotDrawer.Draw(imageSize, topLeft, bottomRight, maxIterationDepth, threshold);

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