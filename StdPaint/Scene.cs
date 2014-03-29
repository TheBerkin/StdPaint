using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// A Scene containing objects to be rendered.
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// The Camera currently used to render the scene.
        /// </summary>
        public Camera ActiveCamera;

        /// <summary>
        /// The objects in the scene.
        /// </summary>
        public List<Renderable> Objects;

        /// <summary>
        /// Create a new Scene object.
        /// </summary>
        public Scene()
        {
            Objects = new List<Renderable>();
        }

        /// <summary>
        /// Render the current scene to the active buffer.
        /// </summary>
        /// <param name="buffer"></param>
        public void Render(ConsoleBuffer buffer)
        {
            foreach (Renderable obj in Objects)
                foreach (Face face in obj.Faces)
                    foreach (Triangle3f tri in face.Triangles)
                    {
                        Vector2 point1 = ActiveCamera.ProjectVector(buffer.Width, buffer.Height, tri.A, obj.ModelMatrix);
                        Vector2 point2 = ActiveCamera.ProjectVector(buffer.Width, buffer.Height, tri.B, obj.ModelMatrix);
                        Vector2 point3 = ActiveCamera.ProjectVector(buffer.Width, buffer.Height, tri.C, obj.ModelMatrix);
                        buffer.DrawTriangle(point1.ToPoint(), point2.ToPoint(), point3.ToPoint(), face.Color);
                    }
        }
    }
}
