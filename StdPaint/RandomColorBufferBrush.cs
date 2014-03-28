using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// Produces a random pattern of colors determined by a random number generator.
    /// </summary>
    public class RandomColorBufferBrush : BufferBrush
    {
        private Random _rand;
        private BufferColor[] _colors;

        /// <summary>
        /// Creates a new RandomColorBufferBrush with the specified colors.
        /// </summary>
        /// <param name="colors">The colors that the brush will use.</param>
        public RandomColorBufferBrush(params BufferColor[] colors)
        {
            _colors = colors;
            _rand = new Random();
        }

        /// <summary>
        /// The colors used by the brush instance.
        /// </summary>
        public BufferColor[] Colors
        {
            get { return _colors; }
            set { _colors = value; }
        }

        /// <summary>
        /// Gets the color of the brush at the specified coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns></returns>
        public override BufferColor GetColor(int x, int y)
        {
            return _colors[_rand.Next(_colors.Length)];
        }
    }
}
