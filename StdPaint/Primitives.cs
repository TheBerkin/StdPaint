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
        /// The triangles of the face.
        /// </summary>
        public Triangle3f[] Triangles;

        /// <summary>
        /// The color of the face.
        /// </summary>
        public BufferColor Color;

        /// <summary>
        /// Create a new Face from one or more triangles.
        /// </summary>
        /// <param name="tris">One or more triangles.</param>
        public Face(BufferColor color, params Triangle3f[] tris)
        {
            this.Triangles = tris;
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
                    color,
                    new Triangle3f(new Vector3(-1, 1, 1), new Vector3(-1, -1, 1), new Vector3(1, 1, 1)),
                    new Triangle3f(new Vector3(-1, -1, 1), new Vector3(1, 1, 1), new Vector3(1, -1, 1))
                )
            };
        }
    }
}
