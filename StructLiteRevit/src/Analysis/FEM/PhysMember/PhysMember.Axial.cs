using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    public partial class PhysMember : Member3D
    {
        /// <summary>
        ///    Returns the axial force at a point along the member's length.
        /// </summary>
        ///   <param name="x"> The location at which to find the axial force.</param>
        public new double Axial(double x, string combo_name = "Combo 1")
        {
            var T = Find_Member(x);
            var member = T.Item1;
            var x_mod = T.Item2;
            return member.Axial(x_mod, combo_name);
        }
        /// <summary>
        ///   Returns the maximum axial force moment in the member.
        /// </summary>
        public new double Max_Axial(string combo_name = "Combo 1")
        {
            var Pmax = Sub_Members.First().Value.Max_Axial(combo_name);
            foreach (var member in Sub_Members.Values)
            {
                var memMax = member.Max_Axial(combo_name);
                if (memMax > Pmax)
                {
                    Pmax = memMax;
                }
            }
            return Pmax;
        }
        /// <summary>
        ///   Returns the minimum axial force moment in the member.
        /// </summary>
        public new double Min_Axial(string combo_name = "Combo 1")
        {
            var Pmin = Sub_Members.First().Value.Min_Axial(combo_name);
            foreach (var member in Sub_Members.Values)
            {
                var memMin = member.Min_Axial(combo_name);
                if (memMin < Pmin)
                {
                    Pmin = memMin;
                }
            }
            return Pmin;
        }
    }
}
