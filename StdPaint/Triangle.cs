using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// Represents a set of three points.
    /// </summary>
    public struct Triangle
    {
        /// <summary>
        /// The first point in the triangle.
        /// </summary>
        public Point A;
        /// <summary>
        /// The second point in the triangle.
        /// </summary>
        public Point B;
        /// <summary>
        /// The third point in the triangle.
        /// </summary>
        public Point C;

        /// <summary>
        /// Initializes a new instance of the Triangle struct with the supplied coordinates.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <param name="c">The third point.</param>
        public Triangle(Point a, Point b, Point c)
        {
            this.A = a;
            this.B = b;
            this.C = c;
        }

        /// <summary>
        /// Gets the area of the triangle.
        /// </summary>
        public double Area
        {
            get { return GetArea(A, B, C); }
        }

        /// <summary>
        /// Returns the area of the triangle made from the specified points.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <param name="c">The third point.</param>
        /// <returns></returns>
        public static double GetArea(Point a, Point b, Point c)
        {
            double ab = (b - a).Length;
            double bc = (b - c).Length;
            double ca = (c - a).Length;
            double s = (ab + bc + ca) / 2;
            return Math.Sqrt(s * (s - ab) * (s - bc) * (s - ca));
        }

        /// <summary>
        /// Returns a Rectangle that represents the bounding box of this Triangle.
        /// </summary>
        /// <returns></returns>
        public Rectangle GetBounds()
        {
            int minx = Math.Min(A.X, Math.Min(B.X, C.X));
            int maxx = Math.Max(A.X, Math.Max(B.X, C.X));
            int miny = Math.Min(A.Y, Math.Min(B.Y, C.Y));
            int maxy = Math.Max(A.Y, Math.Max(B.Y, C.Y));

            return new Rectangle(minx, maxx, miny, maxy);
        }
    }
}
