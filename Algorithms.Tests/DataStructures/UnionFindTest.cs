using System.Linq;
using EdlinSoftware.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.DataStructures
{
    [TestClass]
    public class UnionFindTest
    {
        private UnionFind<int> _unionFind;

        [TestInitialize]
        public void TestInitialize()
        {
            _unionFind = new UnionFind<int>();
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void ElementsCount_ShouldBeZero_ForEmptyUnionFind()
        {
            Assert.AreEqual(0, _unionFind.ElementsCount);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GroupsCount_ShouldBeZero_ForEmptyUnionFind()
        {
            Assert.AreEqual(0, _unionFind.GroupsCount);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Add_ShouldBeAbleToAddOneItem()
        {
            var elements = _unionFind.Add(1);

            Assert.AreEqual(1, _unionFind.ElementsCount);
            Assert.AreEqual(1, _unionFind.GroupsCount);
            Assert.AreEqual(1, elements.Length);
            CollectionAssert.AreEqual(new[] { 1 }, elements.Select(e => e.Item).ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Add_ShouldBeAbleToAddManyItems()
        {
            var elements = _unionFind.Add(1, 2, 3, 4);

            Assert.AreEqual(4, _unionFind.ElementsCount);
            Assert.AreEqual(4, _unionFind.GroupsCount);
            Assert.AreEqual(4, elements.Length);
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4 }, elements.Select(e => e.Item).ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Elements_ShouldReturnCollectionOfElements()
        {
            _unionFind.Add(1, 2, 3, 4);
            _unionFind.Add(5);

            var elements = _unionFind.Elements.ToArray();

            Assert.AreEqual(5, elements.Length);
            CollectionAssert.AreEquivalent(new[] { 1, 2, 3, 4, 5 }, elements.Select(e => e.Item).ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Groups_ShouldReturnCollectionOfGroups()
        {
            _unionFind.Add(1, 2, 3, 4);
            _unionFind.Add(5);

            var groups = _unionFind.Groups.ToArray();

            Assert.AreEqual(5, groups.Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void EachGroupShouldInitiallyContainOneElement()
        {
            _unionFind.Add(1, 2, 3, 4);
            _unionFind.Add(5);

            var groups = _unionFind.Groups.ToArray();

            foreach (var group in groups)
            {
                Assert.AreEqual(1, group.ElementsCount);
            }
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GroupOfElement_ShouldReturnGroupOfElement()
        {
            var elements = _unionFind.Add(1, 2, 3, 4).ToArray();

            var group = elements[2].Group;

            Assert.IsNotNull(group);
            Assert.AreEqual(1, group.ElementsCount);

            var groupElements = group.Elements.ToArray();

            CollectionAssert.AreEquivalent(new[] { elements[2] }, groupElements);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Union_ShouldNotChangeElementsCount()
        {
            var elements = _unionFind.Add(1, 2, 3, 4).ToArray();

            _unionFind.Union(elements[1], elements[2]);

            Assert.AreEqual(4, _unionFind.ElementsCount);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Union_ShouldReduceGroupsCountByOne()
        {
            var elements = _unionFind.Add(1, 2, 3, 4).ToArray();

            _unionFind.Union(elements[1], elements[2]);

            Assert.AreEqual(3, _unionFind.GroupsCount);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Union_UnionedElementsMustHaveTheSameGroup()
        {
            var elements = _unionFind.Add(1, 2, 3, 4).ToArray();

            _unionFind.Union(elements[1], elements[2]);

            Assert.AreSame(elements[1].Group, elements[2].Group);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Union_UnionedGroupShouldContainUnionedElements()
        {
            var elements = _unionFind.Add(1, 2, 3, 4).ToArray();

            _unionFind.Union(elements[1], elements[2]);

            var group = elements[1].Group;

            Assert.AreEqual(2, group.ElementsCount);
            CollectionAssert.AreEquivalent(new[] { elements[1], elements[2] }, group.Elements.ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Union_ShouldSupportComplexUnion()
        {
            var elements = _unionFind.Add(1, 2, 3, 4).ToArray();

            _unionFind.Union(elements[0], elements[1]);
            _unionFind.Union(elements[2], elements[3]);
            _unionFind.Union(elements[0], elements[2]);

            var group = elements[1].Group;

            Assert.AreEqual(4, _unionFind.ElementsCount);
            Assert.AreEqual(1, _unionFind.GroupsCount);
            Assert.AreEqual(4, group.ElementsCount);
            CollectionAssert.AreEquivalent(elements, group.Elements.ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Union_ShouldNotChangeElementsFromTheSameGroup()
        {
            var elements = _unionFind.Add(1, 2, 3, 4).ToArray();

            _unionFind.Union(elements[0], elements[1]);
            _unionFind.Union(elements[1], elements[0]);

            var group = elements[1].Group;

            Assert.AreEqual(4, _unionFind.ElementsCount);
            Assert.AreEqual(3, _unionFind.GroupsCount);
            Assert.AreEqual(2, group.ElementsCount);
            CollectionAssert.AreEquivalent(new[] { elements[1], elements[0] }, group.Elements.ToArray());
        }
    }
}
