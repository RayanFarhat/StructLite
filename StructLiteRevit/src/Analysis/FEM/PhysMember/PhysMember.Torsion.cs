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
        ///    Returns the torsional moment at a point along the member's length.
        /// </summary>
        ///   <param name="x"> The location at which to find the torque.</param>
        public new double Torsion(double x, string combo_name = "Combo 1")
        {
            var T = Find_Member(x);
            var member = T.Item1;
            var x_mod = T.Item2;
            return member.Torsion(x_mod, combo_name);
        }
        /// <summary>
        ///   Returns the maximum torsional moment in the member.
        /// </summary>
        public new double Max_Torsion(string combo_name = "Combo 1")
        {
            var Tmax = Sub_Members.First().Value.Max_Torsion(combo_name);
            foreach (var member in Sub_Members.Values)
            {
                var memMax = member.Max_Torsion(combo_name);
                if (memMax > Tmax)
                {
                    Tmax = memMax;
                }
            }
            return Tmax;
        }
        /// <summary>
        ///   Returns the minimum torsional moment in the member.
        /// </summary>
        public new double Min_Torsion(string combo_name = "Combo 1")
        {
            var Tmin = Sub_Members.First().Value.Min_Torsion(combo_name);
            foreach (var member in Sub_Members.Values)
            {
                var memMin = member.Min_Torsion(combo_name);
                if (memMin < Tmin)
                {
                    Tmin = memMin;
                }
            }
            return Tmin;
        }
    }
}
