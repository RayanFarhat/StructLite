using Autodesk.Revit.DB.Visual;
using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    public partial class FEModel3D
    {
        /// <summary>
        /// Returns the model's global stiffness matrix.
        /// </summary>
        ///   <param name="combo_name">The load combination to get the stiffness matrix for. Defaults to 'Combo 1'.</param>
        ///   <param name="check_stability">auses Pynite to check for instabilities if set to True. Defaults
        ///   to True.Set to False if you want the model to run faster.</param>
        public Matrix K(string combo_name = "Combo 1", bool check_stability = true)
        {
            //TODO: check if remove combo_name


            // Initialize a dense matrix of zeros
            var K = new Matrix(Nodes.Count * 6);

            // Add stiffness terms for each physical member in the model
            foreach (var member in Members.Values)
            {
                //  Get the member's global stiffness matrix
                // Storing it as a local variable eliminates the need to rebuild it every time a term is needed
                var member_K = member.K();

                // Step through each term in the member's stiffness matrix
                // 'a' & 'b' below are row/column indices in the member's stiffness matrix
                // 'm' & 'n' are corresponding row/column indices in the global stiffness matrix
                for (int a = 0; a < 12; a++)
                {
                    int m;
                    // Determine if index 'a' is related to the i-node or j-node
                    // Find the corresponding index 'm' in the global stiffness matrix
                    if (a < 6)
                        m = (int)member.i_node.ID * 6 + a;
                    else
                        m = (int)member.j_node.ID * 6 + (a - 6);

                    for (int b = 0; b < 12; b++)
                    {
                        int n;
                        // Determine if index 'b' is related to the i-node or j-node
                        // Find the corresponding index 'n' in the global stiffness matrix
                        if (b < 6)
                            n = (int)member.i_node.ID * 6 + b;
                        else
                            n = (int)member.j_node.ID * 6 + (b - 6);

                        // Now that 'm' and 'n' are known, place the term in the global stiffness matrix
                        K[m, n] += member_K[a, b];
                    }
                }
            }
            // Check that there are no nodal instabilities
            if (check_stability)
            {
                Analyzer.check_stability(this, K);
            }
            return K;
        }
    }
}
