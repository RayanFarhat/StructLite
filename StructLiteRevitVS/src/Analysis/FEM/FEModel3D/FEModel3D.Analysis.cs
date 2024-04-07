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
        /// Performs first-order static analysis.
        /// </summary>
        /// <param name="check_stability">hen set to True, checks the stiffness matrix for any unstable degrees of freedom
        /// and reports them back to the console. This does add to the solution time. Defaults to True.</param>
        public void Analyze(bool check_stability = true , List<string> Combo_tags = null)
        {
            // Prepare the model for analysis
            Analyzer.prepareModel(this);
            var K = this.K();
            if (check_stability)
            {
                Analyzer.check_stability(this, K);
            }

            // Get the auxiliary list used to determine how the matrices will be partitioned
            var t = Analyzer.Partition_D(this);
            var D1_indices = t.unknown_indices;
            var D2_indices = t.known_indices;
            var D2 = t.known_values;

            // Get the partitioned global stiffness matrix K11, K12, K21, K22
            var k_par = Analyzer.PartitionMatrix(K, D1_indices, D2_indices);
            var K11 = k_par.k11;
            var K12 = k_par.k12;
            var K21 = k_par.k21;
            var K22 = k_par.k22;

            // Identify which load combinations have the tags the user has given
            var comboList = Analyzer.identify_combos(this, Combo_tags);
            // Step through each load combination
            foreach (var combo in comboList)
            {
                // Get the partitioned global fixed end reaction vector
                var FER = Analyzer.PartitionVector(this.FER(combo.Name),D1_indices,D2_indices);
                var FER1 = FER.unknownVector;
                var FER2 = FER.knownVector;
                // Get the partitioned global nodal force vector      
                var P = Analyzer.PartitionVector(this.P(combo.Name), D1_indices, D2_indices);
                var P1 = P.unknownVector;
                var P2 = P.knownVector;

                // Calculate the global displacement vector

                Vector D1;
                if (K11.RowCount == 0 && K11.ColumnCount == 0)
                {
                    // All displacements are known, so D1 is an empty vector
                    D1 = new Vector(0);
                }
                else
                {
                    if (K11.IsSingular())
                    {
                        throw new Exception("The stiffness matrix is singular, which implies rigid body motion. The structure is unstable.");
                    }
                    // Calculate the unknown displacements D1
                    var F = P1.Subtract(FER1);
                    var F2 = Vector.FromMatrix(K12.Multiply(D2));//F2 = 0 when there is no enforced displacment
                    // this is equal to D1 = K11^-1 * F.Subtract(F2), but Solve is faster, any problems then switch to the old one
                    D1 = K11.Solve(F.Subtract(F2));
                }
                // Store the calculated displacements to the model and the nodes in the model
                Analyzer.StoreDisplacements(this,D1,D2,D1_indices,D2_indices, combo);
            }
            Analyzer.calcReactions(this, Combo_tags);
            if (Analyzer.BucklingExist(this, Combo_tags))
            {
                throw new Exception("The structure is unstable. Buckling Detected!");
            }
        }
    }
}
