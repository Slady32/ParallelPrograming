using System;
using System.Collections.Generic;
using System.Drawing;

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
                graph.Points.Add(new Point(randomizor.Next(0, 200), randomizor.Next(0, 200)));
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