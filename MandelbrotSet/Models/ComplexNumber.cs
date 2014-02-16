
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

        public void Add(ComplexNumber number)
        {
            RealPart += number.RealPart;
            ImaginaryPart += number.ImaginaryPart;
        }

        public void MultiplyWith(ComplexNumber number)
        {
            double currentRealPart = RealPart;

            RealPart = RealPart * number.RealPart - ImaginaryPart * number.ImaginaryPart;
            ImaginaryPart = currentRealPart * number.ImaginaryPart + ImaginaryPart * number.RealPart;
        }
    }
}
