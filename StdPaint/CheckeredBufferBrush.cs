using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// A buffer brush that produces a checkered pattern when used.
    /// </summary>
    public class CheckeredBufferBrush : BufferBrush
    {
        private BufferColor _ca, _cb;

        /// <summary>
        /// Creates a new CheckeredBufferBrush with the specified colors.
        /// </summary>
        /// <param name="colorA">The first color in the pattern.</param>
        /// <param name="colorB">The second color in the pattern.</param>
        public CheckeredBufferBrush(BufferColor colorA, BufferColor colorB)
        {
            _ca = colorA;
            _cb = colorB;
        }

        /// <summary>
        /// The first color in the pattern.
        /// </summary>
        public BufferColor ColorA
        {
            get { return _ca; }
            set { _ca = value; }
        }

        /// <summary>
        /// The second color in the pattern.
        /// </summary>
        public BufferColor ColorB
        {
            get { return _cb; }
            set { _cb = value; }
        }

        public override BufferColor GetColor(int x, int y)
        {
            return (x + y) % 2 == 0 ? _ca : _cb;
        }
    }
}
