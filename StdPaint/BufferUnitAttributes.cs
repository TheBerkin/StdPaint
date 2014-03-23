using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// Defines color attributes for a buffer unit.
    /// </summary>
    [Flags]
    public enum BufferUnitAttributes : short
    {
        /// <summary>
        /// No attributes. Gives black.
        /// </summary>
        None = 0x0000,
        /// <summary>
        /// Gives a dark blue color to the foreground.
        /// </summary>
        ForegroundBlue = 0x0001,
        /// <summary>
        /// Gives a dark green color to the foreground.
        /// </summary>
        ForegroundGreen = 0x0002,
        /// <summary>
        /// Gives a dark red color to the foreground.
        /// </summary>
        ForegroundRed = 0x0004,
        /// <summary>
        /// Brightens the foreground color.
        /// </summary>
        ForegroundIntensity = 0x0008,
        /// <summary>
        /// Gives a dark blue color to the background.
        /// </summary>
        BackgroundBlue = 0x0010,
        /// <summary>
        /// Gives a dark green color to the background.
        /// </summary>
        BackgroundGreen = 0x0020,
        /// <summary>
        /// Gives a dark red color to the background.
        /// </summary>
        BackgroundRed = 0x0040,
        /// <summary>
        /// Brightens the background color.
        /// </summary>
        BackgroundIntensity = 0x0080
    }
}
