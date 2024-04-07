using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    public partial class Member3D
    {
        /// <summary>
        ///   Returns the shear at a point along the member's length.
        /// </summary>
        ///   <param name="D">The local direction in which to find the shear. Must be one of Fy or Fz</param>
        ///   <param name="x">The location at which to find the shear.</param>
        public double Shear(Direction D, double x, string combo_name = "Combo 1")
        {
            Check_segments(combo_name);

            // Check which direction is of interest
            if (D == Direction.Fy)
            {
                // Check which segment 'x' falls on
                foreach (var segment in SegmentsZ)
                {
                    // x >= x1 and x < x2
                    if (Num.IsFirstBiggerOrEqualThanSecond(x, (double)segment.x1)
                        && Num.IsFirstSmallerThanSecond(x, (double)segment.x2))
                    {
                        return segment.Shear(x - (double)segment.x1);
                    }
                }
                if (Num.IsEqual(x, L()))
                {
                    var lastIndex = SegmentsZ.Count - 1;
                    return SegmentsZ[lastIndex].Shear(x - (double)SegmentsZ[lastIndex].x1);
                }
            }
            else if (D == Direction.Fz)
            {
                // Check which segment 'x' falls on
                foreach (var segment in SegmentsY)
                {
                    // x >= x1 and x < x2
                    if (Num.IsFirstBiggerOrEqualThanSecond(x, (double)segment.x1)
                        && Num.IsFirstSmallerThanSecond(x, (double)segment.x2))
                    {
                        return segment.Shear(x - (double)segment.x1);
                    }
                }
                if (Num.IsEqual(x, L()))
                {
                    var lastIndex = SegmentsY.Count - 1;
                    return SegmentsY[lastIndex].Shear(x - (double)SegmentsY[lastIndex].x1);
                }
            }
            throw new Exception($"x or direction not valid in {combo_name}.");
        }
        /// <summary>
        ///   Returns the maximum shear in the member for the given direction.
        /// </summary>
        ///   <param name="D">The local direction in which to find the shear. Must be one of Fy or Fz</param>
        public double Max_Shear(Direction D, string combo_name = "Combo 1")
        { 
            Check_segments(combo_name);
            double Vmax = 0;
            if (D == Direction.Fy)
            {
                Vmax = SegmentsZ[0].Shear(0);
                foreach (var segment in SegmentsZ)
                {
                    var segMax = segment.Max_Shear();
                    if (segMax > Vmax)
                        Vmax = segMax;
                }
            }
            else if (D == Direction.Fz)
            {
                Vmax = SegmentsY[0].Shear(0);
                foreach (var segment in SegmentsY)
                {
                    var segMax = segment.Max_Shear();
                    if (segMax > Vmax)
                        Vmax = segMax;
                }
            }
            else 
                throw new Exception("Direction is not Fy or Fz.");

            return Vmax;
        }
        /// <summary>
        ///   Returns the minimum shear in the member for the given direction.
        /// </summary>
        ///   <param name="D">The local direction in which to find the shear. Must be one of Fy or Fz</param>
        public double Min_Shear(Direction D, string combo_name = "Combo 1")
        {
            Check_segments(combo_name);
            double Vmin = 0;
            if (D == Direction.Fy)
            {
                Vmin = SegmentsZ[0].Shear(0);
                foreach (var segment in SegmentsZ)
                {
                    var segMin = segment.Min_Shear();
                    if (segMin < Vmin)
                        Vmin = segMin;
                }
            }
            else if (D == Direction.Fz)
            {
                Vmin = SegmentsY[0].Shear(0);
                foreach (var segment in SegmentsY)
                {
                    var segMin = segment.Min_Shear();
                    if (segMin < Vmin)
                        Vmin = segMin;
                }
            }
            else
                throw new Exception("Direction is not Fy or Fz.");

            return Vmin;
        }
        /// <summary>
        /// Returns the array of the shear in the member for the given direction.
        /// </summary>
        ///   <param name="D">The local direction in which to find the shear. Must be one of Fy or Fz</param>
        ///   <param name="n_points">The number of points in the array to generate over the full length of the member.</param>
        public double[][] Shear_Array(Direction D, int n_points, string combo_name = "Combo 1")
        {
            Check_segments(combo_name);
            var x_arr = Num.Linspace(0, L(), n_points);
            var y_arr = x_arr.Select(x => Shear(D, x, combo_name)).ToArray(); ;
            double[][] twoDArray = new double[2][];
            twoDArray[0] = x_arr;
            twoDArray[1] = y_arr;
            return twoDArray;
        }
    }
}
