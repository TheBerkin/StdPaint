using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// Represents a set of three two-dimensional floating-point vectors.
    /// </summary>
    public struct Triangle2f
    {
        /// <summary>
        /// The first vector.
        /// </summary>
        public Vector2 A;

        /// <summary>
        /// The second vector.
        /// </summary>
        public Vector2 B;

        /// <summary>
        /// The third vector.
        /// </summary>
        public Vector2 C;

        /// <summary>
        /// Initializes a new instance of the StdPaint.Trangle2f structure with the specified vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <param name="c">The third vector.</param>
        public Triangle2f(Vector2 a, Vector2 b, Vector2 c)
        {
            A = a;
            B = b;
            C = c;
        }

        /// <summary>
        /// Gets the area of the triangle.
        /// </summary>
        public double Area
        {
            get { return GetArea(ref A, ref B, ref C); }
        }

        /// <summary>
        /// Returns the area of the triangle made from the specified vectors.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <param name="c">The third point.</param>
        /// <returns></returns>
        public static double GetArea(Vector2 a, Vector2 b, Vector2 c)
        {
            double ab = (b - a).Length;
            double bc = (b - c).Length;
            double ca = (c - a).Length;
            double s = (ab + bc + ca) / 2;
            return Math.Sqrt(s * (s - ab) * (s - bc) * (s - ca));
        }

        /// <summary>
        /// Returns the area of the triangle made from the specified vectors.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <param name="c">The third point.</param>
        /// <returns></returns>
        public static double GetArea(ref Vector2 a, ref Vector2 b, ref Vector2 c)
        {
            double ab = (b - a).Length;
            double bc = (b - c).Length;
            double ca = (c - a).Length;
            double s = (ab + bc + ca) / 2;
            return Math.Sqrt(s * (s - ab) * (s - bc) * (s - ca));
        }
    }

    /// <summary>
    /// Represents a set of three three-dimensional floating-point vectors.
    /// </summary>
    public struct Triangle3f
    {
        /// <summary>
        /// The first vector.
        /// </summary>
        public Vector4 A;

        /// <summary>
        /// The second vector.
        /// </summary>
        public Vector4 B;

        /// <summary>
        /// The third vector.
        /// </summary>
        public Vector4 C;

        /// <summary>
        /// Initializes a new instance of the StdPaint.Trangle3f structure with the specified vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <param name="c">The third vector.</param>
        public Triangle3f(Vector3 a, Vector3 b, Vector3 c)
        {
            A = Vector4.FromVector3(a);
            B = Vector4.FromVector3(b);
            C = Vector4.FromVector3(c);
        }

        /// <summary>
        /// Initializes a new instance of the StdPaint.Trangle3f structure with the specified vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <param name="c">The third vector.</param>
        public Triangle3f(Vector4 a, Vector4 b, Vector4 c)
        {
            A = a;
            B = b;
            C = c;
        }


        /// <summary>
        /// Gets the area of the triangle.
        /// </summary>
        public double Area
        {
            get { return GetArea(ref A, ref B, ref C); }
        }

        /// <summary>
        /// Returns the area of the triangle made from the specified vectors.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <param name="c">The third point.</param>
        /// <returns></returns>
        public static double GetArea(Vector4 a, Vector4 b, Vector4 c)
        {
            double ab = (b - a).Length;
            double bc = (b - c).Length;
            double ca = (c - a).Length;
            double s = (ab + bc + ca) / 2;
            return Math.Sqrt(s * (s - ab) * (s - bc) * (s - ca));
        }

        /// <summary>
        /// Returns the area of the triangle made from the specified vectors.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <param name="c">The third point.</param>
        /// <returns></returns>
        public static double GetArea(ref Vector4 a, ref Vector4 b, ref Vector4 c)
        {
            double ab = (b - a).Length;
            double bc = (b - c).Length;
            double ca = (c - a).Length;
            double s = (ab + bc + ca) / 2;
            return Math.Sqrt(s * (s - ab) * (s - bc) * (s - ca));
        }
    }
}
