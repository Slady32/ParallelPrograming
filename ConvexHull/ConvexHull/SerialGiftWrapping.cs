using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvexHull
{
    class SerialGiftWrapping : IHull
    {
        private Graph _graph;
        public IGraph Graph
        {
            get
            {
                return _graph;
            }
        }


        public SerialGiftWrapping(Graph graph)
        {
            _graph = graph;
        }

        public event EventHandler<TimeSpan> Done;

        public void Execute()
        {
            var watch = new Stopwatch();
            watch.Start();

            Point vPointOnHull = _graph.Points.Where(p => p.X == _graph.Points.Min(min => min.X)).First();

            Point vEndpoint;
            do
            {
                _graph.HullNodes.Add(new Node(vPointOnHull));
                vEndpoint = _graph.Points[0];

                for (int i = 1; i < _graph.Points.Count; i++)
                {
                    if ((vPointOnHull == vEndpoint)
                        || (Orientation(vPointOnHull, vEndpoint, _graph.Points[i]) == -1))
                    {
                        vEndpoint = _graph.Points[i];
                    }
                }

                vPointOnHull = vEndpoint;

            }
            while (vEndpoint != _graph.HullNodes[0].Position);

            for (int i = 0; i < _graph.HullNodes.Count; i++)
            {
                if (i == 0)
                {
                    _graph.HullNodes[i].Prev = _graph.HullNodes[_graph.HullNodes.Count - 1];
                    _graph.HullNodes[i].Next = _graph.HullNodes[i + 1];
                }
                else if (i == _graph.HullNodes.Count - 1)
                {
                    _graph.HullNodes[i].Prev = _graph.HullNodes[i - 1];
                    _graph.HullNodes[i].Next = _graph.HullNodes[0];
                }
                else
                {
                    _graph.HullNodes[i].Prev = _graph.HullNodes[i - 1];
                    _graph.HullNodes[i].Next = _graph.HullNodes[i + 1];
                }
            }

            watch.Stop();

            OnDone(watch.Elapsed);
        }

        private static int Orientation(Point p1, Point p2, Point p)
        {
            // Determinant
            int Orin = (p2.X - p1.X) * (p.Y - p1.Y) - (p.X - p1.X) * (p2.Y - p1.Y);

            if (Orin > 0)
                return -1; //          (* Orientaion is to the left-hand side  *)
            if (Orin < 0)
                return 1; // (* Orientaion is to the right-hand side *)

            return 0; //  (* Orientaion is neutral aka collinear  *)
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
