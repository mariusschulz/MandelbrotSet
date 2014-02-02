namespace MandelbrotSet.Models
{
    public class MandelbrotComputer
    {
        private readonly int _maxIterationDepth;
        private readonly double _thresholdSquared;

        public MandelbrotComputer(int maxIterationDepth, double threshold)
        {
            _maxIterationDepth = maxIterationDepth;
            _thresholdSquared = threshold * threshold;
        }

        public int ComputeIterationDepthFor(ComplexNumber z)
        {
            ComplexNumber c = z;

            for (int n = 0; n < _maxIterationDepth; n++)
            {
                z.MultiplyWith(z);
                z.Add(c);

                if (z.AbsoluteValueSquared >= _thresholdSquared)
                    return n + 1;
            }

            return _maxIterationDepth;
        }
    }
}
