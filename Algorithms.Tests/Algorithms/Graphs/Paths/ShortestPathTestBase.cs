using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Graphs.Paths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs.Paths
{
    [TestClass]
    public abstract class ShortestPathTestBase : GraphTestBase
    {
        protected void CheckPath(IPath<long, double, long> path, double value, IEnumerable<long> pathNodes)
        {
            Assert.IsNotNull(path);
            Assert.AreEqual(value, path.Value, 0.000001);
            CollectionAssert.AreEqual(pathNodes.ToArray(), path.Path.ToArray());
        }
    }
}
