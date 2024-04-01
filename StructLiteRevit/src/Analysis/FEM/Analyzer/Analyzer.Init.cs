using StructLite.Mathematics;
using System;
using System.Collections;
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
        /// Prepares a model for analysis by ensuring at least one load combination is defined, and internally numbering all nodes and elements.
        /// </summary>
        public static void prepareModel(FEModel3D model)
        {
            // Reset any nodal displacements
            model._D = new Dictionary<string, Vector>();
            foreach (var node in model.Nodes.Values)
            {
                node.DX = new Dictionary<string, double>();
                node.DY = new Dictionary<string, double>();
                node.DZ = new Dictionary<string, double>();
                node.RX = new Dictionary<string, double>();
                node.RY = new Dictionary<string, double>();
                node.RZ = new Dictionary<string, double>();
            }
            // Ensure there is at least 1 load combination to solve if the user didn't define any
            if (model.LoadCombos.Count == 0)
            {
                // Create and add a default load combination to the dictionary of load combinations
                var combo = new LoadCombo("Combo 1", new Dictionary<string, double> { { "Case 1", 1.0 } });
                model.LoadCombos.Add("Combo 1", combo);
            }
            // Assign an internal ID to all nodes and elements in the model.
            // This number is different from the name used by the user to identify nodes and elements.
            renumber(model);
        }
        /// <summary>
        ///  Assigns node and element ID numbers to be used internally by the program. Numbers are
        /// assigned according to the order in which they occur in each dictionary.
        /// </summary>
        public static void renumber(FEModel3D model)
        {
            int id = 0;
            // Number each node in the model
            foreach (var node in model.Nodes.Values)
            {
                node.ID = id;
                id++;
            }

            // Number each member in the model
            id = 0;
            foreach (var member in model.Members.Values)
            {
                member.ID = id;
                id++;
            }
            
        }
    }
}
