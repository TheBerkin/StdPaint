using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// Represents a view to render a scene from.
    /// </summary>
    public class Camera
    {
        private Matrix4 projectionMatrix;
        private Matrix4 viewMatrix;
        private int fov;
        private double near;
        private double far;
        private int oldWidth, oldHeight;
        private Vector4 position;

        /// <summary>
        /// Create a new Camera object.
        /// </summary>
        /// <param name="fov">The angle of the Field of View of the camera.</param>
        /// <param name="near">The distance from the Camera to the near clipping plane.</param>
        /// <param name="far">The distance from the Camera to the far clipping plane.</param>
        public Camera(int fov, double near, double far)
        {
            projectionMatrix = Matrix4.Identity;
            viewMatrix = Matrix4.Identity;
            position = new Vector4(0, 0, 0, 1);
            this.near = near;
            this.far = far;
            this.fov = fov;
        }

        /// <summary>
        /// Project a Vector4 onto a screen.
        /// </summary>
        /// <param name="width">The width of the buffer that is being rendered to.</param>
        /// <param name="height">The height of the buffer that is being rendered to.</param>
        /// <param name="vector">The Vector4 to project.</param>
        /// <param name="model">The model matrix of the object that is being rendered.</param>
        /// <returns>The projected vector.</returns>
        public Vector2 ProjectVector(int width, int height, Vector4 vector, Matrix4 model = null)
        {
            // different size, update projection matrix
            /*if (width != oldWidth || height != oldHeight)
            {
                oldWidth = width;
                oldHeight = height;
                Matrix4 projection = Matrix4.Identity;
                Matrix4 scale = Matrix4.Identity;
                Matrix4 rotation = Matrix4.Identity;
                Matrix4 translate = Matrix4.Identity;
                double k = near / far;
                projection[10] = 1 / (k - 1);
                projection[11] = k / (k - 1);
                projection[14] = -1;
                projection[15] = 0;
                scale[0] = (height / width) / far;
                scale[5] = 1 / far;
                scale[10] = 1 / far;
                Vector4 n = position * (-1 / position.Magnitude);
                //Vector4 upN = (new Vector4(0, 1, 0, 1) * n);
                Vector4 r = upN * (1 / upN.Magnitude);
                rotation[8] = n.X;
                rotation[9] = n.Y;
                rotation[10] = n.Z;
            }*/
            Vector4 cartesian = vector * (model * (projectionMatrix * viewMatrix));
            return new Vector2(width * ((cartesian.X / cartesian.W + 1) / 2), height * ((cartesian.Y / cartesian.W + 1) / 2));
        }
    }
}
