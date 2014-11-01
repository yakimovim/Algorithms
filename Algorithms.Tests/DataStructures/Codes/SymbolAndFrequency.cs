
namespace EdlinSoftware.Tests.DataStructures.Codes
{
    public class SymbolAndFrequency<TSymbol>
    {
        public SymbolAndFrequency(TSymbol symbol, double frequency)
        {
            Symbol = symbol;
            Frequency = frequency;
        }

        public TSymbol Symbol { get; private set; }

        public double Frequency { get; private set; }
    }
}
