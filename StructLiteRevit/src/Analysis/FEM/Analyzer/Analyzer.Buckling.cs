using StructLite.Analysis;
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
        /// Check if there is any buckling in the structure,still not implemented
        /// </summary>
        /// <returns>return true if there is buckling,false if not</returns>
        public static bool BucklingExist(FEModel3D model, List<string> Combo_tags = null)
        {
            return false;
            var comboList = identify_combos(model, Combo_tags);
            double Load, K;
            foreach (var combo in comboList)
            {
                foreach (var m in model.Members.Values)
                {

                    Direction loadDirection;
                    if (m.i_node.Y == m.j_node.Y && m.i_node.Z == m.j_node.Z)
                        loadDirection = Direction.FX;
                    else if (m.i_node.X == m.j_node.X && m.i_node.Z == m.j_node.Z)
                        loadDirection = Direction.FY;
                    else if (m.i_node.X == m.j_node.X && m.i_node.Y == m.j_node.Y)
                        loadDirection = Direction.FZ;
                    else
                        continue;

                    if (m.i_node.isFixed() && m.j_node.isFixed())
                        K = 0.5;
                    else if (m.i_node.isPinned() && m.j_node.isPinned())
                        K = 1.0;
                    else if ((m.i_node.isFixed() && m.j_node.isFree()) || 
                        (m.i_node.isFree() && m.j_node.isFixed()))
                        K = 2.0;
                    else if ((m.i_node.isFixed() && m.j_node.isPinned()) ||
                        (m.i_node.isPinned() && m.j_node.isFixed()))
                        K = 0.7;
                }
            }
                
            return false;
        }
    }
}
