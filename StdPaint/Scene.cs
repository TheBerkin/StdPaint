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
                    foreach (Edge edge in face.Edges)
                    {
                        Vector2 point1 = ActiveCamera.ProjectVector(buffer.Width, buffer.Height, edge.Point1, obj.ModelMatrix);
                        Vector2 point2 = ActiveCamera.ProjectVector(buffer.Width, buffer.Height, edge.Point2, obj.ModelMatrix);
                        buffer.DrawLine((int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y, edge.Color);
                    }
        }
    }
}
