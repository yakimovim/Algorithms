using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EdlinSoftware.DataStructures.Strings;
using JetBrains.Annotations;

namespace EdlinSoftware.Algorithms.Strings
{
    /// <summary>
    /// Searches all matches of pattern strings in a text with a given number of errors
    /// using multiple threads.
    /// </summary>
    /// <typeparam name="TSymbol">Type of symbols.</typeparam>
    public static class MultithreadedSuffixTreeApproximateSearch<TSymbol>
    {
        private class TaskDescription
        {
            public TaskDescription(ConcurrentBag<StringSearchApproximateMatch<TSymbol>> resultsBag, Waiter waiter, SuffixTreeNode<TSymbol> startNode, IReadOnlyList<TSymbol> pattern, int patternStartIndex, IReadOnlyList<TSymbol> text, uint numberOfErrors, IComparer<TSymbol> comparer)
            {
                ResultsBag = resultsBag;
                Waiter = waiter;
                StartNode = startNode;
                Pattern = pattern;
                PatternStartIndex = patternStartIndex;
                Text = text;
                NumberOfErrors = numberOfErrors;
                Comparer = comparer;
            }

            public ConcurrentBag<StringSearchApproximateMatch<TSymbol>> ResultsBag { get; }
            public Waiter Waiter { get; }
            public SuffixTreeNode<TSymbol> StartNode { get; }
            public IReadOnlyList<TSymbol> Pattern { get; }
            public int PatternStartIndex { get; }
            public IReadOnlyList<TSymbol> Text { get; }
            public uint NumberOfErrors { get; }
            public IComparer<TSymbol> Comparer { get; }
        }

        private class EdgeMatch
        {
            public EdgeMatch(int matchLength, uint numberOfErrors)
            {
                MatchLength = matchLength;
                NumberOfErrors = numberOfErrors;
            }

            public int MatchLength { get; }
            public uint NumberOfErrors { get; }
        }

        private class Waiter
        {
            private readonly object _lock = new object();
            private int _counter;

            public void Increment()
            {
                lock (_lock)
                {
                    _counter++;
                }
            }

            public void Decrement()
            {
                lock (_lock)
                {
                    _counter--;
                }
            }

            public bool IsFinished()
            {
                lock (_lock)
                {
                    return _counter == 0;
                }

            }
        }

        /// <summary>
        /// Returns enumeration of matches of words in <paramref name="patterns"/> in the <paramref name="text"/> string.
        /// </summary>
        /// <param name="text">String to search the words in <paramref name="patterns"/> in.</param>
        /// <param name="stopSymbol">Stop symbol different from all symbols of <paramref name="text"/>.</param>
        /// <param name="patterns">Patterns to search for.</param>
        /// <param name="numberOfErrors">Maximum number of errors in a match.</param>
        /// <param name="comparer">Comparer of symbols.</param>
        public static IEnumerable<StringSearchApproximateMatch<TSymbol>> Search(
            [NotNull] IReadOnlyList<TSymbol> text,
            TSymbol stopSymbol,
            IEnumerable<IEnumerable<TSymbol>> patterns,
            uint numberOfErrors,
            IComparer<TSymbol> comparer = null)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            if (patterns == null)
                return new StringSearchApproximateMatch<TSymbol>[0];

            comparer = new StopSymbolFirstComparer<TSymbol>(comparer ?? Comparer<TSymbol>.Default, stopSymbol);

            var suffixTree = SuffixTreeCreator<TSymbol>.CreateSuffixTree(text, stopSymbol, comparer);

            var waiter = new Waiter();

            var results = new ConcurrentBag<StringSearchApproximateMatch<TSymbol>>();

            foreach (var pattern in patterns)
            {
                waiter.Increment();

                var task = new TaskDescription(
                    results,
                    waiter,
                    suffixTree.Root,
                    pattern.ToArray(),
                    0,
                    suffixTree.Text,
                    numberOfErrors,
                    comparer);

                ThreadPool.QueueUserWorkItem(state =>
                {
                    ProcessSearchTask(task);
                });
            }

            while (!waiter.IsFinished())
            {
                Thread.Sleep(0);
            }

            return results;
        }

        private static bool PatternDoesNotOverlapStopSymbol(int startPosition, int patternLength, int textLength)
        {
            if (startPosition >= textLength - 1)
                return false;
            if (patternLength > 0 && startPosition + patternLength >= textLength)
                return false;
            return true;
        }

        private static void ProcessSearchTask(TaskDescription task)
        {
            if (task.PatternStartIndex >= task.Pattern.Count)
            {
                foreach (var start in GetSuffixStarts(task.StartNode).Where(s => PatternDoesNotOverlapStopSymbol(s, task.Pattern.Count, task.Text.Count)))
                {
                    task.ResultsBag.Add(new StringSearchApproximateMatch<TSymbol>(start, task.Pattern));
                }
                task.Waiter.Decrement();
                return;
            }

            foreach (var edge in task.StartNode.Edges.Values)
            {
                EdgeMatch edgeMatch = GetEdgeMatch(edge, task.Text, task.Pattern, task.PatternStartIndex, task.Comparer);

                if(edgeMatch.NumberOfErrors > task.NumberOfErrors)
                    continue;

                var newPatternStartIndex = task.PatternStartIndex + edgeMatch.MatchLength;

                if (newPatternStartIndex >= task.Pattern.Count)
                {
                    foreach (var start in GetSuffixStarts(edge.To).Where(s => PatternDoesNotOverlapStopSymbol(s, task.Pattern.Count, task.Text.Count)))
                    {
                        task.ResultsBag.Add(new StringSearchApproximateMatch<TSymbol>(start, task.Pattern));
                    }
                    continue;
                }

                var newTask = new TaskDescription(
                    task.ResultsBag,
                    task.Waiter,
                    edge.To,
                    task.Pattern,
                    newPatternStartIndex,
                    task.Text,
                    task.NumberOfErrors - edgeMatch.NumberOfErrors,
                    task.Comparer
                    );

                task.Waiter.Increment();

                ThreadPool.QueueUserWorkItem(state =>
                {
                    ProcessSearchTask(newTask);
                });
            }

            task.Waiter.Decrement();
        }

        private static EdgeMatch GetEdgeMatch(
            SuffixTreeEdge<TSymbol> edge, 
            IReadOnlyList<TSymbol> text, 
            IReadOnlyList<TSymbol> pattern, 
            int patternStartIndex,
            IComparer<TSymbol> comparer)
        {
            int matchLength = 0;
            uint numberOfErrors = 0;

            while (true)
            {
                var textIndex = edge.Start + matchLength;
                var pattentIndex = patternStartIndex + matchLength;
                if(matchLength >= edge.Length)
                    break;
                if(pattentIndex >= pattern.Count)
                    break;

                if (comparer.Compare(text[(int) textIndex], pattern[pattentIndex]) != 0)
                    numberOfErrors++;

                matchLength++;
            }

            return new EdgeMatch(matchLength, numberOfErrors);
        }

        private static IEnumerable<int> GetSuffixStarts(SuffixTreeNode<TSymbol> node)
        {
            var queue = new Queue<SuffixTreeNode<TSymbol>>();
            queue.Enqueue(node);

            while (queue.Count > 0)
            {
                node = queue.Dequeue();
                if (node.SuffixStart.HasValue)
                    yield return (int) node.SuffixStart.Value;

                foreach (var edge in node.Edges.Values)
                {
                    queue.Enqueue(edge.To);
                }
            }
        }
    }
}