using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// Renders a three-dimensional scene to a buffer
    /// </summary>
    public class Renderer
    {
        /// <summary>
        /// The Scene currently used for rendering.
        /// </summary>
        public Scene ActiveScene;

        /// <summary>
        /// Create a new renderer.
        /// </summary>
        public Renderer()
        {

        }

        /// <summary>
        /// Render the current scene to the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer to render to.</param>
        public void Render(ConsoleBuffer buffer)
        {
            ActiveScene.Render(buffer);
        }
    }
}
