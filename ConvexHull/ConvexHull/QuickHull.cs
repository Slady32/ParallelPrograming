using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConvexHull
{
    public class QuickHull : IHull
    {
        private readonly Graph _graph;

        private ConcurrentBag<Node> _upperHalf;
        private ConcurrentBag<Node> _lowerHalf;

        private Node _leftNode;
        private Node _rightNode;

        private static readonly object _lockObject = new object();
        private int _index = 0;

        public QuickHull(Graph graph)
        {
            _graph = graph;
        }

        public void Execute()
        {
            FindMinMaxX();

            _upperHalf = new ConcurrentBag<Node>();
            _lowerHalf = new ConcurrentBag<Node>();
            ExecuteThread(_graph.Points.Count, () => Split());
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

            _leftNode.Next = _leftNode.Prev = _rightNode;
            _rightNode.Next = _rightNode.Prev = _leftNode;
        }

        private void Split()
        {
            // calculate y for x-value
            Node curNode = null;
            lock (_lockObject)
            {
                curNode = new Node(_graph.Points[_index++]);
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
                _upperHalf.Add(curNode);
            }
            else
            {
                _lowerHalf.Add(curNode);
            }
        }
    }
}
