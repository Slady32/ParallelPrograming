using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvexHull
{
    public class GraphFactory
    {
        public Graph GenerateGraphWithRandoms(int originX, int originY)
        {
            var graph = new Graph(new Point(originX, originY));
            return graph;
        }

        public Graph GenerateGraphWithList(int originX, int originY, List<Point> points)
        {
            var graph = new Graph(new Point(originX, originY));
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
                if (i == graph.Nodes.Count - 1)
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
