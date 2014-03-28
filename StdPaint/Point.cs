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

        /// <summary>
        /// Returns a Vector2 object that contains the Point's current coordinates.
        /// </summary>
        /// <returns></returns>
        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        /// <summary>
        /// The aquared distance from the origin.
        /// </summary>
        public int LengthSquared
        {
            get { return X * X + Y * Y; }
        }

        /// <summary>
        /// The distance from the origin.
        /// </summary>
        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        /// <summary>
        /// Returns the midpoint between two points.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <returns></returns>
        public static Point Mid(Point a, Point b)
        {
            return (a + b) / 2;
        }

        /// <summary>
        /// Returns the dot product between two points.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <returns></returns>
        public static int VectorDot(Point a, Point b)
        {
            return (a.X * b.X) + (a.Y * b.Y);
        }

        /// <summary>
        /// Adds two points.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <returns></returns>
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Subtracts two points.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <returns></returns>
        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Multiplies two points.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <returns></returns>
        public static Point operator *(Point a, Point b)
        {
            return new Point(a.X * b.X, a.Y * b.Y);
        }

        /// <summary>
        /// Multiplies the components of a point by a scalar.
        /// </summary>
        /// <param name="a">The point to multiply.</param>
        /// <param name="b">The scalar to multiply by.</param>
        /// <returns></returns>
        public static Point operator *(Point a, int b)
        {
            return new Point(a.X * b, a.Y * b);
        }

        /// <summary>
        /// Divides the components of a point by a scalar.
        /// </summary>
        /// <param name="a">The point to divide.</param>
        /// <param name="b">The scalar to divide by.</param>
        /// <returns></returns>
        public static Point operator /(Point a, int b)
        {
            return new Point(a.X / b, a.Y / b);
        }

        /// <summary>
        /// Returns a string representing this Point.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[X={0}; Y={1};]", this.X, this.Y);
        }
    }
}
