using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvexHull
{
    public partial class GraphsForm : Form
    {
        private IList<IPainter> Graphs { get; set; }

        public GraphsForm()
        {
            InitializeComponent();

            Graphs = new List<IPainter>
            {
                new Graph(new Point(10, 20))
                {
                    Nodes = new List<Node>
                    {
                        new Node(new Point(0, 0)),
                        new Node(new Point(0, 10)),
                        new Node(new Point(10, 10)),
                        new Node(new Point(10, 0))
                    }
                }
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawGraph(e);
        }

        private void DrawGraph(PaintEventArgs e)
        {
            foreach (var graph in Graphs)
            {
                graph.Paint(e);
            }
        }
    }
}
