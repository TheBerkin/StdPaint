using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// Specified how a buffer should be drawn.
    /// </summary>
    public enum BufferDrawMode
    {
        /// <summary>
        /// Override the original buffer contents.
        /// </summary>
        DrawOver,
        /// <summary>
        /// Combine the new colors with the old colors using a bitwise OR operation.
        /// </summary>
        Additive,
        /// <summary>
        /// Draw all colors except for black.
        /// </summary>
        IgnoreBlack
    }
}
