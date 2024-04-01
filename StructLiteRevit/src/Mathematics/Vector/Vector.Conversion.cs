using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Mathematics
{
    public partial class Vector
    {
        /// <summary>
        /// Converts vector to matrix.
        /// </summary>
        public virtual Matrix AsMatrix()
        {
            return new Matrix(VectorFunctions.ToMatrix(this.InnerArray));
        }
    }
}
