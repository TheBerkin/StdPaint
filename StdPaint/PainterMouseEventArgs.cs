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
        /// The location of the cursor, in buffer coordinates, when the event was raised.
        /// </summary>
        public readonly Point UnitLocation;

        /// <summary>
        /// The location of the cursor, in screen coordinates, when the event was raised.
        /// </summary>
        public readonly Point ScreenLocation;

        internal PainterMouseEventArgs(Point unitLoc, Point screenLoc)
        {
            this.UnitLocation = unitLoc;
            this.ScreenLocation = screenLoc;
        }
    }
}
