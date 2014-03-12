using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvexHull
{
    public class OneThreadPerSplitQuickHull : QuickHull
    {
        public OneThreadPerSplitQuickHull(IGraph graph) : base(graph) { }

        protected override void ExecuteSplit(IList<System.Drawing.Point> points, IList<Node> lowerHalf, IList<Node> upperHalf, Node leftNode, Node rightNode)
        {
            ExecuteThread(1, () => SplitLowerHalf(points, lowerHalf, upperHalf, leftNode, rightNode));
            ExecuteThread(1, () => SplitUpperHalf(points, lowerHalf, upperHalf, leftNode, rightNode));
        }

        protected override void ExecuteFindMinY(IList<Node> list, Node prevNode, Node nextNode)
        {
            FindMinY(list, prevNode, nextNode);
        }

        protected override void ExecuteFindMaxY(IList<Node> list, Node prevNode, Node nextNode)
        {
            FindMaxY(list, prevNode, nextNode);
        }
    }
}
