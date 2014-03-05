using System;
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
    public class QuickHull : IHull
    {
        private readonly Graph _graph;
        
        public QuickHull(Graph graph)
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
            var upperHalf = new ConcurrentQueue<Node>();
            // lowerHalf is over the line (y < value)
            var lowerHalf = new ConcurrentQueue<Node>();

            var points = new ConcurrentQueue<Point>(_graph.Points);
            ExecuteThread(points.Count, () => Split(points, lowerHalf, upperHalf, leftNode, rightNode));

            FindMinY(lowerHalf, leftNode, rightNode);
            FindMaxY(upperHalf, rightNode, leftNode);

            watch.Stop();

            Console.WriteLine("Duration {0}", watch.Elapsed);
        }

        private void ExecuteThread(int maxThreads, Action action)
        {
            var threads = Enumerable.Range(0, maxThreads).Select(t => new Thread(() => action())).ToList();

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());
        }

        private void FindMinMaxX(IList<Point> points, out Node leftNode, out Node rightNode)
        {
            leftNode = new Node(points.FirstOrDefault(p => p.X == points.Min(po => po.X)));
            rightNode = new Node(points.FirstOrDefault(p => p.X == points.Max(po => po.X)));

            leftNode.Next = rightNode;
            rightNode.Next = leftNode;
        }

        private void Split(ConcurrentQueue<Point> points, ConcurrentQueue<Node> lowerHalf, ConcurrentQueue<Node> upperHalf, Node leftNode, Node rightNode)
        {
            // calculate y for x-value
            Point point;
            points.TryDequeue(out point);
            Node curNode = new Node(point);
            if (curNode.Position == leftNode.Position || curNode.Position == rightNode.Position)
            {
                return;
            }

            // insert wheter >= y or < y
            int xDif = rightNode.Position.X - leftNode.Position.X;
            int yDif = Math.Max(leftNode.Position.Y, rightNode.Position.Y) - Math.Min(leftNode.Position.Y, rightNode.Position.Y);
            // == steigung
            float slope = (1f * yDif) / (1f * xDif);

            var x = Math.Max(rightNode.Position.X, curNode.Position.X) - Math.Min(rightNode.Position.X, curNode.Position.X);
            float yHypo = x * slope + rightNode.Position.Y;
            // TODO Berechnung stimmt noch nicht ganz!!!!
            if (curNode.Position.Y >= yHypo)
            {
                if (upperHalf != null)
                {
                    upperHalf.Enqueue(curNode);
                }
            }
            else
            {
                if (lowerHalf != null)
                {
                    lowerHalf.Enqueue(curNode);
                }
            }
        }

        private void FindMinY(ConcurrentQueue<Node> list, Node prevNode, Node nextNode)
        {
            var nodeLengthPairs = new ConcurrentDictionary<Node, float>();

            FindExtremeY(list, nodeLengthPairs, prevNode, nextNode);

            var curNode = nodeLengthPairs.FirstOrDefault(n => n.Value == nodeLengthPairs.Max(nl => nl.Value)).Key;
            if (curNode != null)
            {
                _graph.HullNodes.Add(curNode);
                prevNode.Next = curNode;
                curNode.Next = nextNode;

                // lowerHalf is over the line (y < value)
                var lowerHalf = new ConcurrentQueue<Node>();

                // all between prev and cur point
                var positions = nodeLengthPairs.Keys.Where(p => p.Position.Y <= prevNode.Position.Y && p.Position.X >= prevNode.Position.X && p.Position.X < curNode.Position.X).Select(p => p.Position);
                var points = new ConcurrentQueue<Point>(positions);

                ExecuteThread(points.Count, () => Split(points, lowerHalf, null, prevNode, curNode));
                if (lowerHalf.Count > 0)
                {
                    FindMinY(lowerHalf, prevNode, curNode);
                }

                // all between cur and next point
                lowerHalf = new ConcurrentQueue<Node>();

                positions = nodeLengthPairs.Keys.Where(p => p.Position.Y <= nextNode.Position.Y && p.Position.X <= nextNode.Position.X && p.Position.X > curNode.Position.X).Select(p => p.Position);
                points = new ConcurrentQueue<Point>(positions);

                ExecuteThread(points.Count, () => Split(points, lowerHalf, null, curNode, nextNode));
                if (lowerHalf.Count > 0)
                {
                    FindMinY(lowerHalf, curNode, nextNode);
                }
            }
        }

        private void FindMaxY(ConcurrentQueue<Node> list, Node prevNode, Node nextNode)
        {
            var nodeLengthPairs = new ConcurrentDictionary<Node, float>();

            FindExtremeY(list, nodeLengthPairs, prevNode, nextNode);
            var curNode = nodeLengthPairs.FirstOrDefault(n => n.Value == nodeLengthPairs.Max(nl => nl.Value)).Key;
            if (curNode != null)
            {
                _graph.HullNodes.Add(curNode);
                prevNode.Next = curNode;
                curNode.Next = nextNode;

                var upperHalf = new ConcurrentQueue<Node>();
                // all between prev and cur point
                var positions = nodeLengthPairs.Keys.Where(p => p.Position.Y > prevNode.Position.Y && p.Position.X <= prevNode.Position.X && p.Position.X > curNode.Position.X).Select(p => p.Position);
                var points = new ConcurrentQueue<Point>(positions);

                ExecuteThread(points.Count, () => Split(points, null, upperHalf, prevNode, curNode));
                if (upperHalf.Count > 0)
                {
                    FindMaxY(upperHalf, prevNode, curNode);
                }

                upperHalf = new ConcurrentQueue<Node>();
                // all between cur and next point
                positions = nodeLengthPairs.Keys.Where(p => p.Position.Y > nextNode.Position.Y && p.Position.X >= nextNode.Position.X && p.Position.X < curNode.Position.X).Select(p => p.Position);
                points = new ConcurrentQueue<Point>(positions);

                ExecuteThread(points.Count, () => Split(points, null, upperHalf, curNode, nextNode));
                if (upperHalf.Count > 0)
                {
                    FindMaxY(upperHalf, curNode, nextNode);
                }
            }
        }

        private void FindExtremeY(ConcurrentQueue<Node> list, ConcurrentDictionary<Node, float> nodeLengthPairs, Node prevNode, Node nextNode)
        {
            ExecuteThread(list.Count, () => CalculateNodeLengthPairs(list, prevNode, nextNode, nodeLengthPairs));
        }

        private void CalculateNodeLengthPairs(ConcurrentQueue<Node> nodeList, Node prevNode, Node nextNode, ConcurrentDictionary<Node, float> nodeLengthPairs)
        {
            Node curNode = null;
            nodeList.TryDequeue(out curNode);
            
            float length = GetNormalVectorLength(prevNode.Position, nextNode.Position, curNode.Position);
            nodeLengthPairs.AddOrUpdate(curNode, length, (n, f) => length);            
        }

        private int GetNormalVectorLength(Point A, Point B, Point C)
        {
            int ABx = Math.Max(B.X, A.X) - Math.Min(B.X, A.X);
            int ABy = Math.Max(B.Y, A.Y) - Math.Min(A.Y, B.Y);
            int ACy = Math.Max(A.Y, C.Y) - Math.Min(C.Y, A.Y);
            int ACx = Math.Max(A.X, C.X) - Math.Min(C.X, A.X);
            int lengthNormalVector = ABx * ACy - ABy * ACx;

            return (lengthNormalVector < 0) ? -lengthNormalVector : lengthNormalVector;
        }
    }
}
