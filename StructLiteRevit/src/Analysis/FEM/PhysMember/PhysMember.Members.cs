using StructLite.Mathematics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    public partial class PhysMember : Member3D
    {
        /// <summary>
        ///    Returns the sub-member that the physical member's local point 'x' lies on, and 'x' modified for that sub-member's local coordinate system.
        /// </summary>
        public Tuple<Member3D, double> Find_Member(double x)
        {
            // Initialize a summation of sub-member lengths
            double L = 0;

            //  Step through each sub-member (in order from start to end)
            for (int i = 0; i < Sub_Members.Count; i++)
            {
                var member = Sub_Members.ElementAt(i).Value;
                // Sum the sub-member's length
                L += member.L();

                // Check if 'x' lies on this sub-member
                // x < L || (x==L and it is last member)
                if (Num.IsFirstSmallerThanSecond(x,L) || 
                    (Num.IsEqual(x,L) && i == Sub_Members.Count - 1))
                {
                    // Return the sub-member, and a modified value for 'x' relative to the sub-member's i-node
                    return new Tuple<Member3D, double>(member, x - (L - member.L()));
                }
            }
            throw new Exception("x value is not on the member");
        }
        /// <summary>
        /// Subdivides the physical member into sub-members at each node along the physical member.
        /// </summary>
        public void Descritize()
            {
                // Clear out any old sub_members
                Sub_Members.Clear();

                // Start a new list of nodes along the member
                var int_nodes = new Dictionary<Node3D, double>();

                // Create a vector from the i-node to the j-node
                var Xi = i_node.X;
                var Yi = i_node.Y;
                var Zi = i_node.Z;
                var Xj = j_node.X;
                var Yj = j_node.Y;
                var Zj = j_node.Z;
                var vector_ij = new Vector(new double[] { Xj - Xi, Yj - Yi, Zj - Zi });
                var vector_ij_Norm = vector_ij.GetMagnitude();

                // Add the i-node and j-node to the list
                int_nodes.Add(i_node, 0);
                int_nodes.Add(j_node, vector_ij_Norm);//norm

                // Step through each node in the model
                foreach (var node in this.Model.Nodes.Values)
                {
                    if (i_node.Name != node.Name && j_node.Name != node.Name)
                    {
                        // Create a vector from the i-node to the current node
                        var X = node.X;
                        var Y = node.Y;
                        var Z = node.Z;
                        var vector_in = new Vector(new double[] { X - Xi, Y - Yi, Z - Zi });

                        var vector_in_Norm = vector_in.GetMagnitude();
                        // Calculate the angle between the two vectors
                        var angle = Math.Acos(
                            vector_in.DotProduct(vector_ij)
                            / (vector_in_Norm * vector_ij_Norm)
                            );
                        // Determine if the node is colinear with the member
                        if (Num.IsEqual(angle, 0))
                        {
                            // Determine if the node is on the member
                            if (Num.IsFirstSmallerThanSecond(vector_in_Norm, vector_ij_Norm))
                                int_nodes.Add(node, vector_in_Norm);
                        }
                    }
                }
                // Create a list of sorted intermediate nodes by distance from the i-node
                var nodes = int_nodes.OrderBy(x => x.Value).ToList();

                // Break up the member into sub-members at each intermediate node
                for (int i = 0; i < nodes.Count - 1; i++)
                {
                    // Generate the sub-member's name (physical member name + a, b, c, etc.)
                    // Note: this as we know that the number of nodes is not huge
                    var name = this.Name + (char)(i + 97);

                    // Find the i and j nodes for the sub-member, and their positions along the physical
                    // member's local x-axis
                    var node_i = nodes[i].Key;
                    var node_j = nodes[i + 1].Key;
                    var xi = nodes[i].Value;
                    var xj = nodes[i + 1].Value;

                    // Create a new sub-member
                    var new_sub_member = new Member3D(name, node_i, node_j, this.MaterialName, Model, Iy, Iz, Jx, A);

                    // Add distributed to the sub-member
                    foreach (var dist_load in this.DistLoads)
                    {
                        // Find the start and end points of the distributed load in the physical member's
                        // local coordinate system
                        var x1_load = dist_load.x1;
                        var x2_load = dist_load.x2;

                        // Determine if the distributed load should be applied to this segment
                        // x1_load <= xj and x2_load > xi
                        if (Num.IsFirstSmallerOrEqualThanSecond(x1_load, xj) &&
                            Num.IsFirstBiggerThanSecond(x2_load, xi))
                        {
                            double x1 = 0;
                            double x2 = 0;
                            var d = dist_load.direction;
                            var w1 = dist_load.w1;
                            var w2 = dist_load.w2;
                            var caseName = dist_load.CaseName;

                            // Equation describing the load as a function of x
                            Func<double, double> w = x => (w2 - w1) / (x2_load - x1_load) * (x - x1_load) + w1;

                            // Chop up the distributed load for the sub-member
                            // x1_load > xi:
                            if (Num.IsFirstBiggerThanSecond(x1_load, xi))
                            {
                                x1 = x1_load - xi;
                            }
                            else
                            {
                                x1 = 0;
                                w1 = w(xi);
                            }
                            // x2_load < xj
                            if (Num.IsFirstSmallerThanSecond(x2_load, xj))
                            {
                                x2 = x2_load - xi;
                            }
                            else
                            {
                                x2 = xj - xi;
                                w2 = w(xj);
                            }
                            new_sub_member.DistLoads.Add(new DistributedLoad(d, w1, w2, x1, x2, caseName));
                        }
                    }
                    // Add point loads to the sub-member
                    foreach (var pt_load in this.PtLoads)
                    {
                        var d = pt_load.direction;
                        var p = pt_load.P;
                        var x = pt_load.X;
                        var caseName = pt_load.CaseName;

                        // Determine if the point load should be applied to this segment
                        // x >= xi and x < xj or (isclose(x, xj) and isclose(xj, self.L()))
                        if (
                            Num.IsFirstBiggerOrEqualThanSecond(x, xi) &&
                            Num.IsFirstSmallerThanSecond(x, xj) ||
                            (Num.IsEqual(x, xj) && Num.IsEqual(xj, L()))
                            )
                        {
                            x = x - xi;
                            // Add the load to the sub-member
                            new_sub_member.PtLoads.Add(new PointLoad(d, p, x, caseName));
                        }
                    }
                    Sub_Members.Add(name, new_sub_member);
            }
        }
    }
}
