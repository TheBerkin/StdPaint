using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// Contains mouse event data for the StdPaint.Painter.
    /// </summary>
    public class PainterMouseEventArgs : EventArgs
    {
        Point _point;

        /// <summary>
        /// The location of the cursor, in buffer units.
        /// </summary>
        public Point UnitLocation
        {
            get { return _point; }
        }

        internal PainterMouseEventArgs(Point point)
        {
            _point = point;
        }
    }
}
