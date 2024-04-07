using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    public static partial class Analyzer
    {
        /// <summary>
        /// Identifies nodal instabilities in a model's stiffness matrix.
        /// </summary>
        public static void check_stability(FEModel3D model, Matrix K)
        {
            // Initialize the `unstable` flag to `False`
            bool unstable = false;
            string ErrorMessage = "";

            // Step through each diagonal term in the stiffness matrix
            for (int i = 0; i < K.RowCount; i++)
            {
                // Determine which node this term belongs to
                Node3D node = model.Nodes.Values.FirstOrDefault(n => n.ID == i / 6);
                // Determine which degree of freedom this term belongs to
                int dof = i % 6;
                // Check to see if this degree of freedom is supported
                bool supported = false;
                if (dof == 0)
                    supported = node.support_DX;
                else if (dof == 1)
                    supported = node.support_DY;
                else if (dof == 2)
                    supported = node.support_DZ;
                else if (dof == 3)
                    supported = node.support_RX;
                else if (dof == 4)
                    supported = node.support_RY;
                else if (dof == 5)
                    supported = node.support_RZ;

                // Check if the degree of freedom on this diagonal is unstable
                if (Num.IsEqual(K[i,i], 0) && !supported)
                {
                    unstable = true;
                    // Identify which direction this instability effects
                    string direction = "";
                    if (dof == 0)
                        direction = "for translation in the global X direction.";
                    else if (dof == 1)
                        direction = "for translation in the global Y direction.";
                    else if (dof == 2)
                        direction = "for translation in the global Z direction.";
                    else if (dof == 3)
                        direction = "for rotation about the global X axis.";
                    else if (dof == 4)
                        direction = "for rotation about the global Y axis.";
                    else if (dof == 5)
                        direction = "for rotation about the global Z axis.";

                    ErrorMessage += $"* Nodal instability detected: node {node.Name} is unstable {direction}\n";
                }
            }
            if (unstable)
            {
                throw new Exception(ErrorMessage);
            }
        }
        /// <summary>
        /// Checks static equilibrium and prints results to the console.
        /// </summary>
        public static void check_statics(FEModel3D model, List<string> Combo_tags = null)
        {

        }
    }
}
