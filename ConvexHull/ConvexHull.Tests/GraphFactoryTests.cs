using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Collections.Generic;

namespace ConvexHull.Tests
{
    [TestClass]
    public class GraphFactoryTests
    {
        GraphFactory factory;

        [TestInitialize]
        public void SetUp()
        {
            factory = new GraphFactory();
        }

        [TestMethod]
        public void TestCreateGraphWithList_EmptyList()
        {
            var points = new List<Point>();

            var graph = factory.GenerateGraphWithList(new Point(0, 0), points);

            Assert.AreEqual(graph.Nodes.Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestCreateGraphWithList_NullList()
        {
            var graph = factory.GenerateGraphWithList(new Point(0, 0), null);
        }
    }
}
