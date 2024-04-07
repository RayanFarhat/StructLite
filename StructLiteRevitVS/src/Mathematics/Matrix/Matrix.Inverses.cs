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
        /// Calculates the inverse of matrix. Returns null if non-invertible.
        /// </summary>
        public virtual Matrix Invert()
        {
            var inverse = MatrixFunctions.Invert(this.InnerMatrix);
            if (inverse == null)
                return null;
            return new Matrix(inverse);
        }

        /// <summary>
        /// Returns a value indicates whether matrix is invertible. Internally uses matrix determinant.
        /// </summary>
        public virtual bool IsInvertible()
        {
            return MatrixFunctions.IsInvertible(this.InnerMatrix);
        }
        /// <summary>
        /// Check if matrix is singular in faast way
        public bool IsSingular()
        {
            var K = this.Multiply(this.Invert());
            for (global::System.Int32 i = 0; i < K.RowCount; i++)
            {
                for (global::System.Int32 j = 0; j < K.RowCount; j++)
                {
                    if (i == j && K[i, j] != 1)
                    {
                        return true;
                    }
                    else if (i != j && K[i, j] != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
