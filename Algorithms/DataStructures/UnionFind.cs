using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EdlinSoftware.DataStructures
{
    /// <summary>
    /// Represents UnionFind structure.
    /// </summary>
    /// <typeparam name="TItem">Type of item.</typeparam>
    public class UnionFind<TItem>
    {
        private class UnionFindElement : IUnionFindElement<TItem>, IUnionFindGroup<TItem>
        {
            private readonly HashSet<UnionFindElement> _attachedElements = new HashSet<UnionFindElement>();

            public UnionFindElement()
            {
                Leader = this;
            }

            public UnionFindElement Leader { get; set; }

            /// <summary>
            /// Gets item of the element.
            /// </summary>
            public TItem Item { get; set; }

            /// <summary>
            /// Gets number of elements below this element.
            /// </summary>
            public int ElementsCount => Elements.Count();

            /// <summary>
            /// Returns group if this element.
            /// </summary>
            public IUnionFindGroup<TItem> Group
            {
                get 
                {
                    AdjustWithParents();

                    return GroupLeader; 
                }
            }

            /// <summary>
            /// Returns all elements below this element.
            /// </summary>
            public IEnumerable<IUnionFindElement<TItem>> Elements
            {
                get 
                { 
                    yield return this;

                    foreach (var element in _attachedElements.SelectMany(e => e.Elements))
                    {
                        yield return element;
                    }
                }
            }

            /// <summary>
            /// Returns depth of tree of subelements.
            /// </summary>
            public int Depth 
            {
                get 
                {
                    var depth = 1;

                    if (_attachedElements.Count > 0)
                    {
                        depth += _attachedElements.Max(e => e.Depth);
                    }

                    return depth;
                }
            }

            /// <summary>
            /// Gets leader of group of this element.
            /// </summary>
            public UnionFindElement GroupLeader
            {
                [DebuggerStepThrough]
                get { return IsLeader ? this : Leader.GroupLeader; }
            }

            public bool IsLeader
            {
                [DebuggerStepThrough]
                get { return ReferenceEquals(Leader, this); }
            }

            public void Attach(UnionFindElement element)
            {
                if (element == null) throw new ArgumentNullException(nameof(element));

                element.Leader = this;
                _attachedElements.Add(element);
            }

            public void Detach(UnionFindElement element)
            {
                if (element == null) throw new ArgumentNullException(nameof(element));

                _attachedElements.Remove(element);
            }

            public void AdjustWithParents()
            {
                var parents = new LinkedList<UnionFindElement>();

                var element = this;

                while (element.IsLeader == false)
                {
                    parents.AddLast(element);
                    element = element.Leader;
                }

                foreach (var parent in parents)
                {
                    parent.AdjustElement();
                }
            }

            private void AdjustElement()
            {
                if (ReferenceEquals(Leader, this)) { return; }
                if (ReferenceEquals(Leader, GroupLeader)) { return; }

                var groupLeader = GroupLeader;
                Leader.Detach(this);
                groupLeader.Attach(this);
            }
        }

        private readonly LinkedList<UnionFindElement> _elements = new LinkedList<UnionFindElement>();

        public UnionFind(params TItem[] items)
        {
            Add(items);
        }

        /// <summary>
        /// Gets number of elements in the structure.
        /// </summary>
        public int ElementsCount { get; private set; }

        /// <summary>
        /// Gets number of groups in the structure.
        /// </summary>
        public int GroupsCount { get; private set; }

        /// <summary>
        /// Gets elements in the structure.
        /// </summary>
        public IEnumerable<IUnionFindElement<TItem>> Elements
        {
            [DebuggerStepThrough]
            get { return _elements; }
        }

        /// <summary>
        /// Gets groups in the structure.
        /// </summary>
        public IEnumerable<IUnionFindGroup<TItem>> Groups
        {
            [DebuggerStepThrough]
            get { return _elements.Where(e => ReferenceEquals(e.GroupLeader, e)); }
        }

        /// <summary>
        /// Adds items to the structure and returns corresponding elements.
        /// </summary>
        /// <param name="items">Items to add.</param>
        /// <returns>Corresponding elements.</returns>
        public IUnionFindElement<TItem>[] Add(params TItem[] items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            var elements = new LinkedList<IUnionFindElement<TItem>>();

            foreach (var item in items)
            {
                var element = new UnionFindElement { Item = item };

                elements.AddLast(element);
                _elements.AddLast(element);
            }

            ElementsCount += items.Length;
            GroupsCount += items.Length;

            return elements.ToArray();
        }

        /// <summary>
        /// Unions groups of given elements into one group.
        /// </summary>
        /// <param name="element1">First element.</param>
        /// <param name="element2">Second element.</param>
        public void Union(IUnionFindElement<TItem> element1, IUnionFindElement<TItem> element2)
        {
            if (element1 == null) throw new ArgumentNullException(nameof(element1));
            if (element2 == null) throw new ArgumentNullException(nameof(element2));

            var internalElement1 = element1 as UnionFindElement;
            if (internalElement1 == null) throw new ArgumentException("element1 is not from this UnionFind structure.", nameof(element1));
            var internalElement2 = element2 as UnionFindElement;
            if (internalElement2 == null) throw new ArgumentException("element2 is not from this UnionFind structure.", nameof(element2));

            internalElement1.AdjustWithParents();
            internalElement2.AdjustWithParents();

            if (ReferenceEquals(internalElement1.GroupLeader, internalElement2.GroupLeader))
            { return; }

            if (internalElement1.GroupLeader.Depth > internalElement2.GroupLeader.Depth)
            {
                internalElement1.GroupLeader.Attach(internalElement2.GroupLeader);
            }
            else
            {
                internalElement2.GroupLeader.Attach(internalElement1.GroupLeader);
            }

            GroupsCount--;
        }
    }

    /// <summary>
    /// Represents element of UnionFind structure.
    /// </summary>
    /// <typeparam name="TItem">Type of item.</typeparam>
    public interface IUnionFindElement<TItem>
    {
        /// <summary>
        /// Gets item of the element.
        /// </summary>
        TItem Item { get; }

        /// <summary>
        /// Gets corresponding group of the element.
        /// </summary>
        IUnionFindGroup<TItem> Group { get; }
    }

    /// <summary>
    /// Represents group of UnionFind structure.
    /// </summary>
    /// <typeparam name="TItem">Type of item.</typeparam>
    public interface IUnionFindGroup<TItem>
    {
        /// <summary>
        /// Gets number of elements in the group.
        /// </summary>
        int ElementsCount { get; }

        /// <summary>
        /// Gets all elements in the group.
        /// </summary>
        IEnumerable<IUnionFindElement<TItem>> Elements { get; }
    }
}
