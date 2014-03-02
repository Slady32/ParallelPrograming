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
        public Graph GenerateGraphWithRandoms(Point origin, int count)
        {
            var graph = new Graph(origin);
            var randomizor = new Random();
            for (var i = 0; i < count; i++)
            {
                if (i == 0)
                {
                    graph.Points.Add(new Node(new Point(graph.Origin.X - randomizor.Next(1, 4) * 25, graph.Origin.X - randomizor.Next(1, 4) * 25)));
                }
                else
                {
                    graph.Points.Add(new Node(new Point(graph.Origin.X + randomizor.Next(-4, 4) * 25, graph.Origin.X + randomizor.Next(-4, 4) * 25)));
                }
            }

            return SetNeighbours(graph);
        }

        public Graph GenerateGraphWithList(Point origin, List<Point> points)
        {
            var graph = new Graph(origin);
            foreach (var p in points)
            {
                graph.Points.Add(new Node(p));
            }

            return SetNeighbours(graph);
        }

        private Graph SetNeighbours(Graph graph)
        {
            if (graph.Points.Count != 0)
            {
                if (graph.Points.Count > 1)
                {
                    for (var i = 0; i < graph.Points.Count; i++)
                    {
                        if (i == 0)
                        {
                            graph.Points[i].Next = graph.Points[i + 1];
                            graph.Points[i].Prev = graph.Points[graph.Points.Count - 1];
                        }
                        else if (i == graph.Points.Count - 1)
                        {
                            graph.Points[i].Next = graph.Points[0];
                            graph.Points[i].Prev = graph.Points[i - 1];
                        }
                        else
                        {
                            graph.Points[i].Next = graph.Points[i + 1];
                            graph.Points[i].Prev = graph.Points[i - 1];
                        }
                    }
                }
                else
                {
                    graph.Points[0].Next = graph.Points[0];
                    graph.Points[0].Prev = graph.Points[0];
                }
            }
            return graph;
        }
    }
}