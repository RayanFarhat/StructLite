using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    /// <summary>
    ///  A mathematically continuous beam segment
    /// </summary>
    public partial class BeamSegY : BeamSegZ
    {
        /// <summary>
        ///   Returns the moment at a location 'x' on the segment.
        /// </summary>
        public new double Moment(double x)
        {
            // -M1 - V1*x - w1*x**2/2 - x**3*(-w1 + w2)/(6*L)
            return (double)(-M1 - V1 * x - w1 * Math.Pow(x, 2) / 2 - Math.Pow(x, 3) * (w2 - w1) / (6 * Length()));
        }
        /// <summary>
        /// Returns the slope of the elastic curve at any point `x` along the segment.
        /// </summary>
        public new double Slope(double x)
        {
            // theta_1 + (-V1*x**2/2 - w1*x**3/6 + x*(-M1) + x**4*(w1 - w2)/(24*L))/EI
            return (double)(theta1 + (
                -V1 * Math.Pow(x, 2) / 2 - w1 * Math.Pow(x, 3) / 6 + x * (-M1) + Math.Pow(x, 4) * (w1 - w2) / (24 * Length())
                ) / EI);
        }
        /// <summary>
        ///  Returns the deflection at a location 'x' on the segment.
        /// </summary>
        public new double Deflection(double x)
        {
            // delta_1 - theta_1*x + V1*x**3/(6*EI) + w1*x**4/(24*EI) - x**2*(-M1)/(2*EI) - x**5*(w1 - w2)/(120*EI*L)
            var def = (double)(delta1 - theta1 * x + V1*Math.Pow(x, 3)/(6*EI) + w1*Math.Pow(x, 4)/(24 * EI)
                        - Math.Pow(x, 2) * (-M1) / (2 * EI) + Math.Pow(x, 5)*(w2 - w1) / (120*EI*Length())
                );
            if (Length() == 0)
            {
                throw new Exception($"deflection is NaN, length = {Length()}");
            }
            return def;
        }
        /// <summary>
        ///  Returns the maximum moment in the segment.
        /// </summary>
        public new double Max_Moment()
        {
            // Find the quadratic equation parameters
            double a = (double)((w2 - w1) / (2 * Length()));
            double b = (double)(w1);
            double c = (double)(V1);
            double x1, x2;// not thr segment.x1 and segment.x2

            //  Determine possible locations of maximum moment
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
                //x2 = (-b - (b * *2 - 4 * a * c) * *0.5) / (2 * a)
                x1 = Math.Pow(-b + (Math.Pow(b, 2) - 4 * a * c), 0.5) / (2 * a);
                x2 = Math.Pow(-b - (Math.Pow(b, 2) - 4 * a * c), 0.5) / (2 * a);
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
            var M1 = Moment((double)x1);
            var M2 = Moment((double)x2);
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
        public new double Min_Moment()
        {
            // Find the quadratic equation parameters
            double a = (double)((w2 - w1) / (2 * Length()));
            double b = (double)(w1);
            double c = (double)(V1);
            double x1, x2;// not thr segment.x1 and segment.x2

            //  Determine possible locations of minimum moment
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
                // x2 = (-b-(b**2-4*a*c)**0.5)/(2*a)
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
            var M1 = Moment((double)x1);
            var M2 = Moment((double)x2);
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
