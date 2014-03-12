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

        private IHull _hull;

        private Graph _graph;

        public GraphsForm()
        {
            InitializeComponent();

            InitializeGraph();

        }

        private void InitializeGraph()
        {
            var generateRandom = true;
            if (!generateRandom)
            {
                var graphFactory = new GraphFactory();
                var pointList = new List<Point>
                {
                    new Point(62, 45),
                    new Point(17, 112),
                    new Point(108, 113),
                    new Point(126, 135),
                    new Point(53, 129),
                    new Point(85, 44),
                    new Point(80, 115),
                    new Point(28, 4),
                    new Point(96, 90),
                    new Point(85, 163),
                    new Point(72, 78),
                    new Point(100, 29),
                    new Point(153, 96),
                    new Point(184, 164),
                    new Point(183, 79)
                };
                    _graph = graphFactory.GenerateGraphWithList(new Point(100, 100), pointList);
                    
            }
            else if (generateRandom)
            {
                _graph = new GraphFactory().GenerateGraphWithRandoms(new Point(100, 100), 25);
            }
            Graphs = new List<IPainter>
                    {
                        _graph
                    };

            var sb = new StringBuilder();
            foreach (var point in _graph.Points)
            {
                sb.Append(string.Format("new Point({0}, {1}),{2}", point.X, point.Y, Environment.NewLine));
            }

            sb.Remove(sb.Length - 3, 3);
            textBox1.Text = sb.ToString();

            _hull = new QuickHull(_graph);
            _hull.Execute();
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

        private void GenGraph_Click(object sender, EventArgs e)
        {
            InitializeGraph();
            Invalidate();
        }
    }
}
