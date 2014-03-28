using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    public static class ImageLoader
    {
        // the RGB values for each color in the BufferColor enum
        public static byte[,] Colors = new byte[16, 3]{
            { 0, 0, 0 }, // Black
            { 0, 0, 128 }, // DarkBlue
            { 0, 128, 0 }, // DarkGreen
            { 0, 128, 128 }, // DarkCyan
            { 128, 0, 0 }, // DarkRed
            { 128, 0, 128 }, // DarkMagenta
            { 128, 128, 0 }, // DarkYellow
            { 192, 192, 192 }, // Gray
            { 128, 128, 128 }, // DarkGray
            { 0, 0, 255 }, // Blue
            { 0, 255, 0 }, // Green
            { 0, 255, 255 }, // Cyan
            { 255, 0, 0 }, // Red
            { 255, 0, 255 }, // Magenta
            { 255, 255, 0 }, // Yellow
            { 255, 255, 255 }, // White
        };

        public static ConsoleBuffer FromFile(string file)
        {
            return createBuffer((Bitmap)Image.FromFile(file));
        }

        public static ConsoleBuffer FromStream(Stream file)
        {
            return createBuffer((Bitmap)Image.FromStream(file));
        }

        public static ConsoleBuffer FromBitmap(Bitmap bitmap)
        {
            return createBuffer(bitmap);
        }

        private static ConsoleBuffer createBuffer(Bitmap image)
        {
            ConsoleBuffer buffer = new ConsoleBuffer(image.Width, image.Height);
            for (int i = 0; i < image.Width; i++)
                for (int j = 0; j < image.Height; j++)
                {
                    Color c = image.GetPixel(i, j);
                    if (c.A == 0) continue;
                    double[,] distances = new double[16, 2];
                    double smallestDistance = -1;
                    int smallestKey = 0;
                    for (var k = 0; k < 16; k++)
                    {
                        // euclidean distance between colors
                        double distance = (double)Math.Sqrt(Math.Pow(c.R - Colors[k, 0], 2) + Math.Pow(c.G - Colors[k, 1], 2) + Math.Pow(c.B - Colors[k, 2], 2));
                        if (smallestDistance > distance || smallestDistance == -1)
                        {
                            smallestDistance = distance;
                            smallestKey = k;
                        }
                    }
                    buffer.SetUnitBackColor(i, j, (BufferColor)Enum.GetValues(typeof(BufferColor)).GetValue(smallestKey));
                }
            return buffer;
        }
    }
}
