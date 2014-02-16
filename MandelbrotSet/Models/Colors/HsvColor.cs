using System;
using System.Drawing;

namespace MandelbrotSet.Models.Colors
{
    public struct HsvColor
    {
        public double Hue { get; private set; }
        public double Saturation { get; private set; }
        public double Value { get; private set; }

        public HsvColor(double hue, double saturation, double value)
            : this()
        {
            Hue = hue;
            Saturation = saturation;
            Value = value;
        }

        public HsvColor MixWith(HsvColor color2, double mixDegree)
        {
            double hueAvg = (1 - mixDegree) * Hue + mixDegree * color2.Hue;
            double saturationAvg = (1 - mixDegree) * Saturation + mixDegree * color2.Saturation;
            double valueAvg = (1 - mixDegree) * Value + mixDegree * color2.Value;

            return new HsvColor(hueAvg, saturationAvg, valueAvg);
        }

        public Color ToColor()
        {
            double c = Value * Saturation;
            double h = Hue / 60;
            double x = c * (1 - Math.Abs(h % 2 - 1));
            double m = Value - c;

            if (h < 1) return ToColor(m, c, x, 0);
            if (h < 2) return ToColor(m, x, c, 0);
            if (h < 3) return ToColor(m, 0, c, x);
            if (h < 4) return ToColor(m, 0, x, c);
            if (h < 5) return ToColor(m, x, 0, c);

            return ToColor(m, c, 0, x);
        }

        private static Color ToColor(double m, double red, double green, double blue)
        {
            int r = (int)(255 * (red + m));
            int g = (int)(255 * (green + m));
            int b = (int)(255 * (blue + m));

            return Color.FromArgb(255, r, g, b);
        }
    }
}
