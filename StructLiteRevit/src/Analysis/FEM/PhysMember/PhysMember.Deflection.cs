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
        ///    Returns the deflection at a point along the member's length.
        /// </summary>
        ///   <param name="D"> The direction in which to find the deflection. Must be one of local Fx or Fy or Fz</param>
        ///   <param name="x"> The location at which to find the deflection.</param>
        public new double Deflection(Direction D, double x, string combo_name = "Combo 1")
        {
            var T = Find_Member(x);
            var member = T.Item1;
            var x_mod = T.Item2;
            return member.Deflection(D, x_mod, combo_name);
        }
        /// <summary>
        ///   Returns the maximum deflection in the member for the given direction.
        /// </summary>
        ///   <param name="D"> The direction in which to find the maximum  deflection. Must be one of local Fx or Fy or Fz</param>
        public new double Max_Deflection(Direction D, string combo_name = "Combo 1")
        {
            var Dmax = Sub_Members.First().Value.Max_Deflection(D, combo_name);
            foreach (var member in Sub_Members.Values)
            {
                var memMax = member.Max_Deflection(D, combo_name);
                if (memMax > Dmax)
                {
                    Dmax = memMax;
                }
            }
            return Dmax;
        }
        /// <summary>
        ///   Returns the minimum deflection in the member for the given direction.
        /// </summary>
        ///   <param name="D"> The direction in which to find the minimum deflection. Must be one of local Fx or Fy or Fz</param>
        public new double Min_Deflection(Direction D, string combo_name = "Combo 1")
        {
            var Dmin = Sub_Members.First().Value.Min_Deflection(D, combo_name);
            foreach (var member in Sub_Members.Values)
            {
                var memMin = member.Min_Deflection(D, combo_name);
                if (memMin < Dmin)
                {
                    Dmin = memMin;
                }
            }
            return Dmin;
        }
        /// <summary>
        ///    Returns the relative deflection at a point along the member's length.
        /// </summary>
        ///   <param name="D"> The direction in which to find the relative deflection. Must be one of local Fy or Fz</param>
        ///   <param name="x"> The location at which to find the relative deflection.</param>
        public new double Rel_Deflection(Direction D, double x, string combo_name = "Combo 1")
        {
            var T = Find_Member(x);
            var member = T.Item1;
            var x_mod = T.Item2;
            return member.Rel_Deflection(D, x_mod, combo_name);
        }
    }
}
