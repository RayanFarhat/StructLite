using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructLite.Mathematics;

namespace StructLite.Analysis
{
    public partial class BeamSegZ
    {
        /// <summary>
        ///   Returns the shear force at a location 'x' on the segment.
        /// </summary>
        public double Shear(double x)
        {
            return (double)(V1 + w1 * x + Math.Pow(x,2)*(-w1+w2)/(2*Length()));
        }
        /// <summary>
        ///  Returns the maximum shear in the segment.
        /// </summary>
        public double Max_Shear()
        {
            var w1 = (double)this.w1;
            var w2 = (double)this.w2;
            var x1 = (double)this.x1;
            var x2 = (double)this.x2;

            // Determine possible locations of maximum shear
            if (Num.IsEqual(w1 - w2, 0))
                x1 = 0;
            else
                x1 = w1*Length()/(w1-w2);
            //x1 < 0 or x1 > L
            if(Num.IsFirstSmallerThanSecond(x1,0) || Num.IsFirstBiggerThanSecond(x1,Length()))
                x1 = 0;
            x2 = 0;
            var x3 = Length();

            // Find the shear at each location of interest
            var V1 = Shear(x1);
            var V2 = Shear(x2);
            var V3 = Shear(x3);

            double max = V1;
            if (max < V2)
                max = V2;

            if (max < V3)
                max = V3;

            // Return the maximum shear
            return max;
        }
        /// <summary>
        ///  Returns the minimum shear in the segment.
        /// </summary>
        public double Min_Shear()
        {
            var w1 = (double)this.w1;
            var w2 = (double)this.w2;
            var x1 = (double)this.x1;
            var x2 = (double)this.x2;

            // Determine possible locations of maximum shear
            if (Num.IsEqual(w1 - w2, 0))
                x1 = 0;
            else
                x1 = w1 * Length() / (w1 - w2);
            //x1 < 0 or x1 > L
            if (Num.IsFirstSmallerThanSecond(x1, 0) || Num.IsFirstBiggerThanSecond(x1, Length()))
                x1 = 0;
            x2 = 0;
            var x3 = Length();

            // Find the shear at each location of interest
            var V1 = Shear(x1);
            var V2 = Shear(x2);
            var V3 = Shear(x3);

            double min = V1;
            if (min > V2)
                min = V2;

            if (min > V3)
                min = V3;

            // Return the maximum shear
            return min;
        }
    }
}
