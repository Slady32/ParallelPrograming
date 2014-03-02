using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvexHull
{
    public class GraphFactory
    {
        public Graph GenerateGraphWithRandoms(Point origin)
        {
            var graph = new Graph(origin);
            return graph;
        }

        public Graph GenerateGraphWithList(Point origin, List<Point> points)
        {
            var graph = new Graph(origin);
            foreach (var p in points)
            {
                graph.Nodes.Add(new Node(p));
            }
            for (var i = 0; i < graph.Nodes.Count; i++)
            {
                if (i == 0)
                {
                    graph.Nodes[i].Next = graph.Nodes[i + 1];
                    graph.Nodes[i].Prev = graph.Nodes[graph.Nodes.Count - 1];
                }
                else if (i == graph.Nodes.Count - 1)
                {
                    graph.Nodes[i].Next = graph.Nodes[0];
                    graph.Nodes[i].Prev = graph.Nodes[i - 1];
                }
                else
                {
                    graph.Nodes[i].Next = graph.Nodes[i + 1];
                    graph.Nodes[i].Prev = graph.Nodes[i - 1];
                }

            }

            return graph;
        }

    }
}
