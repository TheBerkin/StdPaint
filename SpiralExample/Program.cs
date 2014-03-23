using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StdPaint;

namespace SpiralExample
{
    class Program
    {
        static BufferUnitAttributes[] colors = {
                                                   BufferUnitAttributes.BackgroundBlue | BufferUnitAttributes.BackgroundIntensity,
                                                   BufferUnitAttributes.BackgroundGreen | BufferUnitAttributes.BackgroundIntensity,
                                                   BufferUnitAttributes.BackgroundRed | BufferUnitAttributes.BackgroundIntensity
                                               };

        static long ticks = 0;

        const double SpiralWidth = 6;

        static Point center;

        static void Main(string[] args)
        {
            Console.Title = "StdPaint Spiral Example";

            Console.WriteLine("This program works best with a small, square font. Please change your settings, then press a key...");
            Console.ReadKey();

            Painter.Paint += Painter_Paint;
            Painter.Starting += Painter_Starting;
            Painter.MouseMove += Painter_MouseMove;
            Painter.LeftButtonDown += Painter_LeftButtonDown;
            Painter.LeftButtonUp += Painter_LeftButtonUp;

            Painter.Run(64, 64, 30);
        }

        static bool click = false;

        static void Painter_LeftButtonUp(object sender, PainterMouseEventArgs e)
        {
            click = false;
        }

        static void Painter_LeftButtonDown(object sender, PainterMouseEventArgs e)
        {
            click = true;
        }

        static void Painter_MouseMove(object sender, PainterMouseEventArgs e)
        {
            if (click)
            {
                center = e.UnitLocation;
            }
        }

        static void Painter_Starting(object sender, EventArgs e)
        {
            center = new Point(Painter.ActiveBufferWidth / 2, Painter.ActiveBufferHeight / 2);
        }

        static void Painter_Paint(object sender, EventArgs e)
        {
            var b = Painter.ActiveBufferData;
            int w = Painter.ActiveBufferWidth;
            int h = Painter.ActiveBufferHeight;

            var p = new Point();
            for (int i = 0; i < w; i++)
            for (int j = 0; j < h; j++)
            {
                p.X = i;
                p.Y = j;
                b[j,i].Attributes = colors[(int)((Dist(center, p) + ticks + SpiralOffset(center, p) / 4) / SpiralWidth % colors.Length)];
            }

            ticks++;
        }

        static double SpiralOffset(Point a, Point b)
        {
            return (Math.Atan2(b.Y - a.Y, b.X - a.X) + Math.PI) / Math.PI * 2 * SpiralWidth * colors.Length;
        }

        static double Dist(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }
    }
}
