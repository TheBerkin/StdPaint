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
            if (width != oldWidth || height != oldHeight)
            {
                oldWidth = width;
                oldHeight = height;
                projectionMatrix = Matrix4.Identity;
                double aspectRatio = width / height;
                double zoomX = 1 / Math.Tan(fov / 2);
                double zoomY = zoomX * aspectRatio;
                projectionMatrix[0] = zoomX;
                projectionMatrix[5] = zoomY;
                projectionMatrix[10] = -(far + near) / (far - near);
                projectionMatrix[11] = (-2 * near * far) / (far - near);
                projectionMatrix[14] = -1;
                projectionMatrix[15] = 0;
            }
            Matrix4 matrix = projectionMatrix * viewMatrix;
            matrix *= model;
            Vector4 cartesian = vector * matrix;
            if (cartesian.W != 0)
            {
                cartesian.X /= cartesian.W;
                cartesian.Y /= cartesian.W;
                cartesian.Z /= cartesian.W;
            }
            else
                cartesian.W = 1;
            return new Vector2(cartesian.X / (2 * cartesian.W) + width / 2, cartesian.Y / (2 * cartesian.W) + height / 2);
        }

        /// <summary>
        /// Project a Vector3 onto a screen.
        /// </summary>
        /// <param name="width">The width of the buffer that is being rendered to.</param>
        /// <param name="height">The height of the buffer that is being rendered to.</param>
        /// <param name="vector">The Vector3 to project.</param>
        /// <param name="model">The model matrix of the object that is being rendered.</param>
        /// <returns>The projected vector.</returns>
        public Vector2 ProjectVector(int width, int height, Vector3 vector, Matrix4 model = null)
        {
            return this.ProjectVector(width, height, new Vector4(vector.X, vector.Y, vector.Z, 1), model);
        }
    }
}
