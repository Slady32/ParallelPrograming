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
            Graphs = new List<IPainter>();
            InitializeComponent();

            InitializeGraph(new Point(50,50), GeneratingMethodEnum.QuickHull , false);
            InitializeGraph(new Point(400, 50), GeneratingMethodEnum.SerialQuickHull, false);
            InitializeGraph(new Point(50, 400), GeneratingMethodEnum.QuickHull, false);
            InitializeGraph(new Point(400, 400), GeneratingMethodEnum.SerialQuickHull, false);

        }

        private void InitializeGraph(Point origin, GeneratingMethodEnum generatingMethod, bool generateRandom = true)
        {
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
                    _graph = graphFactory.GenerateGraphWithList(origin, pointList);
                    
            }
            else
            {
                _graph = new GraphFactory().GenerateGraphWithRandoms(origin, 25);
            }
            Graphs.Add(_graph);

            var sb = new StringBuilder();
            foreach (var point in _graph.Points)
            {
                sb.Append(string.Format("new Point({0}, {1}),{2}", point.X, point.Y, Environment.NewLine));
            }

            sb.Remove(sb.Length - 3, 3);
            textBox1.Text = sb.ToString();
            switch (generatingMethod)
            {
                case GeneratingMethodEnum.QuickHull:
                    _hull = new SerialQuickHull(_graph);
                    _hull.Execute();
                    break;
                case GeneratingMethodEnum.SerialQuickHull:
                    _hull = new SerialQuickHull(_graph); // Change to SereialQuickHull
                    _hull.Execute();
                    break;
                default:
                    break;
            }
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
            Graphs.Clear();

            InitializeGraph(new Point(50, 50), GeneratingMethodEnum.QuickHull);
            InitializeGraph(new Point(400, 50), GeneratingMethodEnum.SerialQuickHull);
            InitializeGraph(new Point(50, 400), GeneratingMethodEnum.QuickHull);
            InitializeGraph(new Point(400, 400), GeneratingMethodEnum.SerialQuickHull);
            Invalidate();
        }
    }
}
