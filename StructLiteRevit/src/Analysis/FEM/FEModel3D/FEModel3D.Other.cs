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
        /// Adds a new material to the model.
        /// </summary>
        ///   <param name="name">A unique user-defined name for the material.</param>
        public void add_material(double E, double G, double nu, double rho, double? fy = null, string name = null)
        {
            // Name the material  or check it doesn't already exist
            if (name != null)
            {
                if (Materials.ContainsKey(name))
                {
                    throw new Exception($"Material  name '{name}' already exists");
                }
            }
            else
            {
                // As a guess, start with the length of the dictionary
                name = "M" + Materials.Count;
                var count = 1;
                while (Materials.ContainsKey(name))
                {
                    name = "M" + Materials.Count + count.ToString();
                    count++;
                }
            }
            var new_material = new Material(name, E, G, nu, rho, fy);
            Materials.Add(name, new_material);
        }
        /// <summary>
        /// Adds a load combination to the model.
        /// </summary>
        ///   <param name="name">A unique name for the load combination (e.g. '1.2D+1.6L+0.5S' or 'Gravity Combo').</param>
        ///   <param name="factors">A dictionary containing load cases and their corresponding factors (e.g. {'D':1.2, 'L':1.6, 'S':0.5}).</param>
        ///   <param name="combo_tags">A list of tags used to categorize load combinations. Default is `None`. This can be useful for filtering results later on,
        ///   or for limiting analysis to only those combinations with certain tags. This feature is provided for convenience. It is not necessary to use tags.</param>
        public void add_load_combo(Dictionary<string, double> factors, string name = null, List<string> combo_tags = null)
        {
            // Name the material  or check it doesn't already exist
            if (name != null)
            {
                if (LoadCombos.ContainsKey(name))
                {
                    throw new Exception($"Load Combos  name '{name}' already exists");
                }
            }
            else
            {
                // As a guess, start with the length of the dictionary
                name = "LC" + LoadCombos.Count;
                var count = 1;
                while (LoadCombos.ContainsKey(name))
                {
                    name = "LC" + LoadCombos.Count + count.ToString();
                    count++;
                }
            }
            var new_loadCombo = new LoadCombo(name, factors, combo_tags);
            LoadCombos.Add(name, new_loadCombo);
        }
        /// <summary>
        /// Returns the global displacement vector for the model.
        /// </summary>
        /// <param name="combo_name">The name of the load combination to get the results for. Defaults to 'Combo 1'.</param>
        /// <returns>The global displacement vector for the model</returns>
        public Vector D(string combo_name = "Combo 1")
        {
            return _D[combo_name];
        }
        /// <summary>
        /// override the Main ToString method to print every thing in the model
        /// </summary>
        /// <returns>string with every thing in the model</returns>
        public override string ToString()
        {
            var str = "";
            foreach (var combo in this.LoadCombos)
            {
                str += combo.Value.Name + ":\n";
                foreach (var factor in combo.Value.Factors)
                {
                    str += $"   case name {factor.Key}, factor = {factor.Value}\n";
                }
            }
            str += "Nodes:\n";
            foreach (var node in this.Nodes)
            {
                str += $"{node.Key} x:{node.Value.X}, y:{node.Value.Y}, z:{node.Value.Z}  \n : dx={node.Value.support_DX},dy={node.Value.support_DY},dz={node.Value.support_DZ},rx={node.Value.support_RX},ry={node.Value.support_RY},rz={node.Value.support_RZ}\n";
            }
            foreach (var mem in this.Members)
            {
                str += $"member {mem.Key}:\n    node i {mem.Value.i_node.Name}\n    node j {mem.Value.j_node.Name}\n    len {mem.Value.L()}\n";
                foreach (var ptLoad in mem.Value.PtLoads)
                {
                    string strDir;
                    switch (ptLoad.direction)
                    {
                        case Direction.FX:
                            strDir = "FX";
                            break;
                        case Direction.FY:
                            strDir = "FY";
                            break;
                        case Direction.FZ:
                            strDir = "FZ";
                            break;
                        case Direction.Fx:
                            strDir = "Fx";
                            break;
                        case Direction.Fy:
                            strDir = "Fy";
                            break;
                        case Direction.Fz:
                            strDir = "Fz";
                            break;
                        case Direction.MX:
                            strDir = "MX";
                            break;
                        case Direction.MY:
                            strDir = "MY";
                            break;
                        case Direction.MZ:
                            strDir = "MZ";
                            break;
                        case Direction.Mx:
                            strDir = "Mx";
                            break;
                        case Direction.My:
                            strDir = "My";
                            break;
                        case Direction.Mz:
                            strDir = "Mz";
                            break;
                        default:
                            strDir = "Unknown Dir";
                            break;
                    }
                    str += $"   point load {ptLoad.P}, direction {strDir}, position on member {ptLoad.X}, case name {ptLoad.CaseName}\n";
                }
                foreach (var distLoad in mem.Value.DistLoads)
                {
                    string strDir;
                    switch (distLoad.direction)
                    {
                        case Direction.FX:
                            strDir = "FX";
                            break;
                        case Direction.FY:
                            strDir = "FY";
                            break;
                        case Direction.FZ:
                            strDir = "FZ";
                            break;
                        case Direction.Fx:
                            strDir = "Fx";
                            break;
                        case Direction.Fy:
                            strDir = "Fy";
                            break;
                        case Direction.Fz:
                            strDir = "Fz";
                            break;
                        default:
                            strDir = "Unknown Dir";
                            break;
                    }
                    str += $"   line load w1 {distLoad.w1},w2 {distLoad.w2}, direction {strDir}, x1 {distLoad.x1},x2 {distLoad.x2}, case name {distLoad.CaseName}\n";
                }
            }
            return str;
        }
    }
}
