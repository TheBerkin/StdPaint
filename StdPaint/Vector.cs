using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// A vector representing a point in 2D space.
    /// </summary>
    public class Vector2
    {
        /// <summary>
        /// The X coordinate.
        /// </summary>
        public double X;
        /// <summary>
        /// The Y coordinate.
        /// </summary>
        public double Y;

        /// <summary>
        /// Create a new Vector2 from coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public Vector2(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Create a new Vector2 from coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public Vector2(int x, int y)
        {
            this.X = (double)x;
            this.Y = (double)y;
        }

        /// <summary>
        /// Create a new Point from the Vector2.
        /// </summary>
        /// <returns>A new Point object created from the Vector2.</returns>
        public Point ToPoint()
        {
            return new Point((int)this.X, (int)this.Y);
        }

        /// <summary>
        /// Create a new Vector2 from a Point object.
        /// </summary>
        /// <param name="point">The Point object to create the Vector2 from.</param>
        /// <returns>A new Vector2 created from the Point object.</returns>
        public static Vector2 FromPoint(Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }

    /// <summary>
    /// A vector representing a point in 3D space.
    /// </summary>
    public class Vector3
    {
        /// <summary>
        /// The X coodinate.
        /// </summary>
        public double X;
        /// <summary>
        /// The Y coordinate.
        /// </summary>
        public double Y;
        /// <summary>
        /// The Z coordinate.
        /// </summary>
        public double Z;

        /// <summary>
        /// Up direction vector.
        /// </summary>
        public static Vector3 Up
        {
            get { return new Vector3(0, 1, 0); }
        }

        /// <summary>
        /// Down direction vector.
        /// </summary>
        public static Vector3 Down
        {
            get { return new Vector3(0, -1, 0); }
        }

        /// <summary>
        /// Left direction vector.
        /// </summary>
        public static Vector3 Left
        {
            get { return new Vector3(-1, 0, 0); }
        }

        /// <summary>
        /// Right direction vector.
        /// </summary>
        public static Vector3 Right
        {
            get { return new Vector3(1, 0, 0); }
        }

        /// <summary>
        /// Back direction vector.
        /// </summary>
        public static Vector3 Back
        {
            get { return new Vector3(0, 0, -1); }
        }

        /// <summary>
        /// Forward direction vector.
        /// </summary>
        public static Vector3 Forward
        {
            get { return new Vector3(0, 0, 1); }
        }

        /// <summary>
        /// Create a new Vector3 from coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        public Vector3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vector3 operator *(Vector3 a, Matrix4 b)
        {
            Vector3 final = new Vector3(
                a.X * b[0, 0] + a.Y * b[1, 0] + a.Z * b[2, 0],
                a.X * b[0, 1] + a.Y * b[1, 1] + a.Z * b[2, 1],
                a.X * b[0, 2] + a.Y * b[1, 2] + a.Z * b[2, 2]
            );
            return final;
        }
    }

    /// <summary>
    /// A vector representing homogeneous coordinates in 3D space.
    /// </summary>
    public class Vector4
    {
        /// <summary>
        /// The X coodinate.
        /// </summary>
        public double X;
        /// <summary>
        /// The Y coordinate.
        /// </summary>
        public double Y;
        /// <summary>
        /// The Z coordinate.
        /// </summary>
        public double Z;
        /// <summary>
        /// The W coordinate.
        /// </summary>
        public double W;

        /// <summary>
        /// Create a new Vector3 from coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <param name="w">The W coordinate.</param>
        public Vector4(double x, double y, double z, double w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public double Magnitude
        {
            get { return Math.Sqrt(X * X + Y * Y + Z * Z + W * W); }
        }

        public static Vector4 FromVector3(Vector3 a)
        {
            return new Vector4(a.X, a.Y, a.Z, 1);
        }

        public static Vector4 operator *(double a, Vector4 b)
        {
            return new Vector4(b.X * a, b.Y * a, b.Z * a, b.W * a);
        }

        public static Vector4 operator *(Vector4 a, double b) { return b * a; }

        public static Vector4 operator *(Vector4 a, Matrix4 b)
        {
            Vector4 final = new Vector4(
                a.X * b[0, 0] + a.Y * b[1, 0] + a.Z * b[2, 0] + a.W * b[3, 0],
                a.X * b[0, 1] + a.Y * b[1, 1] + a.Z * b[2, 1] + a.W * b[3, 1],
                a.X * b[0, 2] + a.Y * b[1, 2] + a.Z * b[2, 2] + a.W * b[3, 2],
                a.X * b[0, 3] + a.Y * b[1, 3] + a.Z * b[2, 3] + a.W * b[3, 3]
            );
            return final;
        }
    }
}
