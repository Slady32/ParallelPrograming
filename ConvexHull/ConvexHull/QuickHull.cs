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

        private Node _leftNode;
        private Node _rightNode;

        private static readonly object _lockObject = new object();
        private int _index = 0;

        private ConcurrentDictionary<int, int> _indices;

        public QuickHull(Graph graph)
        {
            _graph = graph;
            _indices = new ConcurrentDictionary<int, int>();
        }

        public void Execute()
        {
            FindMinMaxX();

            // upperHalf is under the line (y >= value)
            var upperHalf = new ConcurrentBag<Node>();
            // lowerHalf is over the line (y < value)
            var lowerHalf = new ConcurrentBag<Node>();
            _indices.AddOrUpdate(0, 0, (k, v) => 0);
            ExecuteThread(_graph.Points.Count, () => Split(lowerHalf, upperHalf));

            _indices.AddOrUpdate(1, 0, (k, v) => v);
            //ExecuteThread(lowerHalf.Count, () => FindMinY(lowerHalf, 1,_leftNode, _rightNode));
            FindMinY(lowerHalf, 1, _leftNode, _rightNode);
            _indices.AddOrUpdate(2, 0, (k, v) => v);
            FindMaxY(upperHalf, 2, _rightNode, _leftNode);
            //ExecuteThread(upperHalf.Count, () => FindMaxY(upperHalf));

            Console.WriteLine();
        }

        private void ExecuteThread(int maxThreads, Action action)
        {
            var threads = Enumerable.Range(0, maxThreads).Select(t => new Thread(() => action())).ToList();

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());
        }

        private void FindMinMaxX()
        {
            _leftNode = new Node(_graph.Points.FirstOrDefault(p => p.X == _graph.Points.Min(po => po.X)));
            _graph.HullNodes.Add(_leftNode);

            _rightNode = new Node(_graph.Points.FirstOrDefault(p => p.X == _graph.Points.Max(po => po.X)));
            _graph.HullNodes.Add(_rightNode);

            _leftNode.Next = _rightNode;
            _rightNode.Next = _leftNode;
        }

        private void Split(ConcurrentBag<Node> lowerHalf, ConcurrentBag<Node> upperHalf)
        {
            // calculate y for x-value
            Node curNode = null;
            lock (_lockObject)
            {
                int index = _indices.SingleOrDefault(i => i.Key == 0).Value;
                curNode = new Node(_graph.Points[index]);
                _indices.TryUpdate(0, index + 1, index);
                if (curNode.Position == _leftNode.Position || curNode.Position == _rightNode.Position)
                {
                    return;
                }
            }

            // insert wheter >= y or < y
            int xDif = _rightNode.Position.X - _leftNode.Position.X;
            int yDif = Math.Max(_leftNode.Position.Y, _rightNode.Position.Y) - Math.Min(_leftNode.Position.Y, _rightNode.Position.Y);
            // == steigung
            float slope = (1f * yDif) / (1f * xDif);

            float yHypo = (_rightNode.Position.X - curNode.Position.X) * slope + _rightNode.Position.Y;

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

            FindExtremeY(list, nodeLengthPairs, indexIndex, prevNode, nextNode, li => li.Min(po => po.Position.Y));

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

            FindExtremeY(list, nodeLengthPairs, indexIndex, prevNode, nextNode, li => li.Min(po => po.Position.Y));
            var curNode = nodeLengthPairs.FirstOrDefault(n => n.Value == nodeLengthPairs.Max(nl => nl.Value)).Key;
            if (curNode != null)
            {
                _graph.HullNodes.Add(curNode);
                prevNode.Next = curNode;
                curNode.Next = nextNode;
            }
        }

        private void FindExtremeY(ConcurrentBag<Node> list, ConcurrentDictionary<Node, float> nodeLengthPairs, int indexIndex, Node prevNode, Node nextNode, Func<ConcurrentBag<Node>, int> func)
        {
            var notConcurrentBagList = list.ToList();

            ExecuteThread(notConcurrentBagList.Count, () => CalculateNodeLengthParis(notConcurrentBagList, indexIndex, prevNode, nextNode, nodeLengthPairs));
        }

        private void CalculateNodeLengthParis(List<Node> nodeList, int indexIndex, Node prevNode, Node nextNode, ConcurrentDictionary<Node, float> nodeLengthPairs)
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
