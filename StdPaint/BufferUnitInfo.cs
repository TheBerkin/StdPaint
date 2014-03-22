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
        public char CharData;
        public BufferUnitAttributes Attributes;
    }
}
