namespace MandelbrotSet.Models
{
    public class MandelbrotComputer
    {
        private readonly int _maxIterationDepth;
        private readonly double _threshold;

        public MandelbrotComputer(int maxIterationDepth, double threshold)
        {
            _maxIterationDepth = maxIterationDepth;
            _threshold = threshold;
        }

        public int ComputeIterationDepthFor(ComplexNumber z)
        {
            ComplexNumber c = z;

            for (int n = 0; n < _maxIterationDepth; n++)
            {
                z = z.Multiply(z).Add(c);

                if (z.AbsoluteValueSquared >= _threshold * _threshold)
                    return n + 1;
            }

            return _maxIterationDepth;
        }
    }
}