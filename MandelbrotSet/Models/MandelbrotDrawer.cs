using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace MandelbrotSet.Models
{
    public class MandelbrotDrawer
    {
        public byte[] ImageBytes { get; set; }

        public void Draw(Size imageSize, ComplexNumber topLeft, ComplexNumber bottomRight, int maxIterationDepth = 199, double threshold = 2.0)
        {
            double realRange = bottomRight.RealPart - topLeft.RealPart;
            double imaginaryRange = topLeft.ImaginaryPart - bottomRight.ImaginaryPart;

            var mandelbrotComputer = new MandelbrotComputer(maxIterationDepth, threshold);

            int[,] iterations = new int[imageSize.Width, imageSize.Height];

            double realResolution = realRange / imageSize.Width;
            double imaginaryResolution = imaginaryRange / imageSize.Height;

            Action<int> computeIteration = x =>
            {
                for (int y = 0; y < imageSize.Height; y++)
                {
                    double realPart = topLeft.RealPart + x * realResolution;
                    double imaginaryPart = topLeft.ImaginaryPart - y * imaginaryResolution;

                    var z = new ComplexNumber(realPart, imaginaryPart);
                    iterations[x, y] = mandelbrotComputer.ComputeIterationDepthFor(z);
                }
            };

            Parallel.For(0, imageSize.Width, computeIteration);

            using (var bitmap = new Bitmap(imageSize.Width, imageSize.Height))
            {
                DrawMandelbrotSet(bitmap, iterations, maxIterationDepth);
            }
        }

        private unsafe void DrawMandelbrotSet(Bitmap bitmap, int[,] iterations, int maxIterationDepth)
        {
            var entireImageArea = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData imageData = bitmap.LockBits(entireImageArea, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            const int bytesPerPixel = 3;

            byte* firstPixel = (byte*)imageData.Scan0.ToPointer();
            int stride = imageData.Stride;

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
                    byte greenValue = (byte)(iterationDepth == maxIterationDepth
                        ? 0
                        : (255.0 * iterationDepth / maxIterationDepth));

                    row[blueIndex] = 0;
                    row[greenIndex] = greenValue;
                    row[redIndex] = 0;
                }
            }

            bitmap.UnlockBits(imageData);
            SaveBitmapToByteArray(bitmap);
        }

        private void SaveBitmapToByteArray(Bitmap bitmap)
        {
            using (var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Png);
                ImageBytes = memoryStream.ToArray();
            }
        }
    }
}
