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
        ///    Returns the moment at a point along the member's length.
        /// </summary>
        ///   <param name="D"> The direction in which to find the moment. Must be one of local My or Mz</param>
        ///   <param name="x"> The location at which to find the moment.</param>
        public new double Moment(Direction D, double x, string combo_name = "Combo 1")
        {
            var T = Find_Member(x);
            var member = T.Item1;
            var x_mod = T.Item2;
            return member.Moment(D, x_mod, combo_name);
        }
        /// <summary>
        ///   Returns the maximum moment in the member for the given direction.
        /// </summary>
        ///   <param name="D"> The direction in which to find the maximum moment. Must be one of local My or Mz</param>
        public new double Max_Moment(Direction D, string combo_name = "Combo 1")
        {
            var Mmax = Sub_Members.First().Value.Max_Moment(D, combo_name);
            foreach (var member in Sub_Members.Values)
            {
                var memMax = member.Max_Moment(D, combo_name);
                if (memMax > Mmax)
                {
                    Mmax = memMax;
                }
            }
            return Mmax;
        }
        /// <summary>
        ///   Returns the minimum moment in the member for the given direction.
        /// </summary>
        ///   <param name="D"> The direction in which to find the minimum moment. Must be one of local My or Mz</param>
        public new double Min_Moment(Direction D, string combo_name = "Combo 1")
        {
            var Mmin = Sub_Members.First().Value.Min_Moment(D, combo_name);
            foreach (var member in Sub_Members.Values)
            {
                var memMin = member.Min_Moment(D, combo_name);
                if (memMin < Mmin)
                {
                    Mmin = memMin;
                }
            }
            return Mmin;
        }
    }
}
