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
        /// Builds a list with known nodal displacements and with the positions in global stiffness matrix of known and unknown nodal displacements.
        /// </summary>
        /// <param name="model">3D model that is analyzed</param>
        /// <returns>A list of the global matrix indices for the unknown nodal displacements (D1_indices). A list of the global matrix indices
        /// for the known nodal displacements (D2_indices). A Vector of the known nodal displacements (D2).</returns>
        public static (List<int> unknown_indices, List<int> known_indices, Vector known_values) Partition_D(FEModel3D model)
        {
            var D1_indices = new List<int>();
            var D2_indices = new List<int>();
            var D2 = new List<double>();

            // Create the auxiliary table
            foreach (var node in model.Nodes.Values)
            {
                var ID = (int)node.ID;

                // Unknown displacement DX
                if (node.support_DX == false && node.EnforcedDX == null)
                {
                    D1_indices.Add(ID * 6 + 0);
                }
                // Known displacement DX
                else if (node.EnforcedDX != null)
                {
                    D2_indices.Add(ID * 6 + 0);
                    D2.Add((double)node.EnforcedDX);
                }
                // Support at DX
                else
                {
                    D2_indices.Add(ID * 6 + 0);
                    D2.Add(0);
                }

                // Unknown displacement DY
                if (node.support_DY == false && node.EnforcedDY == null)
                {
                    D1_indices.Add(ID * 6 + 1);
                }
                // Known displacement DY
                else if (node.EnforcedDY != null)
                {
                    D2_indices.Add(ID * 6 + 1);
                    D2.Add((double)node.EnforcedDY);
                }
                // Support at DY
                else
                {
                    D2_indices.Add(ID * 6 + 1);
                    D2.Add(0);
                }

                // Unknown displacement DZ
                if (node.support_DZ == false && node.EnforcedDZ == null)
                {
                    D1_indices.Add(ID * 6 + 2);
                }
                // Known displacement DZ
                else if (node.EnforcedDZ != null)
                {
                    D2_indices.Add(ID * 6 + 2);
                    D2.Add((double)node.EnforcedDZ);
                }
                // Support at DZ
                else
                {
                    D2_indices.Add(ID * 6 + 2);
                    D2.Add(0);
                }

                // Unknown displacement RX
                if (node.support_RX == false && node.EnforcedRX == null)
                {
                    D1_indices.Add(ID * 6 + 3);
                }
                // Known displacement RX
                else if (node.EnforcedRX != null)
                {
                    D2_indices.Add(ID * 6 + 3);
                    D2.Add((double)node.EnforcedRX);
                }
                // Support at RX
                else
                {
                    D2_indices.Add(ID * 6 + 3);
                    D2.Add(0);
                }

                // Unknown displacement RY
                if (node.support_RY == false && node.EnforcedRY == null)
                {
                    D1_indices.Add(ID * 6 + 4);
                }
                // Known displacement RY
                else if (node.EnforcedRY != null)
                {
                    D2_indices.Add(ID * 6 + 4);
                    D2.Add((double)node.EnforcedRY);
                }
                // Support at RY
                else
                {
                    D2_indices.Add(ID * 6 + 4);
                    D2.Add(0);
                }

                // Unknown displacement RZ
                if (node.support_RZ == false && node.EnforcedRZ == null)
                {
                    D1_indices.Add(ID * 6 + 5);
                }
                // Known displacement RZ
                else if (node.EnforcedRZ != null)
                {
                    D2_indices.Add(ID * 6 + 5);
                    D2.Add((double)node.EnforcedRZ);
                }
                // Support at RZ
                else
                {
                    D2_indices.Add(ID * 6 + 5);
                    D2.Add(0);
                }
            }

            // Convert D2 from a list to a vector
            var VectorD2 = new Vector(D2.Count);
            for (int i = 0; i < D2.Count; i++)
            {
                VectorD2[i] = D2[i];
            }
            return (unknown_indices:D1_indices, known_indices:D2_indices, known_values:VectorD2);
        }

        /// <summary>
        /// Partitions a vector into subvectors based on degree of freedom boundary conditions.
        /// </summary>
        /// <param name="unp_vector">The unpartitioned vector to be partitioned.</param>
        /// <param name="D1_indices">A list of the indices for degrees of freedom that have unknown displacements.</param>
        /// <param name="D2_indices">A list of the indices for degrees of freedom that have known displacements.</param>
        /// <returns>Partitioned subvectors based on degree of freedom boundary conditions.</returns>
        public static (Vector unknownVector, Vector knownVector) PartitionVector(Vector unp_vector, List<int> D1_indices, List<int> D2_indices)
        {
            var VectorD1 = new Vector(D1_indices.Count);
            var VectorD2 = new Vector(D2_indices.Count);
            for (int i = 0; i < D1_indices.Count; i++)
            {
                VectorD1[i] = unp_vector[D1_indices[i]];
            }
            for (int i = 0; i < D2_indices.Count; i++)
            {
                VectorD2[i] = unp_vector[D2_indices[i]];
            }
            return (unknownVector: VectorD1, knownVector:VectorD2);
        }

        /// <summary>
        /// Partitions a matrix into submatrices based on degree of freedom boundary conditions.
        /// </summary>
        /// <param name="unp_matrix">The unpartitioned matrix to be partitioned.</param>
        /// <param name="D1_indices">A list of the indices for degrees of freedom that have unknown displacements.</param>
        /// <param name="D2_indices">A list of the indices for degrees of freedom that have known displacements.</param>
        /// <returns>Partitioned submatrices based on degree of freedom boundary conditions.</returns>
        public static (Matrix k11, Matrix k12, Matrix k21, Matrix k22) PartitionMatrix(Matrix unp_matrix, List<int> D1_indices, List<int> D2_indices)
        {
            var k11 = new Matrix(D1_indices.Count, D1_indices.Count);
            var k12 = new Matrix(D1_indices.Count, D2_indices.Count);
            var k21 = new Matrix(D2_indices.Count, D1_indices.Count);
            var k22 = new Matrix(D2_indices.Count, D2_indices.Count);

            for (int i = 0; i < D1_indices.Count; i++)
            {
                for (int j = 0; j < D1_indices.Count; j++)
                {
                    k11[i, j] = unp_matrix[D1_indices[i], D1_indices[j]];
                }
            }
            for (int i = 0; i < D1_indices.Count; i++)
            {
                for (int j = 0; j < D2_indices.Count; j++)
                {
                    k12[i, j] = unp_matrix[D1_indices[i], D2_indices[j]];
                }
            }
            for (int i = 0; i < D2_indices.Count; i++)
            {
                for (int j = 0; j < D1_indices.Count; j++)
                {
                    k21[i, j] = unp_matrix[D2_indices[i], D1_indices[j]];
                }
            }
            for (int i = 0; i < D2_indices.Count; i++)
            {
                for (int j = 0; j < D2_indices.Count; j++)
                {
                    k22[i, j] = unp_matrix[D2_indices[i], D2_indices[j]];
                }
            }
            return (k11, k12, k21, k22);
        }
        /// <summary>
        /// Unpartitions displacements from the solver and returns them as a global displacement vector.
        /// </summary>
        /// <param name="model">The finite element model being evaluated.</param>
        /// <param name="D1">An array of calculated displacements.</param>
        /// <param name="D2">An array of enforced displacements.</param>
        /// <param name="D1_indices">A list of the degree of freedom indices for each displacement in D1</param>
        /// <param name="D2_indices">A list of the degree of freedom indices for each displacement in D2</param>
        /// <returns>Global displacement vector.</returns>
        public static Vector Unpartition_D(FEModel3D model, Vector D1, Vector D2, List<int> D1_indices, List<int> D2_indices)
        {
            var D = new Vector(model.Nodes.Count * 6);

            foreach (var node in model.Nodes.Values)
            {
                var ID = (int)node.ID;

                // for each DOF cehck if it is in D2_indices, if not then it is inside D1_indices
                var dx = ID * 6 + 0;
                if (D2_indices.Contains(dx))
                    D[dx] = D2[D2_indices.IndexOf(dx)];
                else
                    D[dx] = D1[D1_indices.IndexOf(dx)];
                var dy = ID * 6 + 1;
                if (D2_indices.Contains(dy))
                    D[dy] = D2[D2_indices.IndexOf(dy)];
                else
                    D[dy] = D1[D1_indices.IndexOf(dy)];
                var dz = ID * 6 + 2;
                if (D2_indices.Contains(dz))
                    D[dz] = D2[D2_indices.IndexOf(dz)];
                else
                    D[dz] = D1[D1_indices.IndexOf(dz)];
                var rx = ID * 6 + 3;
                if (D2_indices.Contains(rx))
                    D[rx] = D2[D2_indices.IndexOf(rx)];
                else
                    D[rx] = D1[D1_indices.IndexOf(rx)];
                var ry = ID * 6 + 4;
                if (D2_indices.Contains(ry))
                    D[ry] = D2[D2_indices.IndexOf(ry)];
                else
                    D[ry] = D1[D1_indices.IndexOf(ry)];
                var rz = ID * 6 + 5;
                if (D2_indices.Contains(rz))
                    D[rz] = D2[D2_indices.IndexOf(rz)];
                else
                    D[rz] = D1[D1_indices.IndexOf(rz)];
            }

            return D;
        }
    }
}
