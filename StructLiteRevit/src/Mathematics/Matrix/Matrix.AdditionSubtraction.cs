using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Mathematics
{
    public partial class Matrix
    {
        /// <summary>
        /// Adds another matrix (right).
        /// </summary>
        public virtual Matrix Add(double[,] matrix)
        {
            return new Matrix(MatrixFunctions.Add(this.InnerMatrix, matrix));
        }

        /// <summary>
        /// Adds another matrix (right).
        /// </summary>
        public virtual Matrix Add(Matrix matrix)
        {
            return Add(matrix.InnerMatrix);
        }

        /// <summary>
        /// Subtracts another matrix (right).
        /// </summary>
        public virtual Matrix Subtract(double[,] matrix)
        {
            return new Matrix(MatrixFunctions.Subtract(this.InnerMatrix, matrix));
        }

        /// <summary>
        /// Subtracts another matrix (right).
        /// </summary>
        public virtual Matrix Subtract(Matrix matrix)
        {
            return Subtract(matrix.InnerMatrix);
        }
    }
}
