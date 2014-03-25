using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace StdPaint
{
    /// <summary>
    /// Represents a two-dimensional point.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        /// <summary>
        /// The X position.
        /// </summary>
        public int X;
        /// <summary>
        /// The Y position.
        /// </summary>
        public int Y;

        /// <summary>
        /// Initializes a new instance of the StdPaint.Point structure with the specified coordinates.
        /// </summary>
        /// <param name="x">The X position.</param>
        /// <param name="y">The Y position.</param>
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }


    }
}
