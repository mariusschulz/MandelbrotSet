
namespace MandelbrotSet.Models
{
    public struct ComplexNumber
    {
        public double RealPart { get; private set; }
        public double ImaginaryPart { get; private set; }

        public double AbsoluteValueSquared
        {
            get
            {
                return RealPart * RealPart + ImaginaryPart * ImaginaryPart;
            }
        }

        public ComplexNumber(double realPart, double imaginaryPart)
            : this()
        {
            RealPart = realPart;
            ImaginaryPart = imaginaryPart;
        }

        public ComplexNumber Add(ComplexNumber other)
        {
            return new ComplexNumber(RealPart + other.RealPart, ImaginaryPart + other.ImaginaryPart);
        }

        public ComplexNumber Multiply(ComplexNumber other)
        {
            return new ComplexNumber(
                RealPart * other.RealPart - ImaginaryPart * other.ImaginaryPart,
                RealPart * other.ImaginaryPart + ImaginaryPart * other.RealPart);
        }
    }
}
