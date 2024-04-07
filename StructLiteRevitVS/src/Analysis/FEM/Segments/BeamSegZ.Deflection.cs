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
        ///  Returns the deflection at a location 'x' on the segment.
        /// </summary>
        public double Deflection(double x)
        {
            // delta_1 + theta_1*x + V1*x**3/(6*EI) + w1*x**4/(24*EI) + x**2*(-M1)/(2*EI) + x**5*(-w1 + w2)/(120*EI*L)
            var def = (double)(delta1 + theta1 * x + V1 * Math.Pow(x, 3) / (6 * EI) + w1 * Math.Pow(x, 4) / (24 * EI)
                        + Math.Pow(x, 2) * (-M1) / (2 * EI) + Math.Pow(x, 5) * (w2 - w1) / (120 * EI * Length())
                );
            if (Length() == 0)
            {
                throw new Exception($"deflection segZ is NaN, length = {Length()}, x1={x1}, x2={x2}");
            }
            return def;
        }
        /// <summary>
        ///   Returns the axial deflection at a location 'x' on the segment.
        /// </summary>
        public double AxialDeflection(double x)
        {
            // delta_x1 - 1/EA*(P1*x + p1*x**2/2 + (p2 - p1)*x**3/(6*L))
            return (double)(delta_x1 - 1/EA*(P1*x + p1*Math.Pow(x,2)/2.0 + (p2-p1)* Math.Pow(x, 3)/(6*Length())));
        }
    }
}
