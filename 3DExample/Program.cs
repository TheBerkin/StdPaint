using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StdPaint;
using System.Windows.Forms;

namespace _3DExample
{
    class Program
    {
        static Renderer renderer;
        static Scene scene;
        static Camera camera;
        static Mesh mesh;
        static Plane plane;
        static double rotation = 0;

        static void Main(string[] args)
        {
            Console.Title = "3D Example";

            Console.WriteLine("This program works best with a small, square font. Please change your settings, then press a key...");
            Console.ReadKey();

            Painter.Starting += Painter_Starting;
            Painter.Paint += Painter_Paint;

            Painter.Run(64, 64, 30);
        }

        static void Painter_Starting(object sender, EventArgs e)
        {
            renderer = new Renderer();
            scene = new Scene();
            plane = new Plane(BufferColor.Green);
            mesh = Mesh.FromOBJ("cube.obj");
            camera = new Camera(75, 0.1, 1000);
            scene.ActiveCamera = camera;
            renderer.ActiveScene = scene;
            scene.Objects.Add(mesh);
        }

        static void Painter_Paint(object sender, EventArgs e)
        {
            rotation -= 0.05;
            Painter.BackBuffer.Clear();
            mesh.ModelMatrix = (Quaternion.Rotation(Vector3.Up, rotation) * Quaternion.Rotation(Vector3.Left, rotation)).Matrix;
            renderer.Render(Painter.BackBuffer);
        }

        static void Painter_MouseMove(object sender, PainterMouseEventArgs e)
        {
            rotation = (-(double)e.UnitLocation.X / Painter.BackBuffer.Width);
        }
    }
}
