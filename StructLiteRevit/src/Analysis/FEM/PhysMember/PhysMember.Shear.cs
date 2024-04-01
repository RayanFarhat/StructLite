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
        ///    Returns the shear at a point along the member's length.
        /// </summary>
        ///   <param name="D"> The direction in which to find the shear. Must be one of local Fy or Fz</param>
        ///   <param name="x"> The location at which to find the shear.</param>
        public new double Shear(Direction D, double x, string combo_name = "Combo 1")
        {
            var T = Find_Member(x);
            var member = T.Item1;
            var x_mod = T.Item2;
            return member.Shear(D, x_mod, combo_name);
        }
        /// <summary>
        ///   Returns the maximum shear in the member for the given direction.
        /// </summary>
        ///   <param name="D"> The direction in which to find the maximum  shear. Must be one of local Fy or Fz</param>
        public new double Max_Shear(Direction D, string combo_name = "Combo 1")
        {
            var Vmax = Sub_Members.First().Value.Max_Shear(D, combo_name);
            foreach (var member in Sub_Members.Values)
            {
                var memMax = member.Max_Shear(D, combo_name);
                if (memMax > Vmax)
                {
                    Vmax = memMax;
                }
            }
            return Vmax;
        }
        /// <summary>
        ///   Returns the minimum shear in the member for the given direction.
        /// </summary>
        ///   <param name="D"> The direction in which to find the minimum  shear. Must be one of local Fy or Fz</param>
        public new double Min_Shear(Direction D, string combo_name = "Combo 1")
        {
            var Vmin = Sub_Members.First().Value.Min_Shear(D, combo_name);
            foreach (var member in Sub_Members.Values)
            {
                var memMin = member.Min_Shear(D, combo_name);
                if (memMin < Vmin)
                {
                    Vmin = memMin;
                }
            }
            return Vmin;
        }
    }
}
