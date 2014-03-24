using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using StdPaint;

namespace DrawingExample
{
    class Program
    {
        static ConsoleBuffer paintBuffer;

        static void Main(string[] args)
        {
            Console.Title = "StdPaint Drawing Example";

            Console.WriteLine("This program works best with a small, square font. Please change your settings, then press a key...");
            Console.ReadKey();

            Painter.Paint += Painter_Paint;
            Painter.Starting += Painter_Starting;
            Painter.MouseMove += Painter_MouseMove;
            Painter.LeftButtonDown += Painter_LeftButtonDown;
            Painter.LeftButtonUp += Painter_LeftButtonUp;
            Painter.RightButtonDown += Painter_RightButtonDown;

            Painter.Run(136, 100, 60);
        }

        static void Painter_RightButtonDown(object sender, PainterMouseEventArgs e)
        {
            paintBuffer.FloodFill(e.UnitLocation.X, e.UnitLocation.Y, new CheckeredBufferBrush(pallette[pen], BufferColor.Gray));
        }

        static Point pLast = new Point(-1, -1);

        static readonly BufferColor[] pallette = {
                                                  BufferColor.Blue,
                                                  BufferColor.Green,
                                                  BufferColor.Red,
                                                  BufferColor.Magenta,
                                                  BufferColor.Cyan,
                                                  BufferColor.Yellow,
                                                  BufferColor.DarkGray,
                                                  BufferColor.White,
                                                  BufferColor.DarkBlue,
                                                  BufferColor.DarkGreen,
                                                  BufferColor.DarkRed,
                                                  BufferColor.DarkMagenta,
                                                  BufferColor.DarkCyan,
                                                  BufferColor.DarkYellow,
                                                  BufferColor.Black,
                                                  BufferColor.Gray
                                              };

        static int pen = 3;

        static bool penOn = false;

        static void Painter_LeftButtonUp(object sender, PainterMouseEventArgs e)
        {
            penOn = false;
        }

        static void Painter_LeftButtonDown(object sender, PainterMouseEventArgs e)
        {
            pLast = e.UnitLocation;

            if (!penOn && e.UnitLocation.X < 10 && e.UnitLocation.Y < pallette.Length * 5)
            {
                pen = e.UnitLocation.Y / 5 + (e.UnitLocation.X / 5) * 8;
            }

            penOn = true;
            paintBuffer.SetUnitBackColor(e.UnitLocation.X, e.UnitLocation.Y, pallette[pen]);
        }

        static void Painter_MouseMove(object sender, PainterMouseEventArgs e)
        {            
            if (penOn)
            {
                paintBuffer.DrawLine(pLast.X, pLast.Y, e.UnitLocation.X, e.UnitLocation.Y, pallette[pen]);
            }            
            pLast = e.UnitLocation;
            
        }

        static void Painter_Starting(object sender, EventArgs e)
        {
            paintBuffer = ConsoleBuffer.CreateScreenBuffer();
        }

        static void Painter_Paint(object sender, EventArgs e)
        {            
            // A Clear() call is not needed since the paint buffer covers the whole screen.

            // Draw the canvas
            Painter.ActiveBuffer.DrawBuffer(paintBuffer, 0, 0, BufferDrawMode.DrawOver);
                        
            // Draw the color pallette
            for (int i = 0; i < pallette.Length; i++)
            {
                Painter.ActiveBuffer.DrawBox(5 * (i / 8), (i % 8) * 5, 5, 5, pallette[i]);
            }

            // Draw the extension showing the active color
            Painter.ActiveBuffer.DrawBox(5 * (pen / 8) + 5, (pen % 8) * 5, 2, 5, pallette[pen]);
        }
    }
}
