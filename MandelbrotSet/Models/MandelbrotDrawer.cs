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
            Parallel.For(0, imageSize.Width, x => Parallel.For(0, imageSize.Height, y =>
            {
                double realPart = topLeft.RealPart + x * realRange / imageSize.Width;
                double imaginaryPart = topLeft.ImaginaryPart - y * imaginaryRange / imageSize.Height;

                var z = new ComplexNumber(realPart, imaginaryPart);

                iterations[x, y] = mandelbrotComputer.ComputeIterationDepthFor(z);
            }));

            using (var bitmap = new Bitmap(imageSize.Width, imageSize.Height))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                for (int x = 0; x < imageSize.Width; x++)
                {
                    for (int y = 0; y < imageSize.Height; y++)
                    {
                        int iterationDepth = iterations[x, y];
                        var color = iterationDepth == maxIterationDepth
                            ? Color.Black
                            : Color.FromArgb(255, 0, (int)Math.Floor(255.0 * iterationDepth / maxIterationDepth), 0);

                        using (var pen = new Pen(color))
                        {
                            graphics.DrawRectangle(pen, x, y, 2, 2);
                        }
                    }
                }

                SaveBitmapToByteArray(bitmap);
            }
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