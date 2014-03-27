using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint.GUI
{
    /// <summary>
    /// The base class for all StdPaint GUI controls.
    /// </summary>
    public abstract class Element : IDisposable
    {
        protected Rectangle _rect;

        /// <summary>
        /// The rectangle that this Element is bound to.
        /// </summary>
        public Rectangle Bounds
        {
            get { return _rect; }
            set { _rect = value; }
        }

        /// <summary>
        /// Gets or sets the position of the element.
        /// </summary>
        public Point Position
        {
            get { return _rect.Location; }
            set { _rect.Location = value; }
        }

        internal Element()
        {
            Painter.Paint += Painter_Paint;
            _rect = new Rectangle();
        }

        private void Painter_Paint(object sender, EventArgs e)
        {
            Draw();
        }

        /// <summary>
        /// Called when the Element is ready to be drawn.
        /// </summary>
        protected virtual void Draw()
        {

        }

        /// <summary>
        /// Releases all resources used by this Element.
        /// </summary>
        public void Dispose()
        {
            Painter.Paint -= Painter_Paint;
        }
    }
}
