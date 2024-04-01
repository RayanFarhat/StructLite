using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    public partial class Member3D
    {
        /// <summary>
        /// Returns the torsional moment at a point along the member's length.
        /// </summary>
        ///   <param name="x">The location at which to find the torque.</param>
        public double Torsion(double x, string combo_name = "Combo 1")
        {
            Check_segments(combo_name);
            foreach (var segment in SegmentsX)
            {
                // x >= x1 and x < x2
                if (Num.IsFirstBiggerOrEqualThanSecond(x, (double)segment.x1)
                    && Num.IsFirstSmallerThanSecond(x, (double)segment.x2))
                {
                    return segment.Torsion();
                }
            }
            if (Num.IsEqual(x, L()))
            {
                var lastIndex = SegmentsX.Count - 1;
                return SegmentsX[lastIndex].Torsion();
            }
            throw new Exception($"x is not valid.");
        }
        /// <summary>
        /// Returns the maximum torsional moment in the member.
        /// </summary>
        public double Max_Torsion(string combo_name = "Combo 1")
        {
            Check_segments(combo_name);
            var Tmax = SegmentsX[0].Torsion();
            foreach (var segment in SegmentsX)
            {
                var segMax = segment.Max_Torsion();
                if (segMax > Tmax)
                {
                    Tmax = segMax;
                }
            }
            return Tmax;
        }
        /// <summary>
        /// Returns the minimum torsional moment in the member.
        /// </summary>
        public double Min_Torsion(string combo_name = "Combo 1")
        {
            Check_segments(combo_name);
            var Tmin = SegmentsX[0].Torsion();
            foreach (var segment in SegmentsX)
            {
                var segMin = segment.Min_Torsion();
                if (segMin < Tmin)
                {
                    Tmin = segMin;
                }
            }
            return Tmin;
        }
        /// <summary>
        /// Returns the array of the torque in the member for the given direction.
        /// </summary>
        ///   <param name="n_points">The number of points in the array to generate over the full length of the member.</param>
        public double[][] Torsion_Array(int n_points, string combo_name = "Combo 1")
        {
            Check_segments(combo_name);
            var x_arr = Num.Linspace(0, L(), n_points);
            var y_arr = x_arr.Select(x => Torsion(x, combo_name)).ToArray(); ;
            double[][] twoDArray = new double[2][];
            twoDArray[0] = x_arr;
            twoDArray[1] = y_arr;
            return twoDArray;
        }
    }
}
