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


        const BufferUnitAttributes white =
            BufferUnitAttributes.BackgroundRed |
            BufferUnitAttributes.BackgroundGreen |
            BufferUnitAttributes.BackgroundBlue |
            BufferUnitAttributes.BackgroundIntensity;

        static readonly BufferUnitAttributes[] rgbw = {
                                                  BufferUnitAttributes.BackgroundBlue | BufferUnitAttributes.BackgroundIntensity,
                                                  BufferUnitAttributes.BackgroundGreen | BufferUnitAttributes.BackgroundIntensity,
                                                  BufferUnitAttributes.BackgroundRed | BufferUnitAttributes.BackgroundIntensity,
                                                  BufferUnitAttributes.BackgroundRed |
                                                  BufferUnitAttributes.BackgroundBlue |
                                                  BufferUnitAttributes.BackgroundIntensity,
                                                  white
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

            if (!penOn && e.UnitLocation.X < 5 && e.UnitLocation.Y < rgbw.Length * 5)
            {
                pen = e.UnitLocation.Y / 5;
            }

            penOn = true;
            paintBuffer.SetUnitAttributes(e.UnitLocation.X, e.UnitLocation.Y, rgbw[pen]);
        }

        static void Painter_MouseMove(object sender, PainterMouseEventArgs e)
        {
            if (penOn)
            {
                paintBuffer.DrawLine(pLast.X, pLast.Y, e.UnitLocation.X, e.UnitLocation.Y, rgbw[pen]);
            }
            pLast = e.UnitLocation;
        }

        static void Painter_Starting(object sender, EventArgs e)
        {
            paintBuffer = ConsoleBuffer.CreateScreenBuffer();
        }

        static void Painter_Paint(object sender, EventArgs e)
        {
            Painter.Clear();

            for (int i = 0; i < rgbw.Length; i++)
            {
                Painter.ActiveBuffer.DrawBox(0, i * 5, 5, 5, rgbw[i]);
            }

            Painter.ActiveBuffer.DrawBox(5, pen * 5, 2, 5, rgbw[pen]);

            Painter.ActiveBuffer.DrawBuffer(paintBuffer, 0, 0);
        }
    }
}
