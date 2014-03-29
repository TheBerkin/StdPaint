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
        /// Adds two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns></returns>
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns></returns>
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns></returns>
        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X * b.X, a.Y * b.Y);
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="a">The vector to multiply.</param>
        /// <param name="b">The scalar to multiply by.</param>
        /// <returns></returns>
        public static Vector2 operator *(Vector2 a, double b)
        {
            return new Vector2(a.X * b, a.Y * b);
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns></returns>
        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X / b.X, a.Y / b.Y);
        }

        /// <summary>
        /// Divides a vector by a scalar.
        /// </summary>
        /// <param name="a">The vector to divide.</param>
        /// <param name="b">The scalar to divide by.</param>
        /// <returns></returns>
        public static Vector2 operator /(Vector2 a, double b)
        {
            return new Vector2(a.X / b, a.Y / b);
        }

        /// <summary>
        /// Gets the squared length of this vector.
        /// </summary>
        public double LengthSquared
        {
            get { return X * X + Y * Y; }
        }

        /// <summary>
        /// Gets the length of this vector.
        /// </summary>
        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        /// <summary>
        /// Returns the normalized version of a vector.
        /// </summary>
        /// <param name="vector">The vector to normalize.</param>
        /// <returns></returns>
        public static Vector2 Normalize(Vector2 vector)
        {
            return vector / vector.Length;
        }

        /// <summary>
        /// Returns the normalized version of a vector.
        /// </summary>
        /// <param name="vector">The vector to normalize.</param>
        /// <returns></returns>
        public static Vector2 Normalize(ref Vector2 vector)
        {
            return vector / vector.Length;
        }

        /// <summary>
        /// Returns the dot product of two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns></returns>
        public static double Dot(Vector2 a, Vector2 b)
        {
            a = Vector2.Normalize(ref a);
            b = Vector2.Normalize(ref b);
            return a.X * b.X + a.Y * b.Y;
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns></returns>
        public static double Distance(Vector2 a, Vector2 b)
        {
            return (b - a).Length;
        }

        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns></returns>
        public static double DistanceSquared(Vector2 a, Vector2 b)
        {
            return (b - a).LengthSquared;
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

        /// <summary>
        /// Multiplies a vector by a matrix..
        /// </summary>
        /// <param name="a">The vector to multiply.</param>
        /// <param name="b">The matrix to multiply by.</param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 a, Matrix4 b)
        {
            Vector3 final = new Vector3(
                a.X * b[0, 0] + a.Y * b[1, 0] + a.Z * b[2, 0],
                a.X * b[0, 1] + a.Y * b[1, 1] + a.Z * b[2, 1],
                a.X * b[0, 2] + a.Y * b[1, 2] + a.Z * b[2, 2]
            );
            return final;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns></returns>
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns></returns>
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="a">The vector to multiply.</param>
        /// <param name="b">The scalar to multiply by.</param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 a, double b)
        {
            return new Vector3(a.X * b, a.Y * b, a.Z * b);
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns></returns>
        public static Vector3 operator /(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }

        /// <summary>
        /// Divides a vector by a scalar.
        /// </summary>
        /// <param name="a">The vector to divide.</param>
        /// <param name="b">The scalar to divide by.</param>
        /// <returns></returns>
        public static Vector3 operator /(Vector3 a, double b)
        {
            return new Vector3(a.X / b, a.Y / b, a.Z / b);
        }

        /// <summary>
        /// Gets the squared length of the vector.
        /// </summary>
        public double LengthSquared
        {
            get { return X * X + Y * Y + Z * Z; }
        }

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y + Z * Z); }
        }

        /// <summary>
        /// Returns the normalized version of a vector.
        /// </summary>
        /// <param name="vector">The vector to normalize.</param>
        /// <returns></returns>
        public static Vector3 Normalize(Vector3 vector)
        {
            return vector / vector.Length;
        }

        /// <summary>
        /// Returns the normalized version of a vector.
        /// </summary>
        /// <param name="vector">The vector to normalize.</param>
        /// <returns></returns>
        public static Vector3 Normalize(ref Vector3 vector)
        {
            return vector / vector.Length;
        }

        /// <summary>
        /// Returns the dot product of two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns></returns>
        public static double Dot(Vector3 a, Vector3 b)
        {
            a = Vector3.Normalize(a);
            b = Vector3.Normalize(b);
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns></returns>
        public static double Distance(Vector3 a, Vector3 b)
        {
            return (b - a).Length;
        }

        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns></returns>
        public static double DistanceSquared(Vector3 a, Vector3 b)
        {
            return (b - a).LengthSquared;
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

        /// <summary>
        /// Gets the magnitude of the vector.
        /// </summary>
        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y + Z * Z + W * W); }
        }

        /// <summary>
        /// Returns the Vector4 equivalent of the specified Vector3.
        /// </summary>
        /// <param name="a">The vector to convert.</param>
        /// <returns></returns>
        public static Vector4 FromVector3(Vector3 a)
        {
            return new Vector4(a.X, a.Y, a.Z, 1);
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns></returns>
        public static Vector4 operator -(Vector4 a, Vector4 b)
        {
            return new Vector4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
        }

        /// <summary>
        /// Multiplies a Vector4 by a scalar.
        /// </summary>
        /// <param name="a">The scalar to multiply by.</param>
        /// <param name="b">The Vector4 to multiply.</param>
        /// <returns></returns>
        public static Vector4 operator *(double a, Vector4 b)
        {
            return new Vector4(b.X * a, b.Y * a, b.Z * a, b.W * a);
        }

        /// <summary>
        /// Multiplies a Vector4 by a scalar.
        /// </summary>
        /// <param name="a">The Vector4 to multiply.</param>
        /// <param name="b">The scalar to multiply by.</param>
        /// <returns></returns>
        public static Vector4 operator *(Vector4 a, double b) { return b * a; }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns></returns>
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