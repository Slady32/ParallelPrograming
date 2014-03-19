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

        private IList<IHull> Hulls { get; set; }

        private Graph _graph;

        public GraphsForm()
        {
            Graphs = new List<IPainter>();
            Hulls = new List<IHull>();
            InitializeComponent();

            CreateGraph(new Point(50, 100), 25);

            InitializeGraph(GeneratingMethodEnum.SerialQuickHull);
            _graph.Origin = new Point(400, 100);
            InitializeGraph(GeneratingMethodEnum.OneThreadPerSplitQuickHull);
            _graph.Origin = new Point(50, 450);
            InitializeGraph(GeneratingMethodEnum.OneThreadSplitQuickHull);
            _graph.Origin = new Point(400, 450);
            InitializeGraph(GeneratingMethodEnum.SerialGiftWrapping);

        }

        private void InitializeGraph( GeneratingMethodEnum generatingMethod)
        {
            Graphs.Add(new Graph { Origin = _graph.Origin, Points = _graph.Points});

            IHull hull = null;
            switch (generatingMethod)
            {
                case GeneratingMethodEnum.SerialQuickHull:
                    hull = new SerialQuickHull((Graph)Graphs.Last());
                    break;
                case GeneratingMethodEnum.OneThreadPerSplitQuickHull:
                    hull = new OneThreadPerSplitQuickHull((Graph)Graphs.Last());
                    break;
                case GeneratingMethodEnum.OneThreadSplitQuickHull:
                    hull = new OneThreadSplitQuickHull((Graph)Graphs.Last());
                    break;
                case GeneratingMethodEnum.SerialGiftWrapping:
                    hull = new SerialGiftWrapping((Graph)Graphs.Last());
                    break;
                default:
                    break;
            }

            _graph.Name = generatingMethod.ToString();
            hull.Done += Done;
            hull.Execute();
            Hulls.Add(hull);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawGraph(e);
        }

        private void DrawGraph(PaintEventArgs e)
        {
            Graphs.ForEach(g => g.Paint(e));
        }

        private void GenGraph_Click(object sender, EventArgs e)
        {
            var pointCount = int.Parse(textBox2.Text);

            Graphs.Clear();
            textBox1.Text = string.Empty;

            CreateGraph(new Point(50, 100), pointCount);

            InitializeGraph(GeneratingMethodEnum.SerialQuickHull);
            _graph.Origin = new Point(400, 100);
            InitializeGraph(GeneratingMethodEnum.OneThreadPerSplitQuickHull);
            _graph.Origin = new Point(50, 450);
            InitializeGraph(GeneratingMethodEnum.OneThreadSplitQuickHull);
            _graph.Origin = new Point(400, 450);
            InitializeGraph(GeneratingMethodEnum.SerialGiftWrapping);

            Invalidate();
        }

        private void Done(object sender, TimeSpan elapsed)
        {
            textBox1.Text += sender.GetType().Name + Environment.NewLine + elapsed + Environment.NewLine;
        }

        private void CreateGraph(Point origin, int pointCount)
        {
            _graph = new GraphFactory().GenerateGraphWithRandoms(origin, pointCount);
        }
    }
}
