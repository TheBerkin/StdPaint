using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// Provides rendering capabilities for a 4x8 seven-segment display.
    /// </summary>
    public class SevenSegmentDisplay
    {
        /*           A
         *        |======|
         *      B |    C |
         *      D |======|
         *      E |    F |
         *        |======|
         *           G
         * */

        private static Point[][] segments = {
                                                new[] { new Point(0, 0), new Point(3, 0) },
                                                new[] { new Point(0, 0), new Point(0, 2) },
                                                new[] { new Point(3, 0), new Point(3, 3) },
                                                new[] { new Point(0, 3), new Point(3, 3) },
                                                new[] { new Point(0, 3), new Point(0, 6) },
                                                new[] { new Point(3, 3), new Point(3, 6) },
                                                new[] { new Point(0, 6), new Point(3, 6) }
                                            };

        private static int[][] layouts = {
                                             new[] { 0, 1, 2, 4, 5, 6 },           // 0
                                             new[] { 2, 5 },                       // 1
                                             new[] { 0, 2, 3, 4, 6 },              // 2
                                             new[] { 0, 2, 3, 5, 6 },              // 3
                                             new[] { 1, 2, 3, 5 },                 // 4
                                             new[] { 0, 1, 3, 5, 6 },              // 5
                                             new[] { 0, 1, 3, 4, 5, 6 },           // 6
                                             new[] { 0, 2, 5 },                    // 7
                                             new[] { 0, 1, 2, 3, 4, 5, 6 },        // 8
                                             new[] { 0, 1, 2, 3, 5, 6 }            // 9
                                         };

        /// <summary>
        /// The location of the display.
        /// </summary>
        public Point Location;

        /// <summary>
        /// The foreground brush of the display.
        /// </summary>
        public BufferBrush ForeBrush = SolidBufferBrush.Red;

        /// <summary>
        /// The background brush of the display.
        /// </summary>
        public BufferBrush BackBrush = SolidBufferBrush.Identity;

        private long _value;
        private int[] _digits;
        private int _digitsUsed;
        private int _digitCount;

        /// <summary>
        /// Gets or sets the current value of the display.
        /// </summary>
        public long Value
        {
            get { return _value; }
            set
            {
                _value = value;
                Update();
            }
        }

        /// <summary>
        /// Gets or sets the minimum number of digits to display.
        /// </summary>
        public int Digits
        {
            get { return _digitCount; }
            set 
            {
                _digitCount = value;
                Update();
            }
        }

        /// <summary>
        /// Initializes a new instance of the StdPaint.DigitDisplay class with the specified location and value.
        /// </summary>
        /// <param name="location">The location of the display.</param>
        /// <param name="digits">The minimum number of digits to display.</param>
        /// <param name="value">The value of the display.</param>
        public SevenSegmentDisplay(Point location, int digits, int value = 0)
        {
            this.Location = location;
            _value = value;
            _digits = new int[20];
            _digitCount = digits;
            Update();
        }

        private void Update()
        {
            char[] chars = String.Format("{0:D" + _digitCount + "}", _value).ToCharArray();
            _digitsUsed = 0;

            if (_value < 0)
            {
                _digitsUsed++;
            }

            foreach(char c in chars)
            {
                if (Char.IsDigit(c))
                {
                    _digits[_digitsUsed] = c - 48;
                    _digitsUsed++;
                }
            }
        }

        /// <summary>
        /// Draws the display to the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer to draw to.</param>
        /// <param name="alignment">The alignment of the display to the draw position.</param>
        public void Draw(ConsoleBuffer buffer, Alignment alignment = Alignment.Left)
        {
            Point loc = this.Location;
            Point dloc = loc;
            int ox = alignment == Alignment.Right ? _digitsUsed * 5 - 1 : -1;
            buffer.DrawBox(loc.X - ox, loc.Y, _digitsUsed * 5 - 1, 7, this.BackBrush);
            for(int i = 0; i < _digitsUsed; i++)
            {
                dloc.X = loc.X + i * 5 - ox;

                // Draw negative sign
                if (_value < 0 && i == 0)
                {
                    buffer.DrawLine(segments[3][0] + dloc, segments[3][1] + dloc, this.ForeBrush);
                    continue;
                }

                // Draw digit
                foreach(int j in layouts[_digits[i]])
                {
                    buffer.DrawLine(segments[j][0] + dloc, segments[j][1] + dloc, this.ForeBrush);
                }
            }
        }
    }
}
