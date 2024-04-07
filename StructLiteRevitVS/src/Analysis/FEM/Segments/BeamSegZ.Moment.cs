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
        ///   Returns the moment at a location 'x' on the segment.
        public double Moment(double x)
        {
            // M1 - V1*x - w1*x**2/2 - x**3*(-w1 + w2)/(6*L)
            return (double)(M1 - V1 * x - w1 * Math.Pow(x, 2) / 2 - Math.Pow(x, 3) * (w2 - w1) / (6 * Length()));
        }
        /// <summary>
        ///  Returns the maximum moment in the segment.
        /// </summary>
        public double Max_Moment()
        {
            // Find the quadratic equation parameters
            double a = (double)(-(w2 - w1) / (2 * Length()));
            double b = (double)(-w1);
            double c = (double)(-V1);
            double x1, x2;// not thr segment.x1 and segment.x2

            //  Determine possible locations of maximum moment
            if (Num.IsEqual(a,0))
            {
                if (!Num.IsEqual(b, 0))
                {
                    x1 = -c / b;
                }
                else
                    x1 = 0;
                x2 = 0;
            }
            else if (Num.IsFirstSmallerThanSecond(Math.Pow(b,2)-4*a*c,0))
            {
                x1 = 0;
                x2 = 0;
            }
            else
            {
                //  x1 = (-b+(b**2-4*a*c)**0.5)/(2*a)
                //  x2 = (-b-(b**2-4*a*c)**0.5)/(2*a)
                x1 = (-b + Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
                x2 = (-b - Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
            }
            //x1 < 0 or x1 > L
            if (Num.IsFirstSmallerThanSecond((double)x1, 0) || Num.IsFirstBiggerThanSecond((double)x1, Length()))
                x1 = 0;
            //x2 < 0 or x2 > L
            if (Num.IsFirstSmallerThanSecond((double)x2, 0) || Num.IsFirstBiggerThanSecond((double)x2, Length()))
                x2 = 0;

            double x3 = 0;
            double x4 = Length();

            // Find the moment at each location of interest
            var M1 = Moment(x1);
            var M2 = Moment(x2);
            var M3 = Moment(x3);
            var M4 = Moment(x4);
            //Return the maximum moment
            var max = M1;
            if (max < M2)
                max = M2;
            if (max < M3)
                max = M3;
            if (max < M4)
                max = M4;
            return max;
        }
        /// <summary>
        ///  Returns the minimum moment in the segment.
        /// </summary>
        public double Min_Moment()
        {
            // Find the quadratic equation parameters
            double a = (double)(-(w2 - w1) / (2 * Length()));
            double b = (double)(-w1);
            double c = (double)(-V1);
            double x1, x2;// not thr segment.x1 and segment.x2

            //  Determine possible locations of minimum  moment
            if (Num.IsEqual(a, 0))
            {
                if (!Num.IsEqual(b, 0))
                {
                    x1 = -c / b;
                }
                else
                    x1 = 0;
                x2 = 0;
            }
            else if (Num.IsFirstSmallerThanSecond(Math.Pow(b, 2) - 4 * a * c, 0))
            {
                x1 = 0;
                x2 = 0;
            }
            else
            {
                // x1 = (-b+(b**2-4*a*c)**0.5)/(2*a)
                // x2 = (-b - (b**2 - 4*a*c)**0.5) / (2 * a)
                x1 = (-b + Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
                x2 = (-b - Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
            }
            //x1 < 0 or x1 > L
            if (Num.IsFirstSmallerThanSecond((double)x1, 0) || Num.IsFirstBiggerThanSecond((double)x1, Length()))
                x1 = 0;
            //x2 < 0 or x2 > L
            if (Num.IsFirstSmallerThanSecond((double)x2, 0) || Num.IsFirstBiggerThanSecond((double)x2, Length()))
                x2 = 0;

            double x3 = 0;
            double x4 = Length();

            // Find the moment at each location of interest
            var M1 = Moment(x1);
            var M2 = Moment(x2);
            var M3 = Moment(x3);
            var M4 = Moment(x4);

            //Return the minimum moment
            var min = M1;
            if (min > M2)
                min = M2;
            if (min > M3)
                min = M3;
            if (min > M4)
                min = M4;
            return min;
        }
    }
}
