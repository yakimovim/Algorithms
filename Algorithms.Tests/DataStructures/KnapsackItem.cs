
namespace EdlinSoftware.Tests.DataStructures
{
    public class KnapsackItem
    {
        public KnapsackItem(double value, long size)
        {
            Value = value;
            Size = size;
        }

        public double Value { get; private set; }

        public long Size { get; private set; }
    }
}
