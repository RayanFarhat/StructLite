using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StructLite.Analysis
{
    public partial class Member3D
    {
        public static int count = 0;
        /// <summary>
        ///   Returns the member's local displacement vector.
        /// </summary>
        public Vector d(string combo_name = "Combo 1")
        {
            count++;
            var mat = T().Multiply(D(combo_name));
            return Vector.FromMatrix(mat);
        }
        /// <summary>
        ///  Returns the member's global displacement vector.
        /// </summary>
        public Vector D(string combo_name = "Combo 1")
        {
            var D = new Vector(12);
            D[0] = this.i_node.DX[combo_name];
            D[1] = this.i_node.DY[combo_name];
            D[2] = this.i_node.DZ[combo_name];
            D[3] = this.i_node.RX[combo_name];
            D[4] = this.i_node.RY[combo_name];
            D[5] = this.i_node.RZ[combo_name];
            D[6] = this.j_node.DX[combo_name];
            D[7] = this.j_node.DY[combo_name];
            D[8] = this.j_node.DZ[combo_name];
            D[9] = this.j_node.RX[combo_name];
            D[10] = this.j_node.RY[combo_name];
            D[11] = this.j_node.RZ[combo_name];
            return D;
        }
    }
}
