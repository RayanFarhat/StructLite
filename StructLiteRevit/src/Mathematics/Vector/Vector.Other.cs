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
        /// Rounds vector entries to the nearest integeral value.
        /// </summary>
        public virtual Vector Round(int decimals)
        {
            return new Vector(VectorFunctions.Round(this.InnerArray, decimals));
        }
    }
}
