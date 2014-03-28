using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace StdPaint
{
    /// <summary>
    /// Defines an ASCII font using a matrix of boolean values.
    /// </summary>
    public partial class RasterFont
    {
        private readonly bool[][][] glyphs;
        private readonly int _width, _height;

        /// <summary>
        /// Gets the glyph set for this font.
        /// </summary>
        public bool[][][] Glyphs
        {
            get { return glyphs; }
        }

        /// <summary>
        /// Gets the width, in buffer units, of the glyphs in the font.
        /// </summary>
        public int CharWidth
        {
            get { return _width; }
        }

        /// <summary>
        /// Gets the height, in buffer units, of the glyphs in the font.
        /// </summary>
        public int CharHeight
        {
            get { return _height; }
        }

        /// <summary>
        /// Creates an empty raster font of the specified dimensions.
        /// </summary>
        /// <param name="cx">The glyph width to use.</param>
        /// <param name="cy">The glyph height to use.</param>
        public RasterFont(int cx, int cy)
        {
            glyphs = new bool[256][][];
            int l = cx * cy;
            _width = cx;
            _height = cy;
        }

        /// <summary>
        /// Converts a font definition string to a raster font.
        /// </summary>
        /// <param name="source">The string to get font data from.</param>
        /// <returns></returns>
        public static RasterFont FromString(string source)
        {
            try
            {
                StringReader reader = new StringReader(source);
                string head = reader.ReadLine().Trim();
                var m = Regex.Match(head, @"(?<cx>\d+)x(?<cy>\d+)");
                RasterFont font = null;
                if (!m.Success)
                {
                    return null;
                }
                else
                {
                    font = new RasterFont(Convert.ToInt32(m.Groups["cx"].Value), Convert.ToInt32(m.Groups["cy"].Value));
                }

                for (int i = 0; i < 256 && reader.Peek() != -1; i++)
                {
                    font.glyphs[i] = new bool[font.CharWidth][];
                    for (int j = 0; j < font.CharHeight; j++)
                    {
                        string line = reader.ReadLine();                        
                        font.glyphs[i][j] = line.Trim().ToCharArray().Select<char, bool>(c => c == '+').ToArray();
                    }
                }

                return font;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Draws text to a buffer using the specified settings.
        /// </summary>
        /// <param name="buffer">The buffer to draw to.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position of the text.</param>
        /// <param name="brush">The brush to draw the text with.</param>
        /// <param name="alignment">The alignment of the text relative to its position.</param>
        public void Draw(ConsoleBuffer buffer, string text, Point position, BufferBrush brush, Alignment alignment)
        {
            int // Character offsets
                mx = 0,
                my = 0;

            Point p = new Point();
            Point pc = new Point();

            foreach(char c in text)
            {
                p.X = alignment == Alignment.Left ? mx : -CharWidth - mx;
                p.Y = my;
                p = p + position;
                if (c == '\n')
                {
                    mx = 0;
                    my += CharHeight + 1;
                }
                else if (!Char.IsControl(c))
                {
                    for (int i = 0; i < glyphs[c].Length; i++)
                    {
                        pc.Y = i;
                        for(int j = 0; j < glyphs[c][i].Length; j++)
                        {
                            if (!glyphs[c][i][j]) continue;
                            pc.X = j;
                            buffer.SetUnitBackColor(p + pc, brush.GetColor(p.X + pc.X, p.Y + pc.Y));
                        }
                    }
                    mx += CharWidth + 1;
                }
            }
        }
    }
}
