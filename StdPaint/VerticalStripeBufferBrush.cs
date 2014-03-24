using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// A buffer brush that produces horizontal stripes of different colors.
    /// </summary>
    public class VerticalStripeBufferBrush : BufferBrush
    {
        private BufferColor[] _colors;
        private int _thickness;

        /// <summary>
        /// Creates a new VerticalStripeBufferBrush with the specified colors.
        /// </summary>
        /// <param name="colors">The colors for the stripes.</param>
        public VerticalStripeBufferBrush(int thickness, params BufferColor[] colors)
        {
            if (thickness <= 0)
            {
                throw new ArgumentException("Thickness must be at least 1.");
            }

            _thickness = thickness;
            _colors = colors;
        }

        public override BufferColor GetColor(int x, int y)
        {
            return _colors[(Math.Abs(x) / _thickness) % _colors.Length];
        }
    }
}
