using System.Collections.Generic;

namespace ConvexHull
{
    public class OneThreadPerSplitQuickHull : QuickHull
    {
        public OneThreadPerSplitQuickHull(IGraph graph) : base(graph) { }

        protected override void ExecuteSplit(IList<System.Drawing.Point> points, IList<Node> lowerHalf, IList<Node> upperHalf, Node leftNode, Node rightNode)
        {
            SplitLowerHalf(points, lowerHalf, upperHalf, leftNode, rightNode);
            SplitUpperHalf(points, lowerHalf, upperHalf, leftNode, rightNode);
        }

        protected override void ExecuteFindMinY(IList<Node> list, Node prevNode, Node nextNode)
        {
            ExecuteThread(1, () => FindMinY(list, prevNode, nextNode));
        }

        protected override void ExecuteFindMaxY(IList<Node> list, Node prevNode, Node nextNode)
        {
            ExecuteThread(1, () => FindMaxY(list, prevNode, nextNode));
        }
    }
}
