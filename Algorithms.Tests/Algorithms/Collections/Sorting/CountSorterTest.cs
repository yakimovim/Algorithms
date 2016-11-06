using System;
using System.Linq;
using EdlinSoftware.Algorithms.Collections.Sorting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections.Sorting
{
    [TestClass]
    public class CountSorterTest
    {
        private CountSorter<char> _sorter;

        [TestInitialize]
        public void TestInitialize()
        {
            _sorter = new CountSorter<char>();
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Sort_ShouldThrowException_IfArrayIsNull()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            _sorter.Sort(null);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Sort_ShouldReturnSameArray_IfInputArrayIsEmpty()
        {
            var sorted = _sorter.Sort(new char[0]);

            Assert.AreEqual(0, sorted.Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Sort_ShouldReturnSameArray_IfInputArrayHasOneElement()
        {
            var sorted = _sorter.Sort(new[] { 'A' });

            CollectionAssert.AreEqual(new[] { 'A' }, sorted);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Sort_ShouldSortArray_OfEvenLength()
        {
            var sorted = _sorter.Sort("ABABAB".ToCharArray());

            Assert.AreEqual("AAABBB", new string(sorted));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Sort_ShouldSortArray_OfOddLength()
        {
            var sorted = _sorter.Sort("ABABABA".ToCharArray());

            Assert.AreEqual("AAAABBB", new string(sorted));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Sort_ShouldSortArray_ForLongStrings()
        {
            var letters = "ACGT".ToCharArray();

            var symbols = new char[10000];

            var rnd = new Random((int)DateTime.UtcNow.Ticks);

            for (int i = 0; i < symbols.Length; i++)
            {
                symbols[i] = letters[rnd.Next(letters.Length)];
            }

            var expected = _sorter.Sort(symbols);

            var actual = symbols.OrderBy(c => c).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}