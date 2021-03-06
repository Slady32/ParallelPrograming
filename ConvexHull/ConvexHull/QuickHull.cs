﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConvexHull
{
    public abstract class QuickHull : IHull
    {
        public IGraph Graph
        {
            get
            {
                return _graph;
            }
        }

        private readonly IGraph _graph;
        public event EventHandler<TimeSpan> Done;

        public QuickHull(IGraph graph)
        {
            _graph = graph;
        }

        public void Execute()
        {
            var watch = new Stopwatch();
            watch.Start();

            Node leftNode = null, rightNode = null;

            FindMinMaxX(_graph.Points, out leftNode, out rightNode);
            _graph.HullNodes.Add(leftNode);
            _graph.HullNodes.Add(rightNode);

            // upperHalf is under the line (y >= value)
            var upperHalf = new List<Node>();
            // lowerHalf is over the line (y < value)
            var lowerHalf = new List<Node>();

            Split(_graph.Points, lowerHalf, upperHalf, leftNode, rightNode);

            FindMinY(lowerHalf, leftNode, rightNode);
            FindMaxY(upperHalf, rightNode, leftNode);

            watch.Stop();

            OnDone(watch.Elapsed);
        }

        protected void ExecuteThread(int maxThreads, Action action)
        {
            var threads = Enumerable.Range(0, maxThreads).Select(t => new Thread(() => action())).ToList();

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());
        }

        protected void FindMinMaxX(IList<Point> points, out Node leftNode, out Node rightNode)
        {
            leftNode = new Node(points.FirstOrDefault(p => p.X == points.Min(po => po.X)));
            rightNode = new Node(points.FirstOrDefault(p => p.X == points.Max(po => po.X)));

            leftNode.Next = rightNode;
            rightNode.Next = leftNode;
        }

        protected abstract void ExecuteSplit(IList<Point> points, IList<Node> lowerHalf, IList<Node> upperHalf, Node leftNode, Node rightNode);

        protected void SplitLowerHalf(IList<Point> points, IList<Node> lowerHalf, IList<Node> upperHalf, Node leftNode, Node rightNode)
        {
            Split(points.Take(points.Count / 2).ToList(), lowerHalf, upperHalf, leftNode, rightNode);
        }

        protected void SplitUpperHalf(IList<Point> points, IList<Node> lowerHalf, IList<Node> upperHalf, Node leftNode, Node rightNode)
        {
            Split(points.Skip(points.Count / 2).ToList(), lowerHalf, upperHalf, leftNode, rightNode);
        }

        protected void Split(IList<Point> points, IList<Node> lowerHalf, IList<Node> upperHalf, Node leftNode, Node rightNode)
        {
            // calculate y for x-value
            foreach (var point in points)
            {
                var curNode = new Node(point);
                if (curNode.Position == leftNode.Position || curNode.Position == rightNode.Position)
                {
                    continue;
                }

                // Calculate Differences
                int xDif = rightNode.Position.X - leftNode.Position.X;
                int yDif = leftNode.Position.Y - rightNode.Position.Y;

                // == Steigung
                float slope = (1f * yDif) / (1f * xDif);
                float yHypo = (curNode.Position.X - leftNode.Position.X) * (slope * -1) + leftNode.Position.Y;

                if (curNode.Position.Y >= yHypo)
                {
                    if (upperHalf != null)
                    {
                        upperHalf.Add(curNode);
                    }
                }
                else
                {
                    if (lowerHalf != null)
                    {
                        lowerHalf.Add(curNode);
                    }
                }
            }
        }

        protected abstract void ExecuteFindMinY(IList<Node> list, Node prevNode, Node nextNode);
        protected abstract void ExecuteFindMaxY(IList<Node> list, Node prevNode, Node nextNode);

        protected void FindMinY(IList<Node> list, Node prevNode, Node nextNode)
        {
            var nodeLengthPairs = new Dictionary<Node, float>();

            FindExtremeY(list, nodeLengthPairs, prevNode, nextNode);

            var curNode = nodeLengthPairs.FirstOrDefault(n => n.Value == nodeLengthPairs.Max(nl => nl.Value)).Key;
            if (curNode != null)
            {
                _graph.HullNodes.Add(curNode);
                prevNode.Next = curNode;
                curNode.Next = nextNode;

                // lowerHalf is over the line (y < value)
                var lowerHalf = new List<Node>();

                // all between prev and cur point
                var positions = nodeLengthPairs.Keys.Where(p => p.Position.X < curNode.Position.X).Select(p => p.Position);
                var points = new List<Point>(positions);

                ExecuteSplit(points, lowerHalf, null, prevNode, curNode);
                if (lowerHalf.Count > 0)
                {
                    FindMinY(lowerHalf, prevNode, curNode);
                }

                // all between cur and next point
                lowerHalf = new List<Node>();

                positions = nodeLengthPairs.Keys.Where(p => p.Position.X > curNode.Position.X).Select(p => p.Position);
                points = new List<Point>(positions);

                ExecuteSplit(points, lowerHalf, null, curNode, nextNode);
                if (lowerHalf.Count > 0)
                {
                    FindMinY(lowerHalf, curNode, nextNode);
                }
            }
        }

        protected void FindMaxY(IList<Node> list, Node prevNode, Node nextNode)
        {
            var nodeLengthPairs = new Dictionary<Node, float>();

            FindExtremeY(list, nodeLengthPairs, prevNode, nextNode);
            var curNode = nodeLengthPairs.FirstOrDefault(n => n.Value == nodeLengthPairs.Max(nl => nl.Value)).Key;
            if (curNode != null)
            {
                _graph.HullNodes.Add(curNode);
                prevNode.Next = curNode;
                curNode.Next = nextNode;

                var upperHalf = new List<Node>();
                // all between prev and cur point
                var positions = nodeLengthPairs.Keys.Where(p => p.Position.X > curNode.Position.X).Select(p => p.Position);
                var points = new List<Point>(positions);

                ExecuteSplit(points, null, upperHalf, prevNode, curNode);
                if (upperHalf.Count > 0)
                {
                    FindMaxY(upperHalf, prevNode, curNode);
                }

                upperHalf = new List<Node>();
                // all between cur and next point
                positions = nodeLengthPairs.Keys.Where(p => p.Position.X < curNode.Position.X).Select(p => p.Position);
                points = new List<Point>(positions);

                ExecuteSplit(points, null, upperHalf, curNode, nextNode);
                if (upperHalf.Count > 0)
                {
                    FindMaxY(upperHalf, curNode, nextNode);
                }
            }
        }

        protected void FindExtremeY(IList<Node> list, Dictionary<Node, float> nodeLengthPairs, Node prevNode, Node nextNode)
        {
            foreach (var curNode in list)
            {
                float length = GetTriangleHeight(prevNode.Position, nextNode.Position, curNode.Position);
                nodeLengthPairs.Add(curNode, length);
            }
        }

        private float GetTriangleHeight(Point A, Point B, Point C)
        {
            var dAB = GetDistance(A, B);
            var dAC = GetDistance(A, C);
            var dBC = GetDistance(B, C);
            var betaAngle = Math.Acos(((Math.Pow(dAB, 2) + Math.Pow(dBC, 2) - Math.Pow(dAC, 2)) / (2 * dAB * dBC)));

            var retVal = (float)(dBC * Math.Sin(betaAngle));

            return (retVal < 0) ? -retVal : retVal;
        }

        private double GetDistance(Point A, Point B)
        {
            return Math.Sqrt(Math.Pow(A.X - B.X, 2) + Math.Pow(A.Y - B.Y, 2));
        }

        private void OnDone(TimeSpan elapsed)
        {
            var temp = Done;
            if (temp != null)
            {
                temp(this, elapsed);
            }
        }
    }
}
