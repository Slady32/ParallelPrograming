using System;
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

        public QuickHull(Graph graph)
        {
            _graph = graph;
        }

        public void Execute()
        {
            // TODO Configurable thread count?
            var threads = Enumerable.Range(0, _graph.Points.Count / 2).Select(t => new Thread(() => FindConvexAncestor())).ToList();

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());
        }

        private void FindConvexAncestor()
        {
            // TODO Maybe implement lock
        }

        private void FindMinMaxX()
        {
            var minX = _graph.Points.Min(n => n.X);
            var maxX = _graph.Points.Max(n => n.X);
        }
    }
}
