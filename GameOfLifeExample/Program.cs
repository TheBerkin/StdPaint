using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StdPaint;

namespace GameOfLifeExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Conway's Game of Life in StdPaint";

            Console.WriteLine("This program works best with a small, square font. Please change your settings, then press a key...");
            Console.ReadKey();

            Painter.Starting += Painter_Starting;
            Painter.MouseMove += Painter_MouseMove;
            Painter.LeftButtonDown += Painter_LeftButtonDown;
            Painter.LeftButtonUp += Painter_LeftButtonUp;
            Painter.Paint += Painter_Paint;

            Painter.Run(128, 64, 30);
        }

        static void Painter_Paint(object sender, EventArgs e)
        {
            tickBuffer.Clear();
            var ab = Painter.ActiveBuffer;
            var b = ab.Buffer;
            var b2 = tickBuffer.Buffer;
            int w = ab.Width;
            int h = ab.Height;
            int n = 0;
            BufferUnitAttributes c = black;
            for(int i = 0; i < w; i++)
            for(int j = 0; j < h; j++)
            {
                n = CountNeighbors(ab, i, j);
                c = b[j, i].Attributes;
                if (n < 2 && c != black)
                {
                    b2[j, i].Attributes = black;
                }
                else if (n <= 3 && c != black)
                {
                    b2[j, i].Attributes = c & ~BufferUnitAttributes.BackgroundIntensity;
                }
                else if (n > 3 && c != black)
                {
                    b2[j, i].Attributes = black;
                }
                else if (n == 3 && c == black)
                {
                    b2[j, i].Attributes = PickColor();
                }
            }
            Array.Copy(b2, b, b.Length);
        }

        static int CountNeighbors(ConsoleBuffer buffer, int x, int y)
        {
            return (buffer.GetUnitAttributes(x - 1, y) != black ? 1 : 0) +
                   (buffer.GetUnitAttributes(x + 1, y) != black ? 1 : 0) +
                   (buffer.GetUnitAttributes(x - 1, y + 1) != black ? 1 : 0) +
                   (buffer.GetUnitAttributes(x + 1, y + 1) != black ? 1 : 0) +
                   (buffer.GetUnitAttributes(x - 1, y - 1) != black ? 1 : 0) +
                   (buffer.GetUnitAttributes(x + 1, y - 1) != black ? 1 : 0) +
                   (buffer.GetUnitAttributes(x, y + 1) != black ? 1 : 0) +
                   (buffer.GetUnitAttributes(x, y - 1) != black ? 1 : 0);
        }

        static Point prevPos;
        static bool penOn = false;
        static BufferUnitAttributes black = BufferUnitAttributes.None;
        static BufferUnitAttributes white = BufferUnitAttributes.BackgroundBlue |
                                            BufferUnitAttributes.BackgroundGreen |
                                            BufferUnitAttributes.BackgroundRed |
                                            BufferUnitAttributes.BackgroundIntensity;

        static BufferUnitAttributes[] colors = {
                                                   BufferUnitAttributes.BackgroundBlue | BufferUnitAttributes.BackgroundIntensity,
                                                   BufferUnitAttributes.BackgroundRed | BufferUnitAttributes.BackgroundIntensity,
                                                   BufferUnitAttributes.BackgroundGreen | BufferUnitAttributes.BackgroundIntensity,
                                                   BufferUnitAttributes.BackgroundGreen | BufferUnitAttributes.BackgroundRed | BufferUnitAttributes.BackgroundIntensity,
                                                   BufferUnitAttributes.BackgroundBlue | BufferUnitAttributes.BackgroundRed | BufferUnitAttributes.BackgroundIntensity,
                                                   BufferUnitAttributes.BackgroundGreen | BufferUnitAttributes.BackgroundBlue | BufferUnitAttributes.BackgroundIntensity
                                               };

        static ConsoleBuffer tickBuffer = null;

        static BufferUnitAttributes PickColor()
        {
            return colors[Environment.TickCount % colors.Length];
        }

        static void Painter_LeftButtonUp(object sender, PainterMouseEventArgs e)
        {
            penOn = false;
        }

        static void Painter_LeftButtonDown(object sender, PainterMouseEventArgs e)
        {
            penOn = true;
            Painter.ActiveBuffer.Buffer[e.UnitLocation.Y, e.UnitLocation.X].BackColor = BufferColor.White;
        }

        static void Painter_MouseMove(object sender, PainterMouseEventArgs e)
        {
            if (penOn)
            {
                Painter.ActiveBuffer.DrawLine(prevPos.X, prevPos.Y, e.UnitLocation.X, e.UnitLocation.Y, BufferColor.White);
            }
            prevPos = e.UnitLocation;
        }

        static void Painter_Starting(object sender, EventArgs e)
        {
            prevPos = new Point(-1, -1);
            int w = Painter.ActiveBufferWidth;
            int h = Painter.ActiveBufferHeight;
            var buffer = Painter.ActiveBufferData;
            tickBuffer = ConsoleBuffer.CreateScreenBuffer();
            Random rand = new Random();
            for(int i = 0; i < w; i++)
            for(int j = 0; j < h; j++)
            {
                if (rand.Next(0, 4) == 0)
                {
                    buffer[j, i].Attributes = white;
                }
            }
        }
    }
}
