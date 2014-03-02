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

            Assert.AreEqual(graph.Points.Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestCreateGraphWithList_NullList()
        {
            var graph = factory.GenerateGraphWithList(new Point(0, 0), null);
        }

        [TestMethod]
        public void TestCreateGraphWithList_1Point()
        {
            var points = new List<Point>
            {
                new Point(0, 0)
            };

            var graph = factory.GenerateGraphWithList(new Point(0, 0), points);

            Assert.AreEqual(graph.Points.Count, 1);
        }

        [TestMethod]
        public void TestCreateGraphWithList_4Points()
        {
            var points = new List<Point>
            {
                new Point(0, 0),
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0)
            };

            var graph = factory.GenerateGraphWithList(new Point(0, 0), points);

            Assert.AreEqual(graph.Points.Count, 4);
        }

        [TestMethod]
        public void TestCreateGraphWithList_4PointsConnected()
        {
            var p1 = new Point(0, 0);
            var p2 = new Point(0, 1);
            var p3 = new Point(1, 1);
            var p4 = new Point(1, 0);

            var points = new List<Point>
            {
                p1, p2, p3, p4
            };

            var graph = factory.GenerateGraphWithList(new Point(0, 0), points);

            // Connections of first point
            Assert.AreEqual(graph.Points[0].Prev.Position, p4);
            Assert.AreEqual(graph.Points[0].Next.Position, p2);

            // Connections of second point
            Assert.AreEqual(graph.Points[1].Prev.Position, p1);
            Assert.AreEqual(graph.Points[1].Next.Position, p3);

            // Connections of third point
            Assert.AreEqual(graph.Points[2].Prev.Position, p2);
            Assert.AreEqual(graph.Points[2].Next.Position, p4);

            // Connections of fourth point
            Assert.AreEqual(graph.Points[3].Prev.Position, p3);
            Assert.AreEqual(graph.Points[3].Next.Position, p1);
        }
        
        [TestMethod]
        public void TestCreateGraphWithList_2PointsConnected()
        {
            var p1 = new Point(0, 0);
            var p2 = new Point(0, 1);

            var points = new List<Point>
            {
                p1, p2
            };

            var graph = factory.GenerateGraphWithList(new Point(0, 0), points);

            // Connections of first point
            Assert.AreEqual(graph.Points[0].Prev.Position, p2);
            Assert.AreEqual(graph.Points[0].Next.Position, p2);

            // Connections of second point
            Assert.AreEqual(graph.Points[1].Prev.Position, p1);
            Assert.AreEqual(graph.Points[1].Next.Position, p1);
        }
    }
}
