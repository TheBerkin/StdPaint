using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StdPaint;

namespace ImageExample
{
    class Program
    {
        static ConsoleImage image;
        static int x, y;

        [STAThread]
        static void Main(string[] args)
        {
            Console.Title = "StdPaint Image Example";

            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "JPEG Image (*.jpg,*.jpeg)|*.jpg,*.jpeg|PNG Image (*.png)|*.png|GIF Image (*.gif)|*.gif|All files (*.*)|*.*";
            if (open.ShowDialog() != DialogResult.OK)
                return;

            Console.WriteLine("This program works best with a small, square font. Please change your settings, then press a key...");
            Console.ReadKey();

            image = new ConsoleImage(open.FileName);

            Painter.Starting += Painter_Starting;
            Painter.Paint += Painter_Paint;

            Painter.Run(64, 64, 30);
        }

        static void Painter_Starting(object sender, EventArgs e)
        {
            x = Painter.ActiveBufferWidth / 2 - image.Width / 2;
            y = Painter.ActiveBufferHeight / 2 - image.Height / 2;
        }

        static void Painter_Paint(object sender, EventArgs e)
        {
            image.Draw(x, y, Painter.BackBuffer);
        }
    }
}
