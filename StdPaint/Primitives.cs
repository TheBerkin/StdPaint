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
        /// The edges of the face.
        /// </summary>
        public Edge[] Edges;

        /// <summary>
        /// Create a new Face from one or more Edges.
        /// </summary>
        /// <param name="edge">One or more Edges.</param>
        public Face(params Edge[] edge)
        {
            this.Edges = edge;
        }
    }

    /// <summary>
    /// Represents one edge of a face.
    /// </summary>
    public class Edge
    {
        public Vector4 Point1;
        public Vector4 Point2;
        public BufferColor Color;

        /// <summary>
        /// Create a new edge from two vectors.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        public Edge(Vector3 point1, Vector3 point2, BufferColor color)
        {
            this.Point1 = Vector4.FromVector3(point1);
            this.Point2 = Vector4.FromVector3(point2);
            this.Color = color;
        }

        /// <summary>
        /// Create a new edge from two vectors.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        public Edge(Vector4 point1, Vector4 point2, BufferColor color)
        {
            this.Point1 = point1;
            this.Point2 = point2;
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
                    new Edge(new Vector3(-10, 10, 1), new Vector3(-10, -10, 1), color),
                    new Edge(new Vector3(-10, -10, 1), new Vector3(10, -10, 1), color),
                    new Edge(new Vector3(10, -10, 1), new Vector3(10, 10, 1), color),
                    new Edge(new Vector3(10, 10, 1), new Vector3(-10, 10, 1), color)
                )
            };
        }
    }

    /// <summary>
    /// Represents a six-sided cube primitive.
    /// </summary>
    public class Cube : Renderable
    {
        /// <summary>
        /// Create a new Plane object.
        /// </summary>
        public Cube(BufferColor color)
            : base()
        {
            Faces = new Face[6] {
                //front
                new Face(
                    new Edge(new Vector3(-10, 10, 1), new Vector3(-10, -10, 1), color),
                    new Edge(new Vector3(-10, -10, 1), new Vector3(10, -10, 1), color),
                    new Edge(new Vector3(10, -10, 1), new Vector3(10, 10, 1), color),
                    new Edge(new Vector3(10, 10, 1), new Vector3(-10, 10, 1), color)
                ),
                //back
                new Face(
                    new Edge(new Vector3(-10, 10, 5), new Vector3(-10, -10, 5), color),
                    new Edge(new Vector3(-10, -10, 5), new Vector3(10, -10, 5), color),
                    new Edge(new Vector3(10, -10, 5), new Vector3(10, 10, 5), color),
                    new Edge(new Vector3(10, 10, 5), new Vector3(-10, 10, 5), color)
                ),
                //left
                new Face(
                    new Edge(new Vector3(-10, 10, 5), new Vector3(-10, -10, 5), color),
                    new Edge(new Vector3(-10, -10, 5), new Vector3(-10, -10, 1), color),
                    new Edge(new Vector3(-10, -10, 1), new Vector3(-10, 10, 1), color),
                    new Edge(new Vector3(-10, 10, 1), new Vector3(-10, 10, 5), color)
                ),
                //right
                new Face(
                    new Edge(new Vector3(10, 10, 5), new Vector3(10, -10, 5), color),
                    new Edge(new Vector3(10, -10, 5), new Vector3(10, -10, 1), color),
                    new Edge(new Vector3(10, -10, 1), new Vector3(10, 10, 1), color),
                    new Edge(new Vector3(10, 10, 1), new Vector3(10, 10, 5), color)
                ),
                //bottom
                new Face(
                    new Edge(new Vector3(-10, 10, 5), new Vector3(-10, 10, 1), color),
                    new Edge(new Vector3(-10, 10, 1), new Vector3(10, 10, 1), color),
                    new Edge(new Vector3(10, 10, 1), new Vector3(10, 10, 5), color),
                    new Edge(new Vector3(10, 10, 5), new Vector3(-10, 10, 5), color)
                ),
                //top
                new Face(
                    new Edge(new Vector3(-10, -10, 5), new Vector3(-10, -10, 1), color),
                    new Edge(new Vector3(-10, -10, 1), new Vector3(10, -10, 1), color),
                    new Edge(new Vector3(10, -10, 1), new Vector3(10, -10, 5), color),
                    new Edge(new Vector3(10, -10, 5), new Vector3(-10, -10, 5), color)
                )
            };
        }
    }
}
