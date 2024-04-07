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
        ///   Returns the moment at a point along the member's length.
        /// </summary>
        ///   <param name="D"> The local direction in which to find the moment. Must be one of My or Mz</param>
        ///   <param name="x"> The location at which to find the moment.</param>
        public double Moment(Direction D, double x, string combo_name = "Combo 1")
        {
            Check_segments(combo_name);
            if (D == Direction.My)
            {
                // Check which segment 'x' falls on
                foreach (var segment in SegmentsY)
                {
                    // x >= x1 and x < x2
                    if (Num.IsFirstBiggerOrEqualThanSecond(x, (double)segment.x1)
                        && Num.IsFirstSmallerThanSecond(x, (double)segment.x2))
                    {
                        return segment.Moment(x - (double)segment.x1);
                    }
                }
                if (Num.IsEqual(x, L()))
                {
                    var lastIndex = SegmentsY.Count - 1;
                    return SegmentsY[lastIndex].Moment(x - (double)SegmentsY[lastIndex].x1);
                }
            }
            else if (D == Direction.Mz)
            {
                // Check which segment 'x' falls on
                foreach (var segment in SegmentsZ)
                {
                    // x >= x1 and x < x2
                    if (Num.IsFirstBiggerOrEqualThanSecond(x, (double)segment.x1)
                        && Num.IsFirstSmallerThanSecond(x, (double)segment.x2))
                    {
                        return segment.Moment(x - (double)segment.x1);
                    }
                }
                if (Num.IsEqual(x, L()))
                {
                    var lastIndex = SegmentsZ.Count - 1;
                    return SegmentsZ[lastIndex].Moment(x - (double)SegmentsZ[lastIndex].x1);
                }
            }
            throw new Exception($"x or direction not valid in {combo_name}.");
        }
        /// <summary>
        ///  Returns the maximum moment in the member for the given direction.
        /// </summary>
        ///   <param name="D">The local direction in which to find the moment. Must be one of My or Mz</param>
        public double Max_Moment(Direction D, string combo_name = "Combo 1")
        {
            Check_segments(combo_name);
            double Mmax = 0;
            if (D == Direction.Mz)
            {
                Mmax = SegmentsZ[0].Moment(0);
                foreach (var segment in SegmentsZ)
                {
                    var segMax = segment.Max_Moment();
                    if (segMax > Mmax)
                        Mmax = segMax;
                }
            }
            else if (D == Direction.My)
            {
                Mmax = SegmentsY[0].Moment(0);
                foreach (var segment in SegmentsY)
                {
                    var segMax = segment.Max_Moment();
                    if (segMax > Mmax)
                        Mmax = segMax;
                }
            }
            else
                throw new Exception("Direction is not My or Mz.");

            return Mmax;
        }
        /// <summary>
        ///   Returns the minimum moment in the member for the given direction.
        /// </summary>
        ///   <param name="D">The local direction in which to find the moment. Must be one of My or Mz</param>
        public double Min_Moment(Direction D, string combo_name = "Combo 1")
        {
            Check_segments(combo_name);
            double Mmin = 0;
            if (D == Direction.Mz)
            {
                Mmin = SegmentsZ[0].Moment(0);
                foreach (var segment in SegmentsZ)
                {
                    var segMin = segment.Min_Moment();
                    if (segMin < Mmin)
                        Mmin = segMin;
                }
            }
            else if (D == Direction.My)
            {
                Mmin = SegmentsY[0].Moment(0);
                foreach (var segment in SegmentsY)
                {
                    var segMin = segment.Min_Moment();
                    if (segMin < Mmin)
                        Mmin = segMin;
                }
            }
            else
                throw new Exception("Direction is not My or Mz.");

            return Mmin;
        }
        /// <summary>
        /// Returns the array of the moment in the member for the given direction.
        /// </summary>
        ///   <param name="D">The local direction in which to find the moment. Must be one of My or Mz</param>
        ///   <param name="n_points">The number of points in the array to generate over the full length of the member.</param>
        public double[][] Moment_Array(Direction D, int n_points, string combo_name = "Combo 1")
        {
            Check_segments(combo_name);
            var x_arr = Num.Linspace(0, L(), n_points);
            var y_arr = x_arr.Select(x => Moment(D, x, combo_name)).ToArray(); ;
            double[][] twoDArray = new double[2][];
            twoDArray[0] = x_arr;
            twoDArray[1] = y_arr;
            return twoDArray;
        }
    }
}
