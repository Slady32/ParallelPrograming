using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvexHull
{
    public class Graph
    {
        public Guid Id { get; set; }
        public Point Origin { get; private set; }
        public IList<Node> Nodes { get; set; }

        public Graph(Point origin)
        {
            Id = Guid.NewGuid();
            Origin = origin;
        }
    }
}
