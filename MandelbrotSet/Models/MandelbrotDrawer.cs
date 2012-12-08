using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MandelbrotSet.Models
{
    public class MandelbrotDrawer
    {
        private Size _imageSize;
        public byte[] ImageBytes { get; set; }

        public void Draw(Size imageSize, ComplexNumber upperLeft, ComplexNumber bottomRight)
        {
            _imageSize = imageSize;

            double realRange = bottomRight.RealPart - upperLeft.RealPart;
            double imaginaryRange = upperLeft.ImaginaryPart - bottomRight.ImaginaryPart;

            var mandelbrotComputer = new MandelbrotComputer();

            using (var bitmap = new Bitmap(imageSize.Width, imageSize.Height))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.Black);

                for (int x = 0; x < imageSize.Width; x++)
                {
                    for (int y = 0; y < imageSize.Height; y++)
                    {
                        //mandelbrotComputer.ComputeNumberOfIterations(x, y);
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