﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace StdPaint
{
    /// <summary>
    /// Represents a two-dimensional buffer of characters that can be written to the console.
    /// </summary>
    public class ConsoleBuffer
    {
        #region Non-public fields
        /// <summary>
        /// Buffer dimensions
        /// </summary>
        protected readonly int _width, _height;

        /// <summary>
        /// Buffer data
        /// </summary>
        protected BufferUnitInfo[,] _buffer;

        #endregion

        #region Public fields

        /// <summary>
        /// The total number of units in this buffer.
        /// </summary>
        public readonly int UnitCount;

        #endregion

        #region Constructors

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
            UnitCount = _buffer.Length;
        }

        private ConsoleBuffer(int width, int height, BufferUnitInfo[,] bufferData)
        {
            _width = width;
            _height = height;
            _buffer = bufferData;
            UnitCount = _buffer.Length;
        }

        #endregion

        #region Public methods

        #region Texture functionality

        /// <summary>
        /// Returns the unit background color at the specified texture coordinates.
        /// </summary>
        /// <param name="u">The U coordinate.</param>
        /// <param name="v">The V coordinate.</param>
        /// <param name="mode">The sampling mode to use.</param>
        /// <returns></returns>
        public BufferColor GetColorFromUV(double u, double v, TextureSampleMode mode = TextureSampleMode.Tile)
        {
            switch(mode)
            {
                case TextureSampleMode.Clamp:
                    if (u > 1.0)
                    {
                        u = 1.0;
                    }
                    else if (u < 0.0)
                    {
                        u = 0.0;
                    }
                    if (v > 1.0)
                    {
                        v = 1.0;
                    }
                    else if (v < 0.0)
                    {
                        v = 0.0;
                    }
                    return _buffer[(int)(v * (_height - 1)), (int)(u * (_width - 1))].BackColor;                
                case TextureSampleMode.Explicit:
                    if (u > 1.0 || u < 0.0 || v > 1.0 || v < 0.0)
                    {
                        return BufferColor.Black;
                    }
                    else
                    {
                        return _buffer[(int)(v * (_height - 1)), (int)(u * (_width - 1))].BackColor;
                    }
                default:
                case TextureSampleMode.Tile:
                    return _buffer[Utils.Mod((int)(v * _height), _height), Utils.Mod((int)(u * _width), _width)].BackColor;
            }
        }
        #endregion

        #region Static methods

        /// <summary>
        /// Creates a buffer whose size matches the current unit dimensions of the console.
        /// </summary>
        /// <returns></returns>
        public static ConsoleBuffer CreateScreenBuffer()
        {
            return new ConsoleBuffer(Console.BufferWidth, Console.BufferHeight);
        }

        /// <summary>
        /// Creates a resampled copy of a console buffer using the specified dimensions.
        /// </summary>
        /// <param name="buffer">The buffer to be resampled.</param>
        /// <param name="width">The width of the resampled buffer.</param>
        /// <param name="height">The height of the resampled buffer.</param>
        /// <returns></returns>
        public static ConsoleBuffer ResampledCopy(ConsoleBuffer buffer, int width, int height)
        {
            var sb = new ConsoleBuffer(width, height);
            double dw = ((double)buffer.Width / width);
            double dh = ((double)buffer.Height / height);

            for(int i = 0; i < width; i++)
            for(int j = 0; j < height; j++)
            {
                sb._buffer[j, i].BackColor = buffer._buffer[(int)(j * dh), (int)(i * dw)].BackColor;
            }

            return sb;
        }

        #endregion

        #region Saving/Loading/Cloning

        /// <summary>
        /// Loads buffer data from a file and returns it as a ConsoleBuffer object.
        /// </summary>
        /// <param name="path">The path to the buffer file.</param>
        /// <returns></returns>
        public static ConsoleBuffer FromFile(string path)
        {
            using(BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                int width = reader.ReadInt32();
                int height = reader.ReadInt32();
                BufferUnitInfo[,] bufferData = new BufferUnitInfo[height, width];
                BufferUnitInfo temp = new BufferUnitInfo();
                for(int i = 0; i < width; i++)
                for(int j = 0; j < height; j++)
                {
                    temp._attrs = reader.ReadInt16();
                    temp.CharData = reader.ReadChar();
                    bufferData[j, i] = temp;
                }
                return new ConsoleBuffer(width, height, bufferData);
            }
        }

        /// <summary>
        /// Returns a new ConsoleBuffer instance containing a copy of the buffer's contents.
        /// </summary>
        /// <returns></returns>
        public ConsoleBuffer Clone()
        {
            var buffer = new ConsoleBuffer(_width, _height);
            Array.Copy(_buffer, buffer._buffer, UnitCount);
            return buffer;
        }

        /// <summary>
        /// Writes the buffer data to a file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        public void Save(string path)
        {
            using(BinaryWriter writer = new BinaryWriter(File.Create(path)))
            {
                writer.Write(_width);
                writer.Write(_height);
                for(int i = 0; i < _width; i++)
                for(int j = 0; j < _height; j++)
                {
                    writer.Write((short)_buffer[j, i]._attrs);
                    writer.Write(_buffer[j, i].CharData);
                }
            }
        }

        #endregion

        #region Properties

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

        #endregion

        #region Clear

        /// <summary>
        /// Clears all units in the buffer, optionally specifying a color to fill the buffer with.
        /// </summary>
        /// <param name="color">The color to fill the buffer with.</param>
        public unsafe void Clear(BufferColor color = BufferColor.Black)
        {
            fixed (BufferUnitInfo* b = _buffer)
            {
                for (int i = UnitCount; i >= 0; i--)
                {                    
                    b[i].BackColor = color;
                    b[i].ForeColor = color;
                    b[i].CharData = '\0';
                }
            }
        }

        /// <summary>
        /// Fills the entire buffer with a brush.
        /// </summary>
        /// <param name="brush">The brush to fill the buffer with.</param>
        public unsafe void ClearBrush(BufferBrush brush)
        {
            for (int i = _width - 1; i >= 0; i--)
            for (int j = _height - 1; j >= 0; j--)
            {
                _buffer[j, i].ForeColor = brush.GetColor(i, j);
                _buffer[j, i].BackColor = brush.GetColor(i, j);
                _buffer[j, i].CharData = '\0';
            }
        }

        #endregion

        #region Flood fill

        /// <summary>
        /// Flood fills a closed region containing the specified coordinates with a color.
        /// </summary>
        /// <param name="x">The X coordinate to begin filling at.</param>
        /// <param name="y">The Y coordinate to begin filling at.</param>
        /// <param name="color">The color to fill the region with.</param>
        public void FloodFill(int x, int y, BufferColor color)
        {
            if (!InBounds(x, y)) return;
            var initColor = _buffer[y, x].BackColor;
            if (color == initColor) return;
            List<Point> queue = new List<Point>(32);
            queue.Add(new Point(x, y));
            Point p;
            int w, e, j;
            for (int i = 0; i < queue.Count; i++)
            {
                p = queue[i];
                w = e = p.X;
                while(w - 1 >= 0)
                {
                    if (_buffer[p.Y, w - 1].BackColor == initColor)
                    {
                        w--;
                    }
                    else
                    {
                        break;
                    }
                }
                while (e + 1 < _width)
                {
                    if (_buffer[p.Y, e + 1].BackColor == initColor)
                    {
                        e++;
                    }
                    else
                    {
                        break;
                    }
                }
                for (j = w; j <= e; j++)
                {
                    _buffer[p.Y, j].BackColor = color;
                    if (p.Y + 1 < _height)
                    {
                        if (_buffer[p.Y + 1, j].BackColor == initColor)
                        {
                            queue.Add(new Point(j, p.Y + 1));
                        }
                    }
                    if (p.Y - 1 >= 0)
                    {
                        if (_buffer[p.Y - 1, j].BackColor == initColor)
                        {
                            queue.Add(new Point(j, p.Y - 1));
                        }
                    }                    
                }                
            }
        }

        /// <summary>
        /// Flood fills a closed region containing the specified coordinates with a brush.
        /// </summary>
        /// <param name="x">The X coordinate to begin filling at.</param>
        /// <param name="y">The Y coordinate to begin filling at.</param>
        /// <param name="brush">The brush to fill the region with.</param>
        public void FloodFill(int x, int y, BufferBrush brush)
        {
            if (!InBounds(x, y)) return;
            var initColor = _buffer[y, x].BackColor;
            if (brush.GetColor(x,y) == initColor) return;
            List<Point> queue = new List<Point>(32);
            queue.Add(new Point(x, y));
            Point p;
            int w, e, j;
            for (int i = 0; i < queue.Count; i++)
            {
                p = queue[i];
                w = e = p.X;
                while (w - 1 >= 0)
                {
                    if (_buffer[p.Y, w - 1].BackColor == initColor)
                    {
                        w--;
                    }
                    else
                    {
                        break;
                    }
                }
                while (e + 1 < _width)
                {
                    if (_buffer[p.Y, e + 1].BackColor == initColor)
                    {
                        e++;
                    }
                    else
                    {
                        break;
                    }
                }
                for (j = w; j <= e; j++)
                {
                    _buffer[p.Y, j].BackColor = brush.GetColor(j, p.Y);
                    if (p.Y + 1 < _height)
                    {
                        if (_buffer[p.Y + 1, j].BackColor == initColor)
                        {
                            queue.Add(new Point(j, p.Y + 1));
                        }
                    }
                    if (p.Y - 1 >= 0)
                    {
                        if (_buffer[p.Y - 1, j].BackColor == initColor)
                        {
                            queue.Add(new Point(j, p.Y - 1));
                        }
                    }
                }
            }
        }

        #endregion

        #region Unit manipulation

        /// <summary>
        /// Sets attributes for a specific unit in the buffer.
        /// </summary>
        /// <param name="x">The X coordinate of the unit.</param>
        /// <param name="y">The Y coordinate of the unit.</param>
        /// <param name="attributes">The attributes to assign to the unit.</param>
        public void SetUnitAttributes(int x, int y, BufferUnitAttributes attributes)
        {
            if (InBounds(ref x, ref y))
            {
                _buffer[y, x]._attrs = (short)attributes;
            }
        }

        /// <summary>
        /// Sets the attributes for a specific unit in the buffer.
        /// </summary>
        /// <param name="point">The location of the unit.</param>
        /// <param name="attributes">The attributes to assign to the unit.</param>
        public void SetUnitAttributes(Point point, BufferUnitAttributes attributes)
        {
            if (!InBounds(ref point)) return;
            SetUnitAttributes(point.X, point.Y, attributes);
        }

        /// <summary>
        /// Sets the background color for a specific unit in the buffer.
        /// </summary>
        /// <param name="point">The location of the unit.</param>
        /// <param name="color">The background color to set the unit to.</param>
        public void SetUnitBackColor(Point point, BufferColor color)
        {
            if (!InBounds(ref point)) return;
            _buffer[point.Y, point.X].BackColor = color;
        }

        /// <summary>
        /// Sets the background color for a specific unit in the buffer.
        /// </summary>
        /// <param name="x">The X coordinate of the unit.</param>
        /// <param name="y">The Y coordinate of the unit.</param>
        /// <param name="color">The background color to set the unit to.</param>
        public void SetUnitBackColor(int x, int y, BufferColor color)
        {
            if (!InBounds(ref x, ref y)) return;
            _buffer[y, x].BackColor = color;
        }

        /// <summary>
        /// Sets the foreground color for a specific unit in the buffer.
        /// </summary>
        /// <param name="point">The location of the unit.</param>
        /// <param name="color">The foreground color to set the unit to.</param>
        public void SetUnitForeColor(Point point, BufferColor color)
        {
            if (!InBounds(ref point)) return;
            _buffer[point.Y, point.X].ForeColor = color;
        }

        /// <summary>
        /// Sets the foreground color for a specific unit in the buffer.
        /// </summary>
        /// <param name="x">The X coordinate of the unit.</param>
        /// <param name="y">The Y coordinate of the unit.</param>
        /// <param name="color">The foreground color to set the unit to.</param>
        public void SetUnitForeColor(int x, int y, BufferColor color)
        {
            if (InBounds(ref x, ref y))
            {
                _buffer[y, x].ForeColor = color;
            }
        }

        /// <summary>
        /// Gets the foreground color of the specified unit.
        /// </summary>
        /// <param name="point">The location of the unit.</param>
        /// <returns></returns>
        public BufferColor GetUnitForeColor(Point point)
        {
            if (InBounds(ref point))
            {
                return _buffer[point.Y, point.X].ForeColor;
            }
            return BufferColor.Black;
        }

        /// <summary>
        /// Gets the foreground color of the specified unit.
        /// </summary>
        /// <param name="x">The X coordinate of the unit.</param>
        /// <param name="y">The Y coordinate of the unit.</param>
        /// <returns></returns>
        public BufferColor GetUnitForeColor(int x, int y)
        {
            if (InBounds(x,y))
            {
                return _buffer[y, x].ForeColor;
            }
            return BufferColor.Black;
        }

        /// <summary>
        /// Gets the background color of the specified unit.
        /// </summary>
        /// <param name="point">The location of the unit.</param>
        /// <returns></returns>
        public BufferColor GetUnitBackColor(Point point)
        {
            if (InBounds(ref point))
            {
                return _buffer[point.Y, point.X].BackColor;
            }
            return BufferColor.Black;
        }

        /// <summary>
        /// Gets the background color of the specified unit.
        /// </summary>
        /// <param name="x">The X coordinate of the unit.</param>
        /// <param name="y">The Y coordinate of the unit.</param>
        /// <returns></returns>
        public BufferColor GetUnitBackColor(int x, int y)
        {
            if (InBounds(x, y))
            {
                return _buffer[y, x].BackColor;
            }
            return BufferColor.Black;
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
        /// Gets the character in the specified unit.
        /// </summary>
        /// <param name="x">The X coordinate of the unit.</param>
        /// <param name="y">The Y coordinate of the unit.</param>
        /// <returns></returns>
        public char GetUnitCharacter(int x, int y)
        {
            if(InBounds(x,y))
            {
                return _buffer[y, x].CharData;
            }
            return '\0';
        }

        /// <summary>
        /// Gets the character in the specified unit.
        /// </summary>
        /// <param name="point">The location of the unit.</param>
        /// <returns></returns>
        public char GetUnitCharacter(Point point)
        {
            if (InBounds(point.X, point.Y))
            {
                return _buffer[point.Y, point.X].CharData;
            }
            return '\0';
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
                attrs = (BufferUnitAttributes)_buffer[y, x]._attrs;
            }
            return attrs;
        }

        #endregion

        #region Boxes

        /// <summary>
        /// Draws a solid box with the specified dimensions to the buffer.
        /// </summary>
        /// <param name="x">The X coordinate of the box.</param>
        /// <param name="y">The Y coordinate of the box.</param>
        /// <param name="w">The width of the box.</param>
        /// <param name="h">The height of the box.</param>
        /// <param name="color">The color to draw the box with.</param>
        public void DrawBox(int x, int y, int w, int h, BufferColor color)
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
                        b[y + j, x + i].BackColor = color;
                    }
                }
        }

        /// <summary>
        /// Draws a solid box with the specified dimensions to the buffer.
        /// </summary>
        /// <param name="x">The X coordinate of the box.</param>
        /// <param name="y">The Y coordinate of the box.</param>
        /// <param name="w">The width of the box.</param>
        /// <param name="h">The height of the box.</param>
        /// <param name="brush">The brush to draw the box with.</param>
        public void DrawBox(int x, int y, int w, int h, BufferBrush brush)
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
                        b[y + j, x + i].BackColor = brush.GetColor(x + i, y + j);
                    }
                }
        }

        /// <summary>
        /// Draws a box with the specified border and fill brushes.
        /// </summary>
        /// <param name="x">The X position of the box.</param>
        /// <param name="y">The Y position of the box.</param>
        /// <param name="w">The width of the box.</param>
        /// <param name="h">The height of the box.</param>
        /// <param name="thickness">The border thickness of the box.</param>
        /// <param name="border">The brush to draw the border with.</param>
        /// <param name="fill">The brush to draw the fill with.</param>
        public void DrawBox(int x, int y, int w, int h, int thickness, BufferBrush border, BufferBrush fill)
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
            if (thickness < 0)
            {
                thickness *= -1;
            }

            var b = _buffer;

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    if (InBounds(x + i, y + j))
                    {
                        if ((i >= thickness && i < w - thickness) && (j >= thickness && j < h - thickness))
                        {
                            b[y + j, x + i].BackColor = fill.GetColor(x + i, y + j);
                        }
                        else
                        {
                            b[y + j, x + i].BackColor = border.GetColor(x + i, y + j);
                        }
                    }
                }
        }

        /// <summary>
        /// Draws a box from the specified rectangle and brush information.
        /// </summary>
        /// <param name="rectangle">The bounds of the box to draw.</param>
        /// <param name="thickness">The border thickness of the box.</param>
        /// <param name="border">the border brush of the box.</param>
        /// <param name="fill">The fill brush of the box.</param>
        public void DrawBox(Rectangle rectangle, int thickness, BufferBrush border, BufferBrush fill)
        {
            DrawBox(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height, thickness, border, fill);
        }

        /// <summary>
        /// Draws a box from the specified rectangle and brush information.
        /// </summary>
        /// <param name="rectangle">The bounds of the box to draw.</param>
        /// <param name="thickness">The border thickness of the box.</param>
        /// <param name="border">the border brush of the box.</param>
        /// <param name="fill">The fill brush of the box.</param>
        public void DrawBox(ref Rectangle rectangle, int thickness, BufferBrush border, BufferBrush fill)
        {
            DrawBox(rectangle.Top, rectangle.Left, rectangle.Width, rectangle.Height, thickness, border, fill);
        }

        #endregion

        #region Circles

        /// <summary>
        /// Draws a solid circle to the buffer with the specified attributes.
        /// </summary>
        /// <param name="x">The X position of the circle, relative to its center.</param>
        /// <param name="y">The Y position of the circle, relative to its center.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="color">The color to draw the circle with.</param>
        public void DrawCircle(int x, int y, int radius, BufferColor color)
        {
            if (radius < 0) radius *= -1;
            int rr = radius * radius;
            for(int i = -radius; i <= radius; i++)
            for (int j = -radius; j <= radius; j++)
            {
                if (i * i + j * j <= rr && InBounds(x + i, y + j))
                {
                    _buffer[y + j, x + i].BackColor = color;
                }                
            }
        }

        /// <summary>
        /// Draws a solid circle to the buffer with the specified attributes.
        /// </summary>
        /// <param name="x">The X position of the circle, relative to its center.</param>
        /// <param name="y">The Y position of the circle, relative to its center.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="brush">The brush to draw the circle with.</param>
        public void DrawCircle(int x, int y, int radius, BufferBrush brush)
        {
            if (radius < 0) radius *= -1;
            int rr = radius * radius;
            for (int i = -radius; i <= radius; i++)
                for (int j = -radius; j <= radius; j++)
                {
                    if (i * i + j * j <= rr && InBounds(x + i, y + j))
                    {
                        _buffer[y + j, x + i].BackColor = brush.GetColor(x + i,y + j);
                    }
                }
        }

        /// <summary>
        /// Draws a circle with the specified radius, border thickness, and attributes for both border and fill.
        /// </summary>
        /// <param name="x">The X position of the circle, relative to its center.</param>
        /// <param name="y">The Y position of the circle, relative to its center.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="thickness">The border thickness of the circle.</param>
        /// <param name="border">The border color for the circle.</param>
        /// <param name="fill">The fill color for the circle.</param>
        public void DrawCircle(int x, int y, int radius, int thickness, BufferColor border, BufferColor fill)
        {
            if (radius < 0) radius *= -1;
            if (thickness < 0) thickness *= -1;
            if (thickness > radius) thickness = radius;
            int rra = radius * radius;
            int rrb = (radius - thickness) * (radius - thickness);
            int d = 0;
            for(int i = -radius; i <= radius; i++)
            for (int j = -radius; j <= radius; j++)
            {
                d = i * i + j * j;
                if (InBounds(x + i, y + j))
                {
                    if(d < rrb)
                    {
                        _buffer[y + j, x + i].BackColor = fill;
                    }
                    else if (d <= rra)
                    {
                        _buffer[y + j, x + i].BackColor = border;
                    }
                }
            }
        }

        /// <summary>
        /// Draws a circle with the specified radius, border thickness, and attributes for both border and fill.
        /// </summary>
        /// <param name="x">The X position of the circle, relative to its center.</param>
        /// <param name="y">The Y position of the circle, relative to its center.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="thickness">The border thickness of the circle.</param>
        /// <param name="border">The border brush for the circle.</param>
        /// <param name="fill">The fill brush for the circle.</param>
        public void DrawCircle(int x, int y, int radius, int thickness, BufferBrush border, BufferBrush fill)
        {
            if (radius < 0) radius *= -1;
            if (thickness < 0) thickness *= -1;
            if (thickness > radius) thickness = radius;
            int rra = radius * radius;
            int rrb = (radius - thickness) * (radius - thickness);
            int d = 0;
            for (int i = -radius; i <= radius; i++)
                for (int j = -radius; j <= radius; j++)
                {
                    d = i * i + j * j;
                    if (InBounds(x + i, y + j))
                    {
                        if (d < rrb)
                        {
                            _buffer[y + j, x + i].BackColor = fill.GetColor(x + i, y + j);
                        }
                        else if (d <= rra)
                        {
                            _buffer[y + j, x + i].BackColor = border.GetColor(x + i, y + j);
                        }
                    }
                }
        }

        #endregion

        #region Strings

        /// <summary>
        /// Prints a string to the buffer with the specified attributes and alignment.
        /// </summary>
        /// <param name="x">The X coordinate to start printing at.</param>
        /// <param name="y">The Y coordinate to start printing at.</param>
        /// <param name="text">The string to print.</param>
        /// <param name="color">The color to assign to the text.</param>
        /// <param name="alignment">The alignment of the string.</param>
        public void DrawString(int x, int y, string text, BufferColor color, Alignment alignment = Alignment.Left)
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
                        case Alignment.Left:
                            if (InBounds(x + j, y + i))
                            {
                                b[y + i, x + j].CharData = lines[i][j];
                                b[y + i, x + j].ForeColor = color;
                            }
                            break;
                        case Alignment.Right:
                            if (InBounds(x - len + j, y + i))
                            {
                                b[y + i, x - len + j].CharData = lines[i][j];
                                b[y + i, x - len + j].ForeColor = color;
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
        public void DrawString(int x, int y, Alignment alignment, params object[] args)
        {
            var color = BufferColor.Gray;            
            foreach (var arg in args)
            {
                if (arg is BufferColor)
                {
                    color = (BufferColor)arg;
                }
                else
                {
                    string[] lines = arg.ToString().Split(new[] { '\n' });
                    foreach (string line in lines)
                    {
                        DrawString(x, lines.Length > 1 ? y++ : y, line, color, alignment);
                    }
                }
            }
        }

        #endregion

        #region Lines

        /// <summary>
        /// Draws a line to the buffer.
        /// </summary>
        /// <param name="x">The starting X coordinate of the line.</param>
        /// <param name="y">The starting Y coordinate of the line.</param>
        /// <param name="x2">The ending X coordinate of the line.</param>
        /// <param name="y2">The ending Y coordinate of the line.</param>
        /// <param name="color">The color of the line.</param>
        public void DrawLine(int x, int y, int x2, int y2, BufferColor color)
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
                if (InBounds(ref x, ref y))
                {
                    b[y, x].BackColor = color;
                }
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
        /// Draws a line to the buffer.
        /// </summary>
        /// <param name="x">The starting X coordinate of the line.</param>
        /// <param name="y">The starting Y coordinate of the line.</param>
        /// <param name="x2">The ending X coordinate of the line.</param>
        /// <param name="y2">The ending Y coordinate of the line.</param>
        /// <param name="brush">The brush to draw the line with.</param>
        public void DrawLine(int x, int y, int x2, int y2, BufferBrush brush)
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
                if (InBounds(ref x, ref y))
                {
                    b[y, x].BackColor = brush.GetColor(x, y);
                }
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
        /// Draws a colored line to the buffer.
        /// </summary>
        /// <param name="a">The starting point of the line.</param>
        /// <param name="b">The ending point of the line.</param>
        /// <param name="color">The color of the line.</param>
        public void DrawLine(Point a, Point b, BufferColor color)
        {
            DrawLine(a.X, a.Y, b.X, b.Y, color);
        }

        /// <summary>
        /// Draws a line to the buffer with the specified brush.
        /// </summary>
        /// <param name="a">The starting point of the line.</param>
        /// <param name="b">The ending point of the line.</param>
        /// <param name="brush">The brush to draw the line with.</param>
        public void DrawLine(Point a, Point b, BufferBrush brush)
        {
            DrawLine(a.X, a.Y, b.X, b.Y, brush);
        }

        /// <summary>
        /// Draws a colored line to the buffer.
        /// </summary>
        /// <param name="a">The starting point of the line.</param>
        /// <param name="b">The ending point of the line.</param>
        /// <param name="color">The color of the line.</param>
        public void DrawLine(ref Point a, ref Point b, BufferColor color)
        {
            DrawLine(a.X, a.Y, b.X, b.Y, color);
        }

        /// <summary>
        /// Draws a line to the buffer with the specified brush.
        /// </summary>
        /// <param name="a">The starting point of the line.</param>
        /// <param name="b">The ending point of the line.</param>
        /// <param name="brush">The brush to draw the line with.</param>
        public void DrawLine(ref Point a, ref Point b, BufferBrush brush)
        {
            DrawLine(a.X, a.Y, b.X, b.Y, brush);
        }

        #endregion

        #region Triangles
        /// <summary>
        /// Draws a triangle to the buffer.
        /// </summary>
        /// <param name="triangle">The triangle to draw.</param>
        /// <param name="color">The color to draw the triangle.</param>
        public void DrawTriangle(Triangle triangle, BufferColor color)
        {
            DrawLine(ref triangle.A, ref triangle.B, color);
            DrawLine(ref triangle.B, ref triangle.C, color);
            DrawLine(ref triangle.C, ref triangle.A, color);
        }

        /// <summary>
        /// Draws a triangle to the buffer.
        /// </summary>
        /// <param name="triangle">The triangle to draw.</param>
        /// <param name="color">The color to draw the triangle.</param>
        public void DrawTriangle(ref Triangle triangle, BufferColor color)
        {
            DrawLine(ref triangle.A, ref triangle.B, color);
            DrawLine(ref triangle.B, ref triangle.C, color);
            DrawLine(ref triangle.C, ref triangle.A, color);
        }

        /// <summary>
        /// Draws a triangle to the buffer.
        /// </summary>
        /// <param name="a">The first point of the triangle.</param>
        /// <param name="b">The second point of the triangle.</param>
        /// <param name="c">The third point of the triangle.</param>
        /// <param name="color">The color of the line.</param>
        public void DrawTriangle(Point a, Point b, Point c, BufferColor color)
        {
            DrawLine(ref a, ref b, color);
            DrawLine(ref b, ref c, color);
            DrawLine(ref c, ref a, color);
        }

        /// <summary>
        /// Draws a triangle to the buffer.
        /// </summary>
        /// <param name="a">The first point of the triangle.</param>
        /// <param name="b">The second point of the triangle.</param>
        /// <param name="c">The third point of the triangle.</param>
        /// <param name="brush">The brush of the line.</param>
        public void DrawTriangle(Point a, Point b, Point c, BufferBrush brush)
        {
            DrawLine(ref a, ref b, brush);
            DrawLine(ref b, ref c, brush);
            DrawLine(ref c, ref a, brush);
        }

        /// <summary>
        /// Draws a triangle to the buffer.
        /// </summary>
        /// <param name="a">The first point of the triangle.</param>
        /// <param name="b">The second point of the triangle.</param>
        /// <param name="c">The third point of the triangle.</param>
        /// <param name="color">The color of the line.</param>
        public void DrawTriangle(ref Point a, ref Point b, ref Point c, BufferColor color)
        {
            DrawLine(ref a, ref b, color);
            DrawLine(ref b, ref c, color);
            DrawLine(ref c, ref a, color);
        }

        /// <summary>
        /// Draws a triangle to the buffer.
        /// </summary>
        /// <param name="a">The first point of the triangle.</param>
        /// <param name="b">The second point of the triangle.</param>
        /// <param name="c">The third point of the triangle.</param>
        /// <param name="brush">The brush of the line.</param>
        public void DrawTriangle(ref Point a, ref Point b, ref Point c, BufferBrush brush)
        {
            DrawLine(ref a, ref b, brush);
            DrawLine(ref b, ref c, brush);
            DrawLine(ref c, ref a, brush);
        }

        /// <summary>
        /// Draws a triangle with the specified border and fill brushes.
        /// </summary>
        /// <param name="triangle">The triangle to draw.</param>
        /// <param name="border">The border brush.</param>
        /// <param name="fill">The fill brush.</param>
        public void DrawTriangle(Triangle triangle, BufferBrush border, BufferBrush fill)
        {            
            var bounds = triangle.GetBounds();
            double area = triangle.Area;
            Point p = new Point();
            double sum = 0;
            for (int i = bounds.Left; i < bounds.Right; i++)
            for (int j = bounds.Top; j < bounds.Bottom; j++)
            {
                p.X = i;
                p.Y = j;
                sum = Triangle.GetArea(p, triangle.A, triangle.B) +
                    Triangle.GetArea(p, triangle.B, triangle.C) +
                    Triangle.GetArea(p, triangle.C, triangle.A);
                if (sum >= area - 1 && sum <= area + 1)
                {
                    _buffer[j, i].BackColor = fill.GetColor(i, j);
                }
            }
            DrawTriangle(triangle, border);
        }

        /// <summary>
        /// Draws a triangle to the buffer.
        /// </summary>
        /// <param name="triangle">The triangle to draw.</param>
        /// <param name="brush">The brush to draw the triangle with.</param>
        public void DrawTriangle(Triangle triangle, BufferBrush brush)
        {
            DrawLine(ref triangle.A, ref triangle.B, brush);
            DrawLine(ref triangle.B, ref triangle.C, brush);
            DrawLine(ref triangle.C, ref triangle.A, brush);
        }

        #endregion

        #region Buffers

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
            int offx = 0;
            int offy = 0;
            int w = buffer._width;
            int h = buffer._height;
            if (x + w < 0 || y + h == 0 || y >= _height || x >= _width)
            {
                return;
            }
            if (w + x > _width)
            {
                w -= w + x - _width;
            }
            if (x < 0)
            {
                offx = -x;
            }
            if (h + y > _height)
            {
                h -= h + y - _height;
            }
            if (y < 0)
            {
                offy = -y;
            }

            switch (drawMode)
            {
                case BufferDrawMode.Additive:
                    {
                        for (int i = w - 1; i >= offx; i--)
                        for (int j = h - 1; j >= offy; j--)
                        {
                            b[j + y + offy, i + x + offx]._attrs |= b2[j, i]._attrs;
                            b[j + y + offy, i + x + offx].CharData = b2[j, i].CharData;
                        }     
                    }
                    break;
                case BufferDrawMode.DrawOver:
                    {
                        for (int i = w - 1; i >= offx; i--)
                        for (int j = h - 1; j >= offy; j--)
                        {
                            b[j + y + offy, i + x + offx]._attrs = b2[j, i]._attrs;
                            b[j + y + offy, i + x + offx].CharData = b2[j, i].CharData;
                        }
                    }
                    break;
                case BufferDrawMode.IgnoreBlack:
                    {
                        for (int i = w - 1; i >= offx; i--)
                        for (int j = h - 1; j >= offy; j--)
                        {
                            if (b2[j, i].BackColor != BufferColor.Black)
                            {
                                b[j + y + offy, i + x + offx]._attrs = b2[j, i]._attrs;
                            }
                            b[j + y + offy, i + x + offx].CharData = b2[j, i].CharData;
                        }
                    }
                    break;
            }
        }

        #endregion

        #endregion

        #region Non-public methods

        protected bool InBounds(int x, int y)
        {
            return x >= 0 && x < _width && y >= 0 && y < _height;
        }

        protected bool InBounds(ref int x, ref int y)
        {
            return x >= 0 && x < _width && y >= 0 && y < _height;
        }

        protected bool InBounds(Point point)
        {
            return point.X >= 0 && point.X < _width && point.Y >= 0 && point.Y < _height;
        }

        protected bool InBounds(ref Point point)
        {
            return point.X >= 0 && point.X < _width && point.Y >= 0 && point.Y < _height;
        }

        #endregion
    }
}
