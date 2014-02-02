
namespace MandelbrotSet.Models
{
    public struct ComplexNumber
    {
        private readonly double _realPart;
        public double RealPart { get { return _realPart; } }

        private readonly double _imaginaryPart;
        public double ImaginaryPart { get { return _imaginaryPart; } }

        public double AbsoluteValueSquared
        {
            get { return RealPart * RealPart + ImaginaryPart * ImaginaryPart; }
        }

        public ComplexNumber(double realPart, double imaginaryPart)
        {
            _realPart = realPart;
            _imaginaryPart = imaginaryPart;
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
