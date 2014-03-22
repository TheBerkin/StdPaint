using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    public class PainterMouseEventArgs : EventArgs
    {
        Point _point;

        public Point UnitLocation
        {
            get { return _point; }
        }

        internal PainterMouseEventArgs(Point point)
        {
            _point = point;
        }
    }
}
