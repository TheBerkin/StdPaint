using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// Defines colors for a console buffer cell.
    /// </summary>
    public enum BufferColor : int
    {
        /// <summary>
        /// The blackest black you could ever wish for.
        /// </summary>
        Black = 0,
        /// <summary>
        /// Some call this "navy".
        /// </summary>
        DarkBlue = 1,
        /// <summary>
        /// Smells like pine.
        /// </summary>
        DarkGreen = 2,
        /// <summary>
        /// A less exciting cyan.
        /// </summary>
        DarkCyan = 3,
        /// <summary>
        /// Paint with the blood of your enemies.
        /// </summary>
        DarkRed = 4,
        /// <summary>
        /// Almost purple, but not quite.
        /// </summary>
        DarkMagenta = 5,
        /// <summary>
        /// This is the most disgusting color ever. It is an abomination to the visible spectrum.
        /// </summary>
        DarkYellow = 6,
        /// <summary>
        /// A lovely light gray.
        /// </summary>
        Gray = 7,
        /// <summary>
        /// A depressing dark gray.
        /// </summary>
        DarkGray = 8,
        /// <summary>
        /// Microsoft's favorite color.
        /// </summary>
        Blue = 9,
        /// <summary>
        /// An annoying lime green that will hurt your eyes after a while.
        /// </summary>
        Green = 10,
        /// <summary>
        /// Blue and green conceived a child and this was it.
        /// </summary>
        Cyan = 11,
        /// <summary>
        /// Useful for making people nervous.
        /// </summary>
        Red = 12,
        /// <summary>
        /// May spawn singing dinosaurs.
        /// </summary>
        Magenta = 13,
        /// <summary>
        /// Get some yellow action going.
        /// </summary>
        Yellow = 14,
        /// <summary>
        /// An uninteresting white.
        /// </summary>
        White = 15,
        /// <summary>
        /// Instructs the engine not to change the color of a unit.
        /// </summary>
        Identity = 16
    }
}
