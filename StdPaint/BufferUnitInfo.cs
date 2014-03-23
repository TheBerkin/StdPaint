using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace StdPaint
{
    /// <summary>
    /// Defines color and character information for a buffer unit.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BufferUnitInfo // CHAR_INFO
    {
        /// <summary>
        /// The character displayed in the buffer unit.
        /// </summary>
        public char CharData;
        /// <summary>
        /// The color attributes for the buffer unit.
        /// </summary>
        public BufferUnitAttributes Attributes;
    }
}
