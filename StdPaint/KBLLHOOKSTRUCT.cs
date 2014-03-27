using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace StdPaint
{
    [StructLayout(LayoutKind.Sequential)]
    struct KBLLHOOKSTRUCT
    {
        public uint KeyCode;
        public uint ScanCode;
        private uint Flags;
        public uint Time;
        public IntPtr ExtraInfo;

        public KeyFlags GetFlags()
        {
            return new KeyFlags(Flags);
        }
    }
}
