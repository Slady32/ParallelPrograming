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
        public IList<Point> Points { get; set; }
        public IList<Node> HullNodes { get; set; }

        public Graph(Point origin)
        {
            Id = Guid.NewGuid();
            Origin = origin;
            Points = new List<Point>();
            HullNodes = new List<Node>();
        }

        public void Paint(PaintEventArgs e)
        {
            Points.ForEach(p => PaintPoints(p, e, Pens.Black));
            HullNodes.ForEach(n => PaintNode(n, e, Pens.Blue));
            PaintBaseLine(HullNodes[0], HullNodes[1], e, Pens.Red);
            PaintCoordinateSystem(e, Pens.Green);
        }

        private void PaintCoordinateSystem(PaintEventArgs e, Pen pen)
        {
            var range = 300;
            // Lines
            e.Graphics.DrawLine(pen, Origin, new Point(range + Origin.X, Origin.Y));
            e.Graphics.DrawLine(pen, Origin, new Point(Origin.X, range + Origin.Y));

            // Qualifiers
            for (int i = 0; i <= range; i += 50)
            {
                var startPoint = new Point(Origin.X + i, Origin.Y);
                var endPoint = new Point(Origin.X + i, Origin.Y - 10);
                e.Graphics.DrawLine(pen, startPoint, endPoint);
            }

            for (int i = 0; i <= range; i += 50)
            {
                var startPoint = new Point(Origin.X, Origin.Y + i);
                var endPoint = new Point(Origin.X - 10, Origin.Y + i);
                e.Graphics.DrawLine(pen, startPoint, endPoint);
            }


        }

        private void PaintBaseLine(Node node1, Node node2, PaintEventArgs e, Pen pen)
        {
            var startPoint = new Point(node1.Position.X + Origin.X, node1.Position.Y + Origin.Y);
            var endPoint = new Point(node2.Position.X + Origin.X, node2.Position.Y + Origin.Y);

            e.Graphics.DrawLine(pen, startPoint, endPoint);
        }

        private void PaintPoints(Point point, PaintEventArgs e, Pen pen)
        {
            e.Graphics.DrawRectangle(pen, Origin.X + point.X, Origin.Y + point.Y, 5f, 5f);
        }

        private void PaintNode(Node node, PaintEventArgs e, Pen pen)
        {
            var startPoint = new Point(node.Position.X + Origin.X, node.Position.Y + Origin.Y);
            var endPoint = new Point(node.Next.Position.X + Origin.X, node.Next.Position.Y + Origin.Y);

            e.Graphics.DrawLine(pen, startPoint, endPoint);
        }
    }
}
