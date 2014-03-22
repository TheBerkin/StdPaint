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
        None = 0x0000,
        ForegroundBlue = 0x0001,
        ForegroundGreen = 0x0002,
        ForegroundRed = 0x0004,
        ForegroundIntensity = 0x0008,
        BackgroundBlue = 0x0010,
        BackgroundGreen = 0x0020,
        BackgroundRed = 0x0040,
        BackgroundIntensity = 0x0080
    }
}
