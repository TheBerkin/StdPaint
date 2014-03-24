using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// The base class for buffer brushes.
    /// </summary>
    public abstract class BufferBrush
    {
        /// <summary>
        /// Gets the color at the specified position.
        /// </summary>
        /// <param name="x">The X position to sample from.</param>
        /// <param name="y">The Y position to sample from.</param>
        /// <returns></returns>
        public abstract BufferColor GetColor(int x, int y);
    }
}
