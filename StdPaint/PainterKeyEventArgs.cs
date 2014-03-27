using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StdPaint
{
    /// <summary>
    /// Contains keyboard event data for the StdPaint.Painter.
    /// </summary>
    public class PainterKeyEventArgs : EventArgs
    {
        /// <summary>
        /// The key code for the key that raised the event.
        /// </summary>
        public readonly Keys KeyCode;

        internal PainterKeyEventArgs(Keys key)
        {
            this.KeyCode = key;
        }
    }
}
