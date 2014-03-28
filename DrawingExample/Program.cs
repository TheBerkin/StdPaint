using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using StdPaint;

namespace DrawingExample
{
    class Program
    {
        static ConsoleBuffer paintBuffer;

        [STAThread]
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
            Painter.KeyDown += Painter_KeyDown;

            Painter.Run(136, 100, 12);
        }

        static void Painter_KeyDown(object sender, PainterKeyEventArgs e)
        {
            if (Painter.IsKeyDown(Keys.LControlKey))
            {
                if (e.KeyCode == Keys.S)
                {
                    SaveFileDialog dialog = new SaveFileDialog()
                    {
                        Filter = "Console buffer files|*.cbuf",
                        Title = "Save Drawing Buffer"
                    };
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            paintBuffer.Save(dialog.FileName);
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("Error while saving:\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (e.KeyCode == Keys.O)
                {
                    OpenFileDialog dialog = new OpenFileDialog()
                    {
                        Filter = "Console buffer files|*.cbuf",
                        Title = "Open Drawing Buffer"
                    };
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            paintBuffer = ConsoleBuffer.FromFile(dialog.FileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error while opening:\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        static void Painter_RightButtonDown(object sender, PainterMouseEventArgs e)
        {
            paintBuffer.FloodFill(e.UnitLocation.X, e.UnitLocation.Y, pallette[pen]);
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

        static int pen = 0;
        static bool penOn = false;

        static void Painter_LeftButtonUp(object sender, PainterMouseEventArgs e)
        {
            penOn = false;
        }

        static void Painter_LeftButtonDown(object sender, PainterMouseEventArgs e)
        {
            pLast = e.UnitLocation;

            if (!penOn && e.UnitLocation.X < 10 && e.UnitLocation.Y < pallette.Length * 5 / 2)
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
            display = new SevenSegmentDisplay(new Point(Console.BufferWidth - 1, 1), 6, 0);
            display.ForeBrush = new HorizontalStripeBufferBrush(1, BufferColor.Green, BufferColor.DarkGreen);
            display.BackBrush = SolidBufferBrush.Black;

            fpsThread = new Thread(FPSUpdateThread);
            fpsThread.Start();
        }

        static SevenSegmentDisplay display;
        static Thread fpsThread;

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

            // Draw framerate             
            display.Draw(Painter.ActiveBuffer, Alignment.Right);
        }

        static void FPSUpdateThread()
        {
            while(Painter.Enabled)
            {
                display.Value = (int)Painter.CurrentFrameRate;
                Thread.Sleep(200);
            }
        }
    }
}
