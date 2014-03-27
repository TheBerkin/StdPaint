using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace StdPaint
{
    /// <summary>
    /// Represents a rectangle with a location, width and height.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle // RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        /// <summary>
        /// Initializes a new rectangle with the specified coordinates.
        /// </summary>
        /// <param name="l">The left bound.</param>
        /// <param name="r">The right bound.</param>
        /// <param name="t">The top bound.</param>
        /// <param name="b">The bottom bound.</param>
        public Rectangle(int l, int r, int t, int b)
        {
            if (r < l)
            {
                Utils.Swap(ref l, ref r);
            }

            if (b < t)
            {
                Utils.Swap(ref t, ref b);
            }

            this.Left = l;
            this.Right = r;
            this.Top = t;
            this.Bottom = b;
        }

        /// <summary>
        /// Gets or sets the top-left corner of the rectangle.
        /// </summary>
        public Point Location
        {
            get { return new Point(this.Left, this.Top); }
            set
            {
                this.Right += value.X - this.Left;
                this.Bottom += value.Y - this.Top;
                this.Left = value.X;
                this.Top = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the width of the rectangle.
        /// </summary>
        public int Width
        {
            get { return this.Right - this.Left; }
            set { this.Right = this.Left + value; }
        }

        /// <summary>
        /// Gets or sets the height of the rectangle.
        /// </summary>
        public int Height
        {
            get { return this.Bottom - this.Top; }
            set { this.Bottom = this.Top + value; }
        }

        /// <summary>
        /// Returns whether the specified point is contained within the bounds of the rectangle.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <returns></returns>
        public bool Contains(Point point)
        {
            return point.X >= this.Left && point.X < this.Right && point.Y >= this.Top && point.Y < this.Bottom;
        }

        /// <summary>
        /// Returns a string representation of this Rectangle object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("T={0}; B={1}; L={2}; R={3};", Top, Bottom, Left, Right);
        }
    }
}
