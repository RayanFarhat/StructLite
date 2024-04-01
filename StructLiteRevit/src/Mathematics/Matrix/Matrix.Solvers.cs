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
        /// Solve a linear matrix equation.
        /// <para>example with numpy to understand</para> 
        ///  <para>a = np.array([[1, 2], [3, 5]])</para> 
        ///  <para>b = np.array([1, 2])</para> 
        ///  <para>its like x + 2 * y = 1 and 3 * x + 5 * y = 2:</para> 
        ///  <para>x = np.linalg.solve(a, b). result = [-1,1] that mean x=-1,y=1</para> 
        /// </summary>
        public Vector Solve(Vector constants)
        {
            return new Vector(MatrixFunctions.Solve(this.InnerMatrix, constants.InnerArray));
        }
    }
}
