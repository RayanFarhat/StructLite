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
        ///   Returns the deflection at a point along the member's length.
        /// </summary>
        ///   <param name="D">The local direction in which to find the deflection. Must be one of local Fx or Fy or Fz</param>
        ///   <param name="x">The location at which to find the deflection.</param>
        public double Deflection(Direction D, double x, string combo_name = "Combo 1")
        {
            Check_segments(combo_name);

            if (D == Direction.Fx)
            {
                // Check which segment 'x' falls on
                foreach (var segment in SegmentsZ)
                {
                    // x >= x1 and x < x2
                    if (Num.IsFirstBiggerOrEqualThanSecond(x, (double)segment.x1)
                        && Num.IsFirstSmallerThanSecond(x, (double)segment.x2))
                    {
                        return segment.AxialDeflection(x - (double)segment.x1);
                    }
                }
                if (Num.IsEqual(x, L()))
                {
                    var lastIndex = SegmentsZ.Count - 1;
                    return SegmentsZ[lastIndex].AxialDeflection(x - (double)SegmentsZ[lastIndex].x1);
                }
            }
            else if (D == Direction.Fy)
            {
                // Check which segment 'x' falls on
                foreach (var segment in SegmentsZ)
                {
                    // x >= x1 and x < x2
                    if (Num.IsFirstBiggerOrEqualThanSecond(x, (double)segment.x1)
                        && Num.IsFirstSmallerThanSecond(x, (double)segment.x2))
                    {
                        return segment.Deflection(x - (double)segment.x1);
                    }
                }
                if (Num.IsEqual(x, L()))
                {
                    var lastIndex = SegmentsZ.Count - 1;
                    return SegmentsZ[lastIndex].Deflection(x - (double)SegmentsZ[lastIndex].x1);
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
                        return segment.Deflection(x - (double)segment.x1);
                    }
                }
                if (Num.IsEqual(x, L()))
                {
                    var lastIndex = SegmentsY.Count - 1;
                    return SegmentsY[lastIndex].Deflection(x - (double)SegmentsY[lastIndex].x1);
                }
            }
            throw new Exception($"x or direction not valid in {combo_name}.");
        }
        /// <summary>
        ///   Returns the maximum deflection in the member.
        /// </summary>
        ///   <param name="D">The local direction in which to find the maximum deflection. Must be one of local Fy or Fz</param>
        public double Max_Deflection(Direction D, string combo_name = "Combo 1")
        {
            Check_segments(combo_name);

            var Dmax = Deflection(D, 0, combo_name);
            // Check the deflection at 100 locations along the member and find the largest value
            for (int i = 1; i < 100; i++)
            {
                var d = Deflection(D, L() * i / 99, combo_name);
                if(d > Dmax)
                    Dmax = d;
            }
            return Dmax;
        }
        /// <summary>
        ///   Returns the minimum deflection in the member.
        /// </summary>
        ///   <param name="D">The local direction in which to find the minimum deflection. Must be one of local Fy or Fz</param>
        public double Min_Deflection(Direction D, string combo_name = "Combo 1")
        {
            Check_segments(combo_name);

            var Dmin = Deflection(D, 0, combo_name);
            // Check the deflection at 100 locations along the member and find the smallest value
            for (int i = 1; i < 100; i++)
            {
                var d = Deflection(D, L() * i / 99, combo_name);
                if (d < Dmin)
                    Dmin = d;
            }
            return Dmin;
        }
        /// <summary>
        /// Returns the array of the deflection in the member for the given direction.
        /// </summary>
        ///   <param name="D">The local direction in which to find the deflection. Must be one of local Fx or Fy or Fz</param>
        ///   <param name="n_points">The number of points in the array to generate over the full length of the member.</param>
        public double[][] Deflection_Array(Direction D, int n_points, string combo_name = "Combo 1")
        {
            Check_segments(combo_name);
            var x_arr = Num.Linspace(0, L(), n_points);
            var y_arr = x_arr.Select(x => Deflection(D, x, combo_name)).ToArray(); ;
            double[][] twoDArray = new double[2][];
            twoDArray[0] = x_arr;
            twoDArray[1] = y_arr;
            return twoDArray;
        }
        /// <summary>
        ///   Returns the relative deflection at a point along the member's length.
        /// </summary>
        ///   <param name="D">The local direction in which to find the relative  deflection. Must be one of local Fy or Fz</param>
        ///   <param name="x">The location at which to find the relative deflection.</param>
        public double Rel_Deflection(Direction D, double x, string combo_name = "Combo 1")
        {
            Check_segments(combo_name);

            var d = this.d();
            var dyi = d[1];
            var dyj = d[7];
            var dzi = d[2];
            var dzj = d[8];
            var L = this.L();

            if (D == Direction.FY)
            {
                // Check which segment 'x' falls on
                foreach (var segment in SegmentsZ)
                {
                    // x >= x1 and x < x2
                    if (Num.IsFirstBiggerOrEqualThanSecond(x, (double)segment.x1)
                        && Num.IsFirstSmallerThanSecond(x, (double)segment.x2))
                    {
                        return segment.Deflection(x - (double)segment.x1) - (dyi + (dyj - dyi) / L * x);
                    }
                }
                if (Num.IsEqual(x, L))
                {
                    var lastIndex = SegmentsZ.Count - 1;
                    return SegmentsZ[lastIndex].Deflection(x - (double)SegmentsZ[lastIndex].x1) - dyj;
                }
            }
            else if (D == Direction.FZ)
            {
                // Check which segment 'x' falls on
                foreach (var segment in SegmentsY)
                {
                    // x >= x1 and x < x2
                    if (Num.IsFirstBiggerOrEqualThanSecond(x, (double)segment.x1)
                        && Num.IsFirstSmallerThanSecond(x, (double)segment.x2))
                    {
                        return segment.Deflection(x - (double)segment.x1) - (dzi + (dzj - dzi) / L * x);
                    }
                }
                if (Num.IsEqual(x, L))
                {
                    var lastIndex = SegmentsY.Count - 1;
                    return SegmentsY[lastIndex].Deflection(x - (double)SegmentsY[lastIndex].x1) - dzj;
                }
            }
            throw new Exception($"x or direction not valid in {combo_name}.");
        }
        /// <summary>
        /// Returns the array of the relative deflection in the member for the given direction.
        /// </summary>
        ///   <param name="D">The local direction in which to find the relative deflection. Must be one of local Fy or Fz</param>
        ///   <param name="n_points">The number of points in the array to generate over the full length of the member.</param>
        public double[][] Rel_Deflection_Array(Direction D, int n_points, string combo_name = "Combo 1")
        {
            Check_segments(combo_name);
            var x_arr = Num.Linspace(0, L(), n_points);
            var y_arr = x_arr.Select(x => Rel_Deflection(D, x, combo_name)).ToArray(); ;
            double[][] twoDArray = new double[2][];
            twoDArray[0] = x_arr;
            twoDArray[1] = y_arr;
            return twoDArray;
        }
    }
}
