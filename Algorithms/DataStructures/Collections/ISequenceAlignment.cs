namespace EdlinSoftware.DataStructures.Collections
{
    /// <summary>
    /// Represents alignment of two sequences.
    /// </summary>
    /// <typeparam name="TSymbol">Type of symbol.</typeparam>
    public interface ISequenceAlignment<out TSymbol>
    {
        /// <summary>
        /// Gets penalty of alignment.
        /// </summary>
        double Penalty { get; }

        /// <summary>
        /// Gets alignment of the first sequence.
        /// </summary>
        TSymbol[] FirstAlignedSequence { get; }

        /// <summary>
        /// Gets alignment of the second sequence.
        /// </summary>
        TSymbol[] SecondAlignedSequence { get; }
    }

    /// <summary>
    /// Represents alignment of two sequences.
    /// </summary>
    /// <typeparam name="TSymbol">Type of symbol.</typeparam>
    internal class SequenceAlignment<TSymbol> : ISequenceAlignment<TSymbol>
    {
        /// <summary>
        /// Gets penalty of alignment.
        /// </summary>
        public double Penalty { get; set; }

        /// <summary>
        /// Gets alignment of the first sequence.
        /// </summary>
        public TSymbol[] FirstAlignedSequence { get; set; }

        /// <summary>
        /// Gets alignment of the second sequence.
        /// </summary>
        public TSymbol[] SecondAlignedSequence { get; set; }
    }
}
