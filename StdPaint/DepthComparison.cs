using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdPaint
{
    /// <summary>
    /// Defines depth comparison types for the StdPaint.ConsoleBuffer3D class.
    /// </summary>
    public enum DepthComparison
    {
        /// <summary>
        /// Permits drawing when the new depth is less than the existing depth buffer value.
        /// </summary>
        Less,
        /// <summary>
        /// Permits drawing when the new depth is less than or equal to the existing depth buffer value.
        /// </summary>
        LessEqual,
        /// <summary>
        /// Permits drawing when the new depth is equal to the existing depth buffer value.
        /// </summary>
        Equal,
        /// <summary>
        /// Permits drawing when the new depth is not equal to the existing depth buffer value.
        /// </summary>
        NotEqual,
        /// <summary>
        /// Permits drawing when the new depth is greater than the existing depth buffer value.
        /// </summary>
        Greater,
        /// <summary>
        /// Permits drawing when the new depth is greater than or equal to the existing depth buffer value.
        /// </summary>
        GreaterEqual,
        /// <summary>
        /// Always permit drawing regardless of depth values.
        /// </summary>
        Always,
        /// <summary>
        /// Block all draw calls.
        /// </summary>
        Never
    }
}
