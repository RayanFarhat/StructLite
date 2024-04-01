using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    /// <summary>
    /// A handler of analysis method in FEM
    /// </summary>
    public static partial class Analyzer
    {
        /// <summary>
        /// Stores calculated displacements from the solver into the model's displacement vector `_D` and into each node object in the model
        /// </summary>
        /// <param name="model">The finite element model being evaluated.</param>
        /// <param name="D1">An array of calculated displacements.</param>
        /// <param name="D2">An array of enforced displacements.</param>
        /// <param name="D1_indices">A list of the degree of freedom indices for each displacement in D1</param>
        /// <param name="D2_indices">A list of the degree of freedom indices for each displacement in D2</param>
        /// <param name="combo">The load combination to store the displacements for</param>
        public static void StoreDisplacements(FEModel3D model, Vector D1, Vector D2, List<int> D1_indices, List<int> D2_indices, LoadCombo combo)
        {
            // The raw results from the solver are partitioned. Unpartition them.
            var D = Unpartition_D(model, D1, D2, D1_indices, D2_indices);

            model._D[combo.Name] = D;
            // Store the calculated global nodal displacements into each node object
            foreach (var node in model.Nodes.Values)
            {
                var ID = (int)node.ID;
                node.DX[combo.Name] = D[ID * 6 + 0];
                node.DY[combo.Name] = D[ID * 6 + 1];
                node.DZ[combo.Name] = D[ID * 6 + 2];
                node.RX[combo.Name] = D[ID * 6 + 3];
                node.RY[combo.Name] = D[ID * 6 + 4];
                node.RZ[combo.Name] = D[ID * 6 + 5];
            }
        }
        /// <summary>
        /// Calculates reactions internally once the model is solved.
        /// </summary>
        /// <param name="model">The finite element model being evaluated.</param>
        /// <param name="Combo_tags">The finite element model being evaluated.</param>
        public static void calcReactions(FEModel3D model, List<string> Combo_tags = null)
        {
            var comboList = identify_combos(model, Combo_tags);

            // Calculate the reactions node by node
            foreach (var node in model.Nodes.Values)
            {
                // Step through each load combination
                foreach (var combo in comboList)
                {
                    // Initialize reactions for this node and load combination
                    node.RxnFX[combo.Name] = 0.0;
                    node.RxnFY[combo.Name] = 0.0;
                    node.RxnFZ[combo.Name] = 0.0;
                    node.RxnMX[combo.Name] = 0.0;
                    node.RxnMY[combo.Name] = 0.0;
                    node.RxnMZ[combo.Name] = 0.0;

                    //  Determine if the node has any supports
                    if (node.support_DX || node.support_DY || node.support_DZ ||
                        node.support_RX || node.support_RY || node.support_RZ)
                    {
                        // Step through each physical member in the model
                        foreach (var member in model.Members.Values)
                        {
                            if(member.i_node == node)
                            {
                                // Get the member's global force vector
                                var F = member.F(combo.Name);
                                node.RxnFX[combo.Name] += F[0];
                                node.RxnFY[combo.Name] += F[1];
                                node.RxnFZ[combo.Name] += F[2];
                                node.RxnMX[combo.Name] += F[3];
                                node.RxnMY[combo.Name] += F[4];
                                node.RxnMZ[combo.Name] += F[5];
                            }
                            else if (member.j_node == node)
                            {
                                // Get the member's global force vector
                                var F = member.F(combo.Name);
                                node.RxnFX[combo.Name] += F[6];
                                node.RxnFY[combo.Name] += F[7];
                                node.RxnFZ[combo.Name] += F[8];
                                node.RxnMX[combo.Name] += F[9];
                                node.RxnMY[combo.Name] += F[10];
                                node.RxnMZ[combo.Name] += F[11];
                            }
                        }
                        // Sum the joint loads applied to the node
                        foreach (var load in node.NodeLoads)
                        {
                            foreach (var kvp in combo.Factors)
                            {
                                var Case = kvp.Key;
                                var factor = kvp.Value;
                                if (load.CaseName == Case)
                                {
                                    if (load.direction == Direction.FX)
                                    {
                                        node.RxnFX[combo.Name] -= factor*load.Value;
                                    }
                                    else if(load.direction == Direction.FY)
                                    {
                                        node.RxnFY[combo.Name] -= factor * load.Value;
                                    }
                                    else if (load.direction == Direction.FZ)
                                    {
                                        node.RxnFZ[combo.Name] -= factor * load.Value;
                                    }
                                    else if (load.direction == Direction.MX)
                                    {
                                        node.RxnMX[combo.Name] -= factor * load.Value;
                                    }
                                    else if (load.direction == Direction.MY)
                                    {
                                        node.RxnMY[combo.Name] -= factor * load.Value;
                                    }
                                    else if (load.direction == Direction.MZ)
                                    {
                                        node.RxnMZ[combo.Name] -= factor * load.Value;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
