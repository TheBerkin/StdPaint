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
        internal static readonly int SizeBytes = Marshal.SizeOf(typeof(BufferUnitInfo));

        /// <summary>
        /// The character displayed in the buffer unit.
        /// </summary>
        public char CharData;
        

        internal short _attrs;

        /// <summary>
        /// The raw color attributes for this buffer unit.
        /// </summary>
        public BufferUnitAttributes Attributes
        {
            get { return (BufferUnitAttributes)_attrs; }
            set { _attrs = (short)value; }
        }

        /// <summary>
        /// Gets the foreground color.
        /// </summary>
        public BufferColor ForeColor
        {
            get { return (BufferColor)(_attrs & 0x0F); }
            set
            {
                if (value == BufferColor.Identity) return;
                _attrs = (short)((_attrs & 0xF0) | (int)value);
            }
        }

        /// <summary>
        /// Gets the background color.
        /// </summary>
        public BufferColor BackColor
        {
            get { return (BufferColor)(_attrs >> 4); }
            set
            {
                if (value == BufferColor.Identity) return;
                _attrs = (short)((_attrs & 0x0F) | ((int)value << 4));
            }
        }
    }
}
