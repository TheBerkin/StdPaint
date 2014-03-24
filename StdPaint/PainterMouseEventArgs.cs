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
        /// <summary>
        /// The location of the cursor, in buffer units, when the event was raised.
        /// </summary>
        public readonly Point UnitLocation;

        internal PainterMouseEventArgs(Point point)
        {
            this.UnitLocation = point;
        }
    }
}
