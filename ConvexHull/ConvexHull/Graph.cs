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
        }

        public void Paint(PaintEventArgs e)
        {
            foreach (var node in Nodes)
            {
                e.Graphics.DrawRectangle(Pens.Black, Origin.X + node.Position.X, Origin.Y + node.Position.Y, 5f, 5f);

                var startPoint = node.Position;
                var endPoint = node.Next.Position;

                e.Graphics.DrawLine(Pens.Black, startPoint, endPoint);
            }
        }
    }
}
