using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvexHull
{
    public class SerialQuickHull : QuickHull
    {
        public SerialQuickHull(IGraph graph) : base(graph) { }

        protected override void ExecuteSplit(IList<System.Drawing.Point> points, IList<Node> lowerHalf, IList<Node> upperHalf, Node leftNode, Node rightNode)
        {
            SplitLowerHalf(points, lowerHalf, upperHalf, leftNode, rightNode);
            SplitUpperHalf(points, lowerHalf, upperHalf, leftNode, rightNode);
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
