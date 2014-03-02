using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvexHull
{
    public class Graph : IPainter
    {
        public Guid Id { get; set; }
        public Point Origin { get; private set; }
        public IList<Node> Nodes { get; set; }

        public Graph(Point origin)
        {
            Id = Guid.NewGuid();
            Origin = origin;
            Nodes = new List<Node>();
        }

        public void Paint(PaintEventArgs e)
        {
            foreach (var node in Nodes)
            {
                e.Graphics.DrawRectangle(Pens.Black, Origin.X + node.Position.X, Origin.Y + node.Position.Y, 5f, 5f);

                var startPoint = new Point(node.Position.X + Origin.X,node.Position.Y + Origin.Y);
                var endPoint = new Point(node.Next.Position.X + Origin.X, node.Next.Position.Y + Origin.Y);

                e.Graphics.DrawLine(Pens.Black, startPoint, endPoint);
            }
        }
    }
}
