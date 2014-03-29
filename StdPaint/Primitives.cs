using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// Represents one face of a mesh.
    /// </summary>
    public class Face
    {
        /// <summary>
        /// The tris of the face.
        /// </summary>
        public Triangle3[] Triangles;

        /// <summary>
        /// Create a new Face from one or more triangles.
        /// </summary>
        /// <param name="tris">One or more triangles.</param>
        public Face(params Triangle3[] tris)
        {
            this.Triangles = tris;
        }
    }

    /// <summary>
    /// Represents one edge of a face.
    /// </summary>
    public class Triangle3
    {
        public Vector4 Point1;
        public Vector4 Point2;
        public Vector4 Point3;
        public BufferColor Color;

        /// <summary>
        /// Create a new edge from two vectors.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        public Triangle3(Vector3 point1, Vector3 point2, Vector3 point3, BufferColor color)
        {
            this.Point1 = Vector4.FromVector3(point1);
            this.Point2 = Vector4.FromVector3(point2);
            this.Point3 = Vector4.FromVector3(point3);
            this.Color = color;
        }

        /// <summary>
        /// Create a new edge from two vectors.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        public Triangle3(Vector4 point1, Vector4 point2, Vector4 point3, BufferColor color)
        {
            this.Point1 = point1;
            this.Point2 = point2;
            this.Point3 = point3;
            this.Color = color;
        }
    }

    /// <summary>
    /// A base class representing a renderable object.
    /// </summary>
    public class Renderable
    {
        public Face[] Faces;
        public Matrix4 ModelMatrix;

        public Renderable()
        {
            ModelMatrix = Matrix4.Identity;
        }
    }

    /// <summary>
    /// Represents a two-sided flat primitive.
    /// </summary>
    public class Plane : Renderable
    {
        /// <summary>
        /// Create a new Plane object.
        /// </summary>
        public Plane(BufferColor color) : base()
        {
            Faces = new Face[1] {
                new Face(
                    new Triangle3(new Vector3(-1, 1, 1), new Vector3(-1, -1, 1), new Vector3(1, 1, 1), color),
                    new Triangle3(new Vector3(-1, -1, 1), new Vector3(1, 1, 1), new Vector3(1, -1, 1), color)
                )
            };
        }
    }
}
