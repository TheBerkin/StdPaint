using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Painter.Run(136, 100, 30);
        }

        static void Painter_RightButtonDown(object sender, PainterMouseEventArgs e)
        {
            paintBuffer.Clear();
        }

        static Point pLast = new Point(-1, -1);

        static readonly BufferColor[] pallette = {
                                                  BufferColor.Blue,
                                                  BufferColor.Green,
                                                  BufferColor.Red,
                                                  BufferColor.Magenta,
                                                  BufferColor.Cyan,
                                                  BufferColor.Yellow,
                                                  BufferColor.White
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

            if (!penOn && e.UnitLocation.X < 5 && e.UnitLocation.Y < pallette.Length * 5)
            {
                pen = e.UnitLocation.Y / 5;
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
                Painter.ActiveBuffer.DrawBox(0, i * 5, 5, 5, pallette[i]);
            }

            // Draw the extension showing the active color
            Painter.ActiveBuffer.DrawBox(5, pen * 5, 2, 5, pallette[pen]);
        }
    }
}
