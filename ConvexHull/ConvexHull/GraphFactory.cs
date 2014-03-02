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
                graph.Nodes.Add(new Node(new Point(randomizor.Next(0,8) * 25, randomizor.Next(0,8) * 25)));
            }

            return SetRandomNeighbours(graph);
        }

        public Graph GenerateGraphWithList(Point origin, List<Point> points)
        {
            var graph = new Graph(origin);
            foreach (var p in points)
            {
                graph.Nodes.Add(new Node(p));
            }

            return SetNeighbours(graph);
        }

        private Graph SetNeighbours(Graph graph)
        {
            if (graph.Nodes.Count != 0)
            {
                if (graph.Nodes.Count > 1)
                {
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
                }
                else
                {
                    graph.Nodes[0].Next = graph.Nodes[0];
                    graph.Nodes[0].Prev = graph.Nodes[0];
                }
            }
            return graph;
        }
        private Graph SetRandomNeighbours(Graph graph)
        {
            var sortgraph = GraphSort(graph);


            return SetNeighbours(graph);
        }

        private Graph GraphSort(Graph graph)
        {
            var retVal = new Graph(graph.Origin);
            var firstNode = graph.Nodes[0];
            for (var i = 0; i < graph.Nodes.Count; i++)
            {
                
            }

            return retVal;
        }
        private Node FindClosest(Node node, Graph graph)
        {
            var retVal = new Node(new Point());
            int minDistance = 150;
            foreach (var n in graph.Nodes)
            {
                if (n.Id != node.Id)
                {
                   var distance = CalculateDifference(n.Position, node.Position);
                   if (distance < minDistance)
                   {
                       minDistance = distance;
                       retVal = n;
                   }
                }
            }


            return retVal;
        }

        private int CalculateDifference(Point position1, Point position2)
        {
            int xDiff = Math.Abs(position1.X - position2.X);
            int yDiff = Math.Abs(position1.Y - position2.Y);
            return xDiff + yDiff;
        }
     }
}