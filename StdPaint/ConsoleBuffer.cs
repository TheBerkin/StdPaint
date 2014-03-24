using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// Represents a two-dimensional buffer of characters that can be written to the console.
    /// </summary>
    public class ConsoleBuffer
    {
        int _width, _height;
        BufferUnitInfo[,] _buffer;

        /// <summary>
        /// Initializes a new ConsoleBuffer instance with the specified dimensions.
        /// </summary>
        /// <param name="width">The width of the buffer, in units.</param>
        /// <param name="height">The height of the buffer, in units.</param>
        public ConsoleBuffer(int width, int height)
        {
            _width = width;
            _height = height;
            _buffer = new BufferUnitInfo[height, width];
        }

        /// <summary>
        /// Creates a buffer whose size matches the current unit dimensions of the console.
        /// </summary>
        /// <returns></returns>
        public static ConsoleBuffer CreateScreenBuffer()
        {
            return new ConsoleBuffer(Console.BufferWidth, Console.BufferHeight);
        }

        /// <summary>
        /// Gets the array of buffer units contained in this instance.
        /// </summary>
        public BufferUnitInfo[,] Buffer
        {
            get { return _buffer; }
        }

        /// <summary>
        /// Gets the width of the buffer, in units.
        /// </summary>
        public int Width
        {
            get { return _width; }
        }

        /// <summary>
        /// Gets the height of the buffer, in units.
        /// </summary>
        public int Height
        {
            get { return _height; }
        }

        /// <summary>
        /// Clears all units in the buffer, optionally specifying attributes to fill the buffer with.
        /// </summary>
        /// <param name="clearAttributes">The attributes to fill the buffer with.</param>
        public void Clear(BufferUnitAttributes clearAttributes = BufferUnitAttributes.None)
        {            
            for (int i = 0; i < _width; i++)
            for (int j = 0; j < _height; j++)
            {
                _buffer[j, i].Attributes = clearAttributes;
                _buffer[j, i].CharData = '\0';
            }
        }

        /// <summary>
        /// Sets attributes for a specific unit in the buffer.
        /// </summary>
        /// <param name="x">The X coordinate of the unit.</param>
        /// <param name="y">The Y coordinate of the unit.</param>
        /// <param name="attributes">The attributes to assign to the unit.</param>
        public void SetUnitAttributes(int x, int y, BufferUnitAttributes attributes)
        {
            if (InBounds(x, y))
            {
                _buffer[y, x].Attributes = attributes;
            }
        }

        /// <summary>
        /// Sets the attributes for a specific unit in the buffer.
        /// </summary>
        /// <param name="point">The location of the unit.</param>
        /// <param name="attributes">The attributes to assign to the unit.</param>
        public void SetUnitAttributes(Point point, BufferUnitAttributes attributes)
        {
            SetUnitAttributes(point.X, point.Y, attributes);
        }

        /// <summary>
        /// Sets the character for a specific unit in the buffer.
        /// </summary>
        /// <param name="x">The X coordinate of the unit.</param>
        /// <param name="y">The Y coordinate of the unit.</param>
        /// <param name="c">The character to assign to the unit.</param>
        public void SetUnitCharacter(int x, int y, char c)
        {
            if (InBounds(x, y))
            {
                _buffer[y, x].CharData = c;
            }
        }

        /// <summary>
        /// Sets the character for a specific unit in the buffer.
        /// </summary>
        /// <param name="p">The location of the unit.</param>
        /// <param name="c">The character to assign to the unit.</param>
        public void SetUnitCharacter(Point p, char c)
        {
            SetUnitCharacter(p.X, p.Y, c);
        }

        /// <summary>
        /// Returns the attributes for the specified unit.
        /// </summary>
        /// <param name="x">The X coordinate of the unit.</param>
        /// <param name="y">The Y coordinate of the unit.</param>
        /// <returns></returns>
        public BufferUnitAttributes GetUnitAttributes(int x, int y)
        {
            var attrs = BufferUnitAttributes.None;
            if (InBounds(x,y))
            {
                attrs = _buffer[y, x].Attributes;
            }
            return attrs;
        }

        /// <summary>
        /// Draws a solid box with the specified dimensions to the buffer.
        /// </summary>
        /// <param name="x">the X coordinate of the box.</param>
        /// <param name="y">The Y coordinate of the box.</param>
        /// <param name="w">The width of the box.</param>
        /// <param name="h">The height of the box.</param>
        /// <param name="attributes">The attributes to draw the box with.</param>
        public void DrawBox(int x, int y, int w, int h, BufferUnitAttributes attributes)
        {
            if (w < 0)
            {
                w *= -1;
                x -= w;
            }
            if (h < 0)
            {
                h *= -1;
                y -= h;
            }

            var b = _buffer;

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    if (InBounds(x + i, y + j))
                    {
                        b[y + j, x + i].Attributes = attributes;
                    }
                }
        }

        /// <summary>
        /// Prints a string to the buffer with the specified attributes and alignment.
        /// </summary>
        /// <param name="x">The X coordinate to start printing at.</param>
        /// <param name="y">The Y coordinate to start printing at.</param>
        /// <param name="text">The string to print.</param>
        /// <param name="attributes">The attributes to assign to the string.</param>
        /// <param name="alignment">The alignment of the string.</param>
        public void DrawString(int x, int y, string text, BufferUnitAttributes attributes, TextAlignment alignment = TextAlignment.Left)
        {
            var b = _buffer;
            string[] lines = text.Split(new[] { '\n' });
            for (int i = 0; i < lines.Length; i++)
            {
                int len = lines[i].Length;
                for (int j = 0; j < len; j++)
                {
                    switch (alignment)
                    {
                        case TextAlignment.Left:
                            if (InBounds(x + j, y + i))
                            {
                                b[y + i, x + j].CharData = lines[i][j];
                                b[y + i, x + j].Attributes = attributes;
                            }
                            break;
                        case TextAlignment.Right:
                            if (InBounds(x - len + j, y + i))
                            {
                                b[y + i, x - len + j].CharData = lines[i][j];
                                b[y + i, x - len + j].Attributes = attributes;
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Draws a string to the buffer with the specified alignment.
        /// </summary>
        /// <param name="x">The X coordinate to start printing at.</param>
        /// <param name="y">The Y coordinate to start printing at.</param>
        /// <param name="alignment">The alinment to assign to the string.</param>
        /// <param name="args">An array of strings and attributes to print.</param>
        public void DrawString(int x, int y, TextAlignment alignment, params object[] args)
        {
            BufferUnitAttributes attributes = BufferUnitAttributes.ForegroundIntensity;
            foreach (var arg in args)
            {
                if (arg is BufferUnitAttributes)
                {
                    attributes = (BufferUnitAttributes)arg;
                }
                else
                {
                    string[] lines = arg.ToString().Split(new[] { '\n' });
                    foreach (string line in lines)
                    {
                        DrawString(x, lines.Length > 1 ? y++ : y, line, attributes, alignment);
                    }
                }
            }
        }

        /// <summary>
        /// Draws a line to the buffer.
        /// </summary>
        /// <param name="x">The starting X coordinate of the line.</param>
        /// <param name="y">The starting Y coordinate of the line.</param>
        /// <param name="x2">The ending X coordinate of the line.</param>
        /// <param name="y2">The ending Y coordinate of the line.</param>
        /// <param name="color">The color of the line.</param>
        public void DrawLine(int x, int y, int x2, int y2, BufferUnitAttributes color)
        {
            var b = _buffer;
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                b[y, x].Attributes = color;
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }

        /// <summary>
        /// Draws the contents of another buffer onto this buffer at the specified location.
        /// </summary>
        /// <param name="buffer">The buffer to draw.</param>
        /// <param name="x">The X position to begin drawing at.</param>
        /// <param name="y">The Y position to begin drawing at.</param>
        /// <param name="drawMode">Specified how the buffer should be drawn.</param>
        public void DrawBuffer(ConsoleBuffer buffer, int x, int y, BufferDrawMode drawMode)
        {
            var b = _buffer;
            var b2 = buffer.Buffer;
            var w = buffer.Width;
            var h = buffer.Height;
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    if (InBounds(x + i, y + j))
                    {
                        switch(drawMode)
                        {
                            case BufferDrawMode.Additive:
                                b[j + y, i + x].Attributes |= b2[j, i].Attributes;
                                break;
                            case BufferDrawMode.DrawOver:
                                b[j + y, i + x].Attributes = b2[j, i].Attributes;
                                break;
                            case BufferDrawMode.IgnoreBlack:
                                if (b2[j,i].Attributes != BufferUnitAttributes.None)
                                {
                                    b[j + y, i + x].Attributes = b2[j, i].Attributes;
                                }
                                break;
                        }
                        
                        b[j + y, i + x].CharData = b2[j, i].CharData;
                    }
                }
        }

        private bool InBounds(int x, int y)
        {
            return x >= 0 && x < _width && y >= 0 && y < _height;
        }
    }
}
