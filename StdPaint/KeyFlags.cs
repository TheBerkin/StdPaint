using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyIO;

namespace StdPaint
{
    class KeyFlags
    {
        private BitField bitfield;
        private uint info;

        public KeyFlags(uint info)
        {
            bitfield = BitField.FromValue(info);
        }

        public bool Released
        {
            get { return bitfield[7]; }
        }
    }
}
