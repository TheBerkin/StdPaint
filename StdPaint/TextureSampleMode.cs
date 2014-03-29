using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// Defines texture sampling techniques.
    /// </summary>
    public enum TextureSampleMode
    {
        /// <summary>
        /// Tile the texture.
        /// </summary>
        Tile = 0,
        /// <summary>
        /// Clamp texture coordinates.
        /// </summary>
        Clamp = 1,
        /// <summary>
        /// Sample from explicit coordinates; give black for out-of-range coordinates.
        /// </summary>
        Explicit = 2
    }
}
