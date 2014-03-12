using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvexHull
{
    class SerialGiftWrapping : IHull
    {
        private Graph _graph;

        public SerialGiftWrapping(Graph graph)
        {
            _graph = graph;
        }

        public void Execute()
        {
            List<Point> tempGraph = _graph.Points.OrderBy(p => p.X).ToList();
            List<Point> tempUpperGraph = new List<Point>();
            _graph.HullNodes.Add(new Node(tempGraph[0]));
            tempGraph.RemoveAt(0);
            foreach (var p in tempGraph)
            {
                if (p.Y > _graph.HullNodes[0].Position.Y)
                {
                    tempUpperGraph.Add(p);
                    tempGraph.Remove(p);
                }
            }
            
            for (int i = 1; i < tempGraph.Count; i++)
            {
                
            }

        }


    }
}
