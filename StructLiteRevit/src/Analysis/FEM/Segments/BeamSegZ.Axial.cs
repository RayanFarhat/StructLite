using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    public partial class BeamSegZ
    {
        /// <summary>
        ///  Returns the axial force at a location 'x' on the segment.
        /// </summary>
        public double Axial(double x)
        {
            return (double)(P1 + (p2-p1)/(2*Length()) * Math.Pow(x,2) + p1*x);
        }
        /// <summary>
        ///  Returns the maximum axial force in the segment.
        /// </summary>
        public double max_axial()
        {
            var p1 = (double)this.p1;
            var p2 = (double)this.p2;
            var x1 = (double)this.x1;
            var x2 = (double)this.x2;

            // Determine possible locations of maximum axial force
            if (Num.IsEqual(p1 - p2, 0))
                x1 = 0;
            else
                x1 = p1 * Length() / (p1 - p2);
            //x1 < 0 or x1 > L
            if (Num.IsFirstSmallerThanSecond(x1, 0) || Num.IsFirstBiggerThanSecond(x1, Length()))
                x1 = 0;
            x2 = 0;
            var x3 = Length();

            // Find the axial force at each location of interest
            var P1 = Axial(x1);
            var P2 = Axial(x2);
            var P3 = Axial(x3);

            double max = P1;
            if (max < P2)
                max = P2;

            if (max < P3)
                max = P3;

            // Return the maximum shear
            return max;
        }
        /// <summary>
        ///  Returns the minimum axial force in the segment.
        /// </summary>
        public double min_axial()
        {
            var p1 = (double)this.p1;
            var p2 = (double)this.p2;
            var x1 = (double)this.x1;
            var x2 = (double)this.x2;

            // Determine possible locations of maximum axial force
            if (Num.IsEqual(p1 - p2, 0))
                x1 = 0;
            else
                x1 = p1 * Length() / (p1 - p2);
            //x1 < 0 or x1 > L
            if (Num.IsFirstSmallerThanSecond(x1, 0) || Num.IsFirstBiggerThanSecond(x1, Length()))
                x1 = 0;
            x2 = 0;
            var x3 = Length();

            // Find the axial force at each location of interest
            var P1 = Axial(x1);
            var P2 = Axial(x2);
            var P3 = Axial(x3);

            double min = P1;
            if (min > P2)
                min = P2;

            if (min > P3)
                min = P3;

            // Return the maximum shear
            return min;
        }
    }

}
