using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Mathematics
{
    internal static partial class MatrixFunctions
    {
        // TODO: Test
        /// <summary>
        /// Creates projection matrix for the specified subspace.
        /// </summary>
        public static double[,] CreateProjectionMatrix(double[,] subspace)
        {
            var subspaceTranspose = MatrixFunctions.Transpose(subspace);

            double[,] value = MatrixFunctions.Multiply(subspaceTranspose, subspace);

            value = MatrixFunctions.Invert(value);

            value = MatrixFunctions.Multiply(value, subspaceTranspose);

            value = MatrixFunctions.Multiply(subspace, value);

            return value;
        }
    }
}
