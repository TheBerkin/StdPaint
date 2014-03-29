using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// A 4x4 matrix for use in 3D.
    /// </summary>
    public class Matrix4
    {
        private double[] items;

        /// <summary>
        /// Create a new Matrix4 object.
        /// </summary>
        public Matrix4()
        {
            items = new double[16];
            for (int i = 0; i < 16; i++)
                items[i] = 0;
        }

        public double this[int i]
        {
            get { return items[i]; }
            set { items[i] = value; }
        }

        public double this[int i, int j]
        {
            get { return items[i * 4 + j]; }
            set { items[i * 4 + j] = value; }
        }

        public static Matrix4 operator *(Matrix4 a, Matrix4 b)
        {
            Matrix4 result = new Matrix4();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    for (int k = 0; k < 4; k++)
                        result[i, j] = result[i, j] + a[i, k] * b[k, j];
            return result;
        }

        /// <summary>
        /// Returns an identity matrix
        /// </summary>
        public static Matrix4 Identity
        {
            get
            {
                Matrix4 matrix = new Matrix4();
                matrix[0] = matrix[5] = matrix[10] = matrix[15] = 1;
                return matrix;
            }
        }

        public static Matrix4 CreateRotationX(double radians)
        {
            Matrix4 matrix = Matrix4.Identity;
            matrix[5] = Math.Cos(radians);
            matrix[6] = -(Math.Sin(radians));
            matrix[9] = Math.Sin(radians);
            matrix[10] = Math.Cos(radians);
            return matrix;
        }

        public static Matrix4 CreateRotationY(double radians)
        {
            Matrix4 matrix = Matrix4.Identity;
            matrix[0] = Math.Cos(radians);
            matrix[2] = Math.Sin(radians);
            matrix[8] = -Math.Sin(radians);
            matrix[10] = Math.Cos(radians);
            return matrix;
        }

        public static Matrix4 CreateRotationZ(double radians)
        {
            Matrix4 matrix = Matrix4.Identity;
            matrix[0] = Math.Cos(radians);
            matrix[1] = Math.Sin(radians);
            matrix[4] = -(Math.Sin(radians));
            matrix[5] = Math.Cos(radians);
            return matrix;
        }
    }
}
