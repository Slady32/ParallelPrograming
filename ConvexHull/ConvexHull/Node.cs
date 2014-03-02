using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvexHull
{
    public class Node
    {
        public readonly Guid Id { get; private set; }
        public Point Position { get; private set; }
        public Node Next { get; set; }
        public Node Prev { get; set; }

        public Node(Point position)
        {
            Position = position;
            Id = Guid.NewGuid();
        }
    }

}
