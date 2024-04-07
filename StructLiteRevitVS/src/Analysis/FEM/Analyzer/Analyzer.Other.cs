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
        /// Returns a list of load combinations that are to be run based on tags given by the user.
        /// </summary>
        /// <param name="model">The model being analyzed.</param>
        /// <param name="Combo_tags">A list of tags used for the load combinations to be evaluated. Defaults to `Null`
        /// in which case all load combinations will be added to the list of load combinations to be run.</param>
        public static List<LoadCombo> identify_combos(FEModel3D model, List<string> Combo_tags = null)
        {
            List<LoadCombo> combo_list;
            // Identify which load combinations to evaluate
            if (Combo_tags == null)
            {
                // Evaluate all load combinations if not tags have been provided
                combo_list = model.LoadCombos.Values.ToList();
            }
            else
            {
                // Initialize the list of load combinations to be evaluated
                combo_list = new List<LoadCombo>();
                // Step through each load combination in the model
                foreach (var combo in model.LoadCombos.Values)
                {
                    // Check if this load combination is tagged with any of the tags we're looking for
                    if (combo.Combo_tags != null && combo.Combo_tags.Any(tag => Combo_tags.Contains(tag)))
                    {
                        // Add the load combination to the list of load combinations to be evaluated
                        combo_list.Add(combo);
                    }
                }
            }
            return combo_list;
        }
    }
}
