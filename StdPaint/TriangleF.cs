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


    }
}
