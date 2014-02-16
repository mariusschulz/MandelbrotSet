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
        private const int COLOR_DENSITY = 5;
        private const int COLOR_INTERPOLATION_FACTOR = 1000;

        public byte[] Draw(Size imageSize, ComplexNumber topLeft, ComplexNumber bottomRight, int maxIterationDepth = 199, double threshold = 2.0)
        {
            IterationValue[,] iterations = CalculateIterationDepths(imageSize, topLeft, bottomRight, maxIterationDepth, threshold);

            using (var bitmap = new Bitmap(imageSize.Width, imageSize.Height))
            {
                DrawMandelbrotSet(bitmap, iterations, maxIterationDepth);
                return SaveImageToByteArray(bitmap);
            }
        }

        private static IterationValue[,] CalculateIterationDepths(Size imageSize, ComplexNumber topLeft, ComplexNumber bottomRight, int maxIterationDepth, double threshold)
        {
            IterationValue[,] iterations = new IterationValue[imageSize.Width, imageSize.Height];

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

        private static unsafe void DrawMandelbrotSet(Bitmap bitmap, IterationValue[,] iterations, int maxIterationDepth)
        {
            var entireImageArea = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData imageData = bitmap.LockBits(entireImageArea, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            const int bytesPerPixel = 3;

            byte* firstPixel = (byte*)imageData.Scan0.ToPointer();
            int stride = imageData.Stride;

            var interpolatedColors = InterpolateColors(maxIterationDepth);

            for (int y = 0; y < imageData.Height; y++)
            {
                byte* row = firstPixel + y * stride;

                for (int x = 0; x < imageData.Width; x++)
                {
                    // Order: BGR
                    int blueIndex = x * bytesPerPixel;
                    int greenIndex = blueIndex + 1;
                    int redIndex = blueIndex + 2;

                    var color = GetInterpolatedColor(interpolatedColors, iterations[x, y], maxIterationDepth);

                    row[blueIndex] = color.B;
                    row[greenIndex] = color.G;
                    row[redIndex] = color.R;
                }
            }

            bitmap.UnlockBits(imageData);
        }

        private static List<HsvColor> InterpolateColors(int maxIterationDepth)
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
            interpolatedColors.AddRange(InterpolateLinearly(blueColors, maxIterationDepth));
            interpolatedColors.AddRange(InterpolateLinearly(orangeColors, maxIterationDepth));

            return interpolatedColors;
        }

        private static IEnumerable<HsvColor> InterpolateLinearly(IList<HsvColor> colors, int maxIterationDepth)
        {
            int interpolationCount = COLOR_INTERPOLATION_FACTOR * maxIterationDepth;
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

        private static Color GetInterpolatedColor(IList<HsvColor> interpolatedColors, IterationValue iterationValue, int maxIterationDepth)
        {
            if (iterationValue.Depth == maxIterationDepth)
                return Color.Black;

            double v = iterationValue.Depth - Math.Log(Math.Log(iterationValue.AbsoluteValue) / Math.Log(maxIterationDepth), 2);
            double index = (int)(v * COLOR_DENSITY * COLOR_INTERPOLATION_FACTOR) % interpolatedColors.Count;

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
