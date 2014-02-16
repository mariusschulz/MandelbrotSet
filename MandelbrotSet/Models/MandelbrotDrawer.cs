using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using MandelbrotSet.Models.Colors;

namespace MandelbrotSet.Models
{
    public class MandelbrotDrawer
    {
        public byte[] Draw(Size imageSize, ComplexNumber topLeft, ComplexNumber bottomRight, int maxIterationDepth = 199, double threshold = 2.0)
        {
            int[,] iterations = CalculateIterationDepths(imageSize, topLeft, bottomRight, maxIterationDepth, threshold);

            using (var bitmap = new Bitmap(imageSize.Width, imageSize.Height))
            {
                DrawMandelbrotSet(bitmap, iterations, maxIterationDepth);
                return SaveImageToByteArray(bitmap);
            }
        }

        private static int[,] CalculateIterationDepths(Size imageSize, ComplexNumber topLeft, ComplexNumber bottomRight, int maxIterationDepth, double threshold)
        {
            int[,] iterations = new int[imageSize.Width, imageSize.Height];

            double realRange = bottomRight.RealPart - topLeft.RealPart;
            double imaginaryRange = topLeft.ImaginaryPart - bottomRight.ImaginaryPart;

            var mandelbrotComputer = new MandelbrotComputer(maxIterationDepth, threshold);

            double realResolution = realRange / imageSize.Width;
            double imaginaryResolution = imaginaryRange / imageSize.Height;

            Action<int> computeIteration = x =>
            {
                double xResolution = x * realResolution;

                for (int y = 0; y < imageSize.Height; y++)
                {
                    double realPart = topLeft.RealPart + xResolution;
                    double imaginaryPart = topLeft.ImaginaryPart - y * imaginaryResolution;

                    var z = new ComplexNumber(realPart, imaginaryPart);
                    iterations[x, y] = mandelbrotComputer.ComputeIterationDepthFor(z);
                }
            };

            Parallel.For(0, imageSize.Width, computeIteration);

            return iterations;
        }

        private static unsafe void DrawMandelbrotSet(Bitmap bitmap, int[,] iterations, int maxIterationDepth)
        {
            var entireImageArea = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData imageData = bitmap.LockBits(entireImageArea, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            const int bytesPerPixel = 3;

            byte* firstPixel = (byte*)imageData.Scan0.ToPointer();
            int stride = imageData.Stride;

            var interpolatedColors = InterpolateColors();

            for (int y = 0; y < imageData.Height; y++)
            {
                byte* row = firstPixel + y * stride;

                for (int x = 0; x < imageData.Width; x++)
                {
                    // Order: BGR
                    int blueIndex = x * bytesPerPixel;
                    int greenIndex = blueIndex + 1;
                    int redIndex = blueIndex + 2;

                    int iterationDepth = iterations[x, y];
                    var color = GetInterpolatedColor(interpolatedColors, iterationDepth, maxIterationDepth);

                    row[blueIndex] = color.B;
                    row[greenIndex] = color.G;
                    row[redIndex] = color.R;
                }
            }

            bitmap.UnlockBits(imageData);
        }

        private static List<HsvColor> InterpolateColors()
        {
            HsvColor[] blueColors =
            {
                new HsvColor(236, 1, 0.37),
                new HsvColor(200, 1, 1),
                new HsvColor(190, 0, 1)
            };

            HsvColor[] orangeColors =
            {
                new HsvColor(54, 0, 1),
                new HsvColor(54, 1, 1),
                new HsvColor(34, 1, 0.8)
            };

            var interpolatedColors = new List<HsvColor>();
            interpolatedColors.AddRange(InterpolateLinearly(blueColors));
            interpolatedColors.AddRange(InterpolateLinearly(orangeColors));

            return interpolatedColors;
        }

        private static IEnumerable<HsvColor> InterpolateLinearly(IList<HsvColor> colors)
        {
            const int interpolationCount = 50;
            double intervalWidth = (double)interpolationCount / (colors.Count - 1);

            for (int i = 0; i < interpolationCount; i++)
            {
                int colorIndex = (int)Math.Floor(i / intervalWidth);

                HsvColor color1 = colors[colorIndex];
                HsvColor color2 = colors[colorIndex + 1];

                double interpolationDegree = i / intervalWidth - colorIndex;

                yield return color1.InterpolateLinearly(color2, interpolationDegree);
            }
        }

        private static Color GetInterpolatedColor(IList<HsvColor> interpolatedColors, int iterationDepth, int maxIterationDepth)
        {
            if (iterationDepth == maxIterationDepth)
                return Color.Black;

            const int COLOR_DENSITY = 3;

            double iterationPercentage = (double)iterationDepth / maxIterationDepth;
            double index = (iterationPercentage * interpolatedColors.Count * COLOR_DENSITY) % interpolatedColors.Count;

            return interpolatedColors[(int)index].ToColor();
        }

        private static byte[] SaveImageToByteArray(Image image)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }
    }
}
