using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// Stores a console buffer capable of drawing three-dimensional objects.
    /// </summary>
    public class ConsoleBuffer3D : ConsoleBuffer
    {
        /*
         * Note: Please suffix 3D drawing methods with "3D". For example, a 3D DrawLine would be DrawLine3D.
         * This is so that primitive names can be differentiated in different dimensions.
         * - Berkin
         */

        #region Non-public fields

        double[,] _depthBuffer;
        DepthComparison _depthComp;

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the depth comparison type for the buffer.
        /// </summary>
        public DepthComparison DepthComparison
        {
            get { return _depthComp; }
            set { _depthComp = value; }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new ConsoleBuffer3D instance with the specified dimensions.
        /// </summary>
        /// <param name="width">The width of the buffer, in units.</param>
        /// <param name="height">The height of the buffer, in units.</param>
        public ConsoleBuffer3D(int width, int height)
            : base(width, height)
        {
            _depthBuffer = new double[height, width];
            _depthComp = DepthComparison.Less;
        }

        #endregion

        #region Non-public methods

        private bool CompareDepth(ref int x, ref int y, ref double depth)
        {
            if (!InBounds(ref x, ref y)) return false;

            switch(_depthComp)
            {
                case DepthComparison.Always:
                    return true;
                case DepthComparison.Never:
                    return false;
                case DepthComparison.Equal:
                    return depth == _depthBuffer[y, x];
                case DepthComparison.Greater:
                    return depth > _depthBuffer[y, x];
                case DepthComparison.GreaterEqual:
                    return depth >= _depthBuffer[y, x];
                case DepthComparison.Less:
                    return depth < _depthBuffer[y, x];
                case DepthComparison.LessEqual:
                    return depth <= _depthBuffer[y, x];
                case DepthComparison.NotEqual:
                    return depth != _depthBuffer[y, x];
                default:
                    return false;
            }
        }

        #endregion
    }
}
