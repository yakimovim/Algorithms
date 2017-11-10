using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Strings;
using JetBrains.Annotations;

namespace EdlinSoftware.Algorithms.Strings
{
    /// <summary>
    /// Searches all matches of pattern strings in a text with a given number of errors.
    /// </summary>
    /// <typeparam name="TSymbol">Type of symbols.</typeparam>
    public static class SuffixTreeApproximateSearch<TSymbol>
    {
        private class TaskDescription
        {
            public TaskDescription(
                SuffixTreeNode<TSymbol> startNode, 
                IReadOnlyList<TSymbol> pattern, 
                int patternStartIndex, 
                uint numberOfErrors)
            {
                StartNode = startNode;
                Pattern = pattern;
                PatternStartIndex = patternStartIndex;
                NumberOfErrors = numberOfErrors;
            }

            public SuffixTreeNode<TSymbol> StartNode { get; }
            public IReadOnlyList<TSymbol> Pattern { get; }
            public int PatternStartIndex { get; }
            public uint NumberOfErrors { get; }
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

        /// <summary>
        /// Returns enumeration of matches of words in <paramref name="patterns"/> in the <paramref name="text"/> string.
        /// </summary>
        /// <param name="text">String to search the words in <paramref name="patterns"/> in.</param>
        /// <param name="stopSymbol">Stop symbol different from all symbols of <paramref name="text"/>.</param>
        /// <param name="patterns">Patterns to search for.</param>
        /// <param name="numberOfErrors">Maximum number of errors in a match.</param>
        /// <param name="comparer">Comparer of symbols.</param>
        public static IEnumerable<StringSearchMatch> Search(
            [NotNull] IReadOnlyList<TSymbol> text,
            TSymbol stopSymbol,
            IEnumerable<IEnumerable<TSymbol>> patterns,
            uint numberOfErrors,
            IComparer<TSymbol> comparer = null)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            if (patterns == null)
                yield break;

            comparer = new StopSymbolFirstComparer<TSymbol>(comparer ?? Comparer<TSymbol>.Default, stopSymbol);

            var suffixTree = SuffixTreeCreator<TSymbol>.CreateSuffixTree(text, stopSymbol, comparer);

            foreach (var pattern in patterns)
            {
                foreach (var match in GetMatches(pattern.ToArray(), numberOfErrors, suffixTree, comparer))
                {
                    yield return match;
                }
            }
        }

        private static IEnumerable<StringSearchMatch> GetMatches(
            IReadOnlyList<TSymbol> pattern, 
            uint numberOfErrors, 
            SuffixTree<TSymbol> suffixTree,
            IComparer<TSymbol> comparer)
        {
            Queue<TaskDescription> queue = new Queue<TaskDescription>();
            queue.Enqueue(new TaskDescription(
                suffixTree.Root,
                pattern,
                0,
                numberOfErrors));

            LinkedList<SuffixTreeNode<TSymbol>> foundNodes = new LinkedList<SuffixTreeNode<TSymbol>>();

            while (queue.Count > 0)
            {
                TaskDescription task = queue.Dequeue();

                ProcessSearchTask(queue, foundNodes, task, suffixTree.Text, comparer);
            }

            foreach (var start in foundNodes.SelectMany(GetSuffixStarts).Where(s => s < suffixTree.Text.Count - 1))
            {
                if(pattern.Count > 0 && start + pattern.Count >= suffixTree.Text.Count)
                    continue;

                yield return new StringSearchMatch(start, pattern.Count);
            }
        }

        private static void ProcessSearchTask(
            Queue<TaskDescription> queue, 
            LinkedList<SuffixTreeNode<TSymbol>> foundNodes, 
            TaskDescription task, 
            IReadOnlyList<TSymbol> text,
            IComparer<TSymbol> comparer)
        {
            if (task.PatternStartIndex >= task.Pattern.Count)
            {
                foundNodes.AddLast(task.StartNode);
                return;
            }

            foreach (var edge in task.StartNode.Edges.Values)
            {
                EdgeMatch edgeMatch = GetEdgeMatch(edge, text, task.Pattern, task.PatternStartIndex, comparer);

                if(edgeMatch.NumberOfErrors > task.NumberOfErrors)
                    continue;

                var newPatternStartIndex = task.PatternStartIndex + edgeMatch.MatchLength;

                if (newPatternStartIndex >= task.Pattern.Count)
                {
                    foundNodes.AddLast(edge.To);
                    continue;
                }
                
                queue.Enqueue(new TaskDescription(
                    edge.To,
                    task.Pattern,
                    newPatternStartIndex,
                    task.NumberOfErrors - edgeMatch.NumberOfErrors
                    ));
            }
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