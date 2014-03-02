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
                    graph.Points.Add(new Point(graph.Origin.X - randomizor.Next(1, 4) * 25, graph.Origin.X - randomizor.Next(1, 4) * 25));
                }
                else
                {
                    graph.Points.Add(new Point(graph.Origin.X + randomizor.Next(-4, 4) * 25, graph.Origin.X + randomizor.Next(-4, 4) * 25));
                }
            }

            return graph;
        }

        public Graph GenerateGraphWithList(Point origin, List<Point> points)
        {
            var graph = new Graph(origin);
            foreach (var p in points)
            {
                graph.Points.Add(p);
            }
            return graph;
        }
    }
}