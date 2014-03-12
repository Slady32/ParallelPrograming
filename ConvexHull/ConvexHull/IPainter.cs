using System.Drawing;
using System.Windows.Forms;

namespace ConvexHull
{
    public interface IPainter
    {
        Point Origin { get; }
        void Paint(PaintEventArgs e);
    }
}
