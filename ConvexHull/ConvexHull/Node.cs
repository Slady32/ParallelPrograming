using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvexHull
{
    public class Node
    {
        public Guid Id { get; private set; }
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