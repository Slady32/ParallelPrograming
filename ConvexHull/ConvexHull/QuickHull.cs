using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        private static readonly object _lockObject = new object();

        private ConcurrentDictionary<int, int> _indices;

        public QuickHull(Graph graph)
        {
            _graph = graph;
            _indices = new ConcurrentDictionary<int, int>();
        }

        public void Execute()
        {
            Node leftNode = null, rightNode = null;

            FindMinMaxX(out leftNode, out rightNode);

            // upperHalf is under the line (y >= value)
            var upperHalf = new ConcurrentBag<Node>();
            // lowerHalf is over the line (y < value)
            var lowerHalf = new ConcurrentBag<Node>();
            _indices.AddOrUpdate(0, 0, (k, v) => 0);
            ExecuteThread(_graph.Points.Count, () => Split(_graph.Points, 0, lowerHalf, upperHalf, leftNode, rightNode));

            _indices.AddOrUpdate(1, 0, (k, v) => v);
            //ExecuteThread(lowerHalf.Count, () => FindMinY(lowerHalf, 1,_leftNode, _rightNode));
            FindMinY(lowerHalf, 1, leftNode, rightNode);
            _indices.AddOrUpdate(2, 0, (k, v) => v);
            FindMaxY(upperHalf, 2, rightNode, leftNode);
            //ExecuteThread(upperHalf.Count, () => FindMaxY(upperHalf));

            Console.WriteLine();
        }

        private void ExecuteThread(int maxThreads, Action action)
        {
            var threads = Enumerable.Range(0, maxThreads).Select(t => new Thread(() => action())).ToList();

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());
        }

        private void FindMinMaxX(out Node leftNode, out Node rightNode)
        {
            leftNode = new Node(_graph.Points.FirstOrDefault(p => p.X == _graph.Points.Min(po => po.X)));
            _graph.HullNodes.Add(leftNode);

            rightNode = new Node(_graph.Points.FirstOrDefault(p => p.X == _graph.Points.Max(po => po.X)));
            _graph.HullNodes.Add(rightNode);

            leftNode.Next = rightNode;
            rightNode.Next = leftNode;
        }

        private void Split(IList<Point> points, int indexIndex, ConcurrentBag<Node> lowerHalf, ConcurrentBag<Node> upperHalf, Node leftNode, Node rightNode)
        {
            // calculate y for x-value
            Node curNode = null;
            lock (_lockObject)
            {
                int index = _indices.SingleOrDefault(i => i.Key == indexIndex).Value;
                curNode = new Node(points[index]);
                _indices.TryUpdate(indexIndex, index + 1, index);
                if (curNode.Position == leftNode.Position || curNode.Position == rightNode.Position)
                {
                    return;
                }
            }

            // insert wheter >= y or < y
            int xDif = rightNode.Position.X - leftNode.Position.X;
            int yDif = Math.Max(leftNode.Position.Y, rightNode.Position.Y) - Math.Min(leftNode.Position.Y, rightNode.Position.Y);
            // == steigung
            float slope = (1f * yDif) / (1f * xDif);

            float yHypo = (rightNode.Position.X - curNode.Position.X) * slope + rightNode.Position.Y;

            if (curNode.Position.Y >= yHypo)
            {
                upperHalf.Add(curNode);
            }
            else
            {
                lowerHalf.Add(curNode);
            }
        }

        private void FindMinY(ConcurrentBag<Node> list, int indexIndex, Node prevNode, Node nextNode)
        {
            var nodeLengthPairs = new ConcurrentDictionary<Node, float>();

            FindExtremeY(list, nodeLengthPairs, indexIndex, prevNode, nextNode);

            var curNode = nodeLengthPairs.FirstOrDefault(n => n.Value == nodeLengthPairs.Max(nl => nl.Value)).Key;
            if (curNode != null)
            {
                _graph.HullNodes.Add(curNode);
                prevNode.Next = curNode;
                curNode.Next = nextNode;
            }
        }

        private void FindMaxY(ConcurrentBag<Node> list, int indexIndex, Node prevNode, Node nextNode)
        {
            var nodeLengthPairs = new ConcurrentDictionary<Node, float>();

            FindExtremeY(list, nodeLengthPairs, indexIndex, prevNode, nextNode);
            var curNode = nodeLengthPairs.FirstOrDefault(n => n.Value == nodeLengthPairs.Max(nl => nl.Value)).Key;
            if (curNode != null)
            {
                _graph.HullNodes.Add(curNode);
                prevNode.Next = curNode;
                curNode.Next = nextNode;
            }
        }

        private void FindExtremeY(ConcurrentBag<Node> list, ConcurrentDictionary<Node, float> nodeLengthPairs, int indexIndex, Node prevNode, Node nextNode)
        {
            var notConcurrentBagList = list.ToList();

            ExecuteThread(notConcurrentBagList.Count, () => CalculateNodeLengthPairs(notConcurrentBagList, indexIndex, prevNode, nextNode, nodeLengthPairs));
        }

        private void CalculateNodeLengthPairs(List<Node> nodeList, int indexIndex, Node prevNode, Node nextNode, ConcurrentDictionary<Node, float> nodeLengthPairs)
        {
            Node curNode = null;
            lock (_lockObject)
            {
                int index = _indices.SingleOrDefault(i => i.Key == indexIndex).Value;
                curNode = nodeList[index];
                _indices.TryUpdate(indexIndex, index + 1, index);
            }
            
            float length = GetNormalVectorLength(prevNode.Position, nextNode.Position, curNode.Position);
            nodeLengthPairs.AddOrUpdate(curNode, length, (n, f) => length);            
        }

        private int GetNormalVectorLength(Point A, Point B, Point C)
        {
            int ABx = B.X - A.X;
            int ABy = B.Y - A.Y;
            int lengthNormalVector = ABx * (A.Y - C.Y) - ABy * (A.X - C.X);

            return (lengthNormalVector < 0) ? -lengthNormalVector : lengthNormalVector;
        }
    }
}
