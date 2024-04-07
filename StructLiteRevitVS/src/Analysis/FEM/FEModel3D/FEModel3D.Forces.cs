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
        /// Assembles and returns the global nodal force vector.
        /// </summary>
        ///   <param name="combo_name">The name of the load combination to get the force vector for. Defaults to 'Combo 1'.</param>
        public Vector P(string combo_name = "Combo 1")
        {
            // Initialize a zero vector to hold all the terms
            Vector P = new Vector(this.Nodes.Count * 6);
            // Get the load combination for the given 'combo_name'
            var combo = this.LoadCombos[combo_name];

            // Add terms for each node in the model
            foreach (var node in Nodes.Values)
            {
                // Get the node's ID
                var ID = (int)node.ID;

                // Step through each load factor in the load combination
                foreach (var kvp in combo.Factors)
                {
                    string caseName = kvp.Key;
                    double factor = kvp.Value;

                    // Add the node's loads to the global nodal load vector
                    foreach (var load in node.NodeLoads)
                    {
                        if (load.CaseName == caseName)
                        {
                            if (load.direction == Direction.FX)
                                P[ID * 6 + 0] += factor * load.Value;
                            else if (load.direction == Direction.FY)
                                P[ID * 6 + 1] += factor * load.Value;
                            else if (load.direction == Direction.FZ)
                                P[ID * 6 + 2] += factor * load.Value;
                            else if (load.direction == Direction.MX)
                                P[ID * 6 + 3] += factor * load.Value;
                            else if (load.direction == Direction.MY)
                                P[ID * 6 + 4] += factor * load.Value;
                            else if (load.direction == Direction.MZ)
                                P[ID * 6 + 5] += factor * load.Value;
                        }
                    }
                }
            }
            return P;
        }
        /// <summary>
        /// Assembles and returns the global fixed end reaction vector for any given load combo.
        /// </summary>
        ///   <param name="combo_name">The name of the load combination to get the fixed end reaction vector for. Defaults to 'Combo 1'.</param>
        public Vector FER(string combo_name = "Combo 1")
        {
            // Initialize a zero vector to hold all the terms
            Vector FER = new Vector(this.Nodes.Count * 6);

            // Step through each sub-member and add terms
            foreach (var member in Members.Values)
            {
                // Get the member's global fixed end reaction vector
                // Storing it as a local variable eliminates the need to rebuild it every time a term is needed
                var member_FER = member.FER(combo_name);

                // Step through each term in the member's fixed end reaction vector
                // 'a' below is the row index in the member's fixed end reaction vector
                // 'm' below is the corresponding row index in the global fixed end reaction vector
                for (int a = 0; a < 12; a++)
                {
                    int m;
                    // Determine if index 'a' is related to the i-node or j-node
                    // Find the corresponding index 'm' in the global fixed end reaction vector
                    if (a < 6)
                        m = (int)member.i_node.ID * 6 + a;
                    else
                        m = (int)member.j_node.ID * 6 + (a-6);

                    // Now that 'm' is known, place the term in the global fixed end reaction vector
                    FER[m] += member_FER[a];
                }
            }
            return FER;
        }
    }
}
