using Autodesk.Revit.DB;
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
        /// Returns the axial force at a point along the member's length.
        /// </summary>
        ///   <param name="x">The location at which to find the axial force.</param>
        public double Axial(double x, string combo_name = "Combo 1")
        {
            Check_segments(combo_name);

            foreach (var segment in SegmentsZ)
            {
                // x >= x1 and x < x2
                if (Num.IsFirstBiggerOrEqualThanSecond(x, (double)segment.x1)
                    && Num.IsFirstSmallerThanSecond(x, (double)segment.x2))
                {
                    return segment.Axial(x - (double)segment.x1);
                }
            }
            if (Num.IsEqual(x, L()))
            {
                var lastIndex = SegmentsZ.Count - 1;
                return SegmentsZ[lastIndex].Axial(x - (double)SegmentsZ[lastIndex].x1);
            }
            throw new Exception($"x is not valid.");
        }
        /// <summary>
        /// Returns the maximum axial force in the member.
        /// </summary>
        public double Max_Axial(string combo_name = "Combo 1")
        {
            Check_segments(combo_name);
            var Pmax = SegmentsZ[0].Axial(0);
            foreach (var segment in SegmentsZ)
            {
                var segMax = segment.max_axial();
                if (segMax > Pmax)
                {
                    Pmax = segMax;
                }
            }
            return Pmax;
        }
        /// <summary>
        /// Returns the minimum axial force in the member.
        /// </summary>
        public double Min_Axial(string combo_name = "Combo 1")
        {
            Check_segments(combo_name);
            var Pmin = SegmentsZ[0].Axial(0);
            foreach (var segment in SegmentsZ)
            {
                var segMin = segment.min_axial();
                if (segMin < Pmin)
                {
                    Pmin = segMin;
                }
            }
            return Pmin;
        }
        /// <summary>
        /// Returns the array of the axial force in the member for the given direction.
        /// </summary>
        ///   <param name="n_points">The number of points in the array to generate over the full length of the member.</param>
        public double[][] Axial_Array(int n_points, string combo_name = "Combo 1")
        {
            Check_segments(combo_name);
            var x_arr = Num.Linspace(0, L(), n_points);
            var y_arr = x_arr.Select(x => Axial(x, combo_name)).ToArray(); ;
            double[][] twoDArray = new double[2][];
            twoDArray[0] = x_arr;
            twoDArray[1] = y_arr;
            return twoDArray;
        }
    }
}
