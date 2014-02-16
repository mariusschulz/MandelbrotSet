
namespace MandelbrotSet.Models
{
    public struct IterationValue
    {
        public int Depth { get; private set; }
        public double AbsoluteValue { get; private set; }

        public IterationValue(int depth, double absoluteValue)
            : this()
        {
            Depth = depth;
            AbsoluteValue = absoluteValue;
        }
    }
}
