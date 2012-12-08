using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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

            using (var bitmap = new Bitmap(imageSize.Width, imageSize.Height))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.Black);

                for (int x = 0; x < imageSize.Width; x++)
                {
                    for (int y = 0; y < imageSize.Height; y++)
                    {
                        double realPart = topLeft.RealPart + x * realRange / imageSize.Width;
                        double imaginaryPart = topLeft.ImaginaryPart - y * imaginaryRange / imageSize.Height;

                        var z = new ComplexNumber(realPart, imaginaryPart);

                        int iterations = mandelbrotComputer.ComputeIterationDepthFor(z);

                        var pen = iterations < maxIterationDepth ? Pens.Black : Pens.White;

                        graphics.DrawRectangle(pen, x, y, 2, 2);
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