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

            var graphFactory = new GraphFactory();
            var pointList = new List<Point>
            {
                new Point(0,0),
                new Point(50,25),
                new Point(100,0),
                new Point(50,125),
                new Point(100,100),
                new Point(50,150),
                new Point(0,100),
                new Point(50,50)
            };
            //var graph = graphFactory.GenerateGraphWithList(new Point(100,100), pointList);
            var graph = graphFactory.GenerateGraphWithRandoms(new Point(100, 100), 10);

            Graphs = new List<IPainter>
            {
                graph
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
