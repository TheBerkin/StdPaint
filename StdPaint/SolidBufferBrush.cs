using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// A buffer brush that produces a solid color.
    /// </summary>
    public class SolidBufferBrush : BufferBrush
    {
        private BufferColor _color;

        /// <summary>
        /// Gets or sets the color of the brush.
        /// </summary>
        public BufferColor Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Creates a new SolidBufferBrush of a specified color.
        /// </summary>
        /// <param name="color">The color of the brush.</param>
        public SolidBufferBrush(BufferColor color)
        {
            _color = color;
        }

        /// <summary>
        /// Gets the color of the brush at the specified coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns></returns>
        public override BufferColor GetColor(int x, int y)
        {
            return _color;
        }

        /// <summary>
        /// A black brush.
        /// </summary>
        public static SolidBufferBrush Black
        {
            get { return new SolidBufferBrush(BufferColor.Black); }
        }

        /// <summary>
        /// A dark blue brush.
        /// </summary>
        public static SolidBufferBrush DarkBlue
        {
            get { return new SolidBufferBrush(BufferColor.DarkBlue); }
        }

        /// <summary>
        /// A dark green brush.
        /// </summary>
        public static SolidBufferBrush DarkGreen
        {
            get { return new SolidBufferBrush(BufferColor.DarkGreen); }
        }

        /// <summary>
        /// A dark cyan brush.
        /// </summary>
        public static SolidBufferBrush DarkCyan
        {
            get { return new SolidBufferBrush(BufferColor.DarkCyan); }
        }

        /// <summary>
        /// A dark red brush.
        /// </summary>
        public static SolidBufferBrush DarkRed
        {
            get { return new SolidBufferBrush(BufferColor.DarkRed); }
        }

        /// <summary>
        /// A dark magenta brush.
        /// </summary>
        public static SolidBufferBrush DarkMagenta
        {
            get { return new SolidBufferBrush(BufferColor.DarkMagenta); }
        }

        /// <summary>
        /// A dark yellow brush.
        /// </summary>
        public static SolidBufferBrush DarkYellow
        {
            get { return new SolidBufferBrush(BufferColor.DarkYellow); }
        }

        /// <summary>
        /// A gray brush.
        /// </summary>
        public static SolidBufferBrush Gray
        {
            get { return new SolidBufferBrush(BufferColor.Gray); }
        }

        /// <summary>
        /// A dark gray brush.
        /// </summary>
        public static SolidBufferBrush DarkGray
        {
            get { return new SolidBufferBrush(BufferColor.DarkGray); }
        }

        /// <summary>
        /// A blue brush.
        /// </summary>
        public static SolidBufferBrush Blue
        {
            get { return new SolidBufferBrush(BufferColor.Blue); }
        }

        /// <summary>
        /// A green brush.
        /// </summary>
        public static SolidBufferBrush Green
        {
            get { return new SolidBufferBrush(BufferColor.Green); }
        }

        /// <summary>
        /// A cyan brush.
        /// </summary>
        public static SolidBufferBrush Cyan
        {
            get { return new SolidBufferBrush(BufferColor.Cyan); }
        }

        /// <summary>
        /// A red brush.
        /// </summary>
        public static SolidBufferBrush Red
        {
            get { return new SolidBufferBrush(BufferColor.Red); }
        }

        /// <summary>
        /// A magenta brush.
        /// </summary>
        public static SolidBufferBrush Magenta
        {
            get { return new SolidBufferBrush(BufferColor.Magenta); }
        }

        /// <summary>
        /// A yellow brush.
        /// </summary>
        public static SolidBufferBrush Yellow
        {
            get { return new SolidBufferBrush(BufferColor.Yellow); }
        }

        /// <summary>
        /// A white brush.
        /// </summary>
        public static SolidBufferBrush White
        {
            get { return new SolidBufferBrush(BufferColor.White); }
        }

        /// <summary>
        /// An identity brush.
        /// </summary>
        public static SolidBufferBrush Identity
        {
            get { return new SolidBufferBrush(BufferColor.Identity); }
        }
    }
}
