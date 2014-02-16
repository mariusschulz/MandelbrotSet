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

        public IterationValue ComputeIterationDepthFor(ComplexNumber z)
        {
            ComplexNumber c = z;

            for (int n = 0; n < _maxIterationDepth; n++)
            {
                z.MultiplyWith(z);
                z.Add(c);

                if (z.AbsoluteValueSquared >= _thresholdSquared)
                    return new IterationValue(n + 1, z.AbsoluteValue);
            }

            return new IterationValue(_maxIterationDepth, double.MinValue);
        }
    }
}
