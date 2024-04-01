using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    /// <summary>
    ///  A mathematically continuous beam segment
    /// </summary>
    public partial class BeamSegZ
    {
        #region Main Properties
        /// <summary>
        /// The starting location of the segment relative to the start of the beam
        /// </summary>
        public double? x1 { get; set; } = null;
        /// <summary>
        /// The ending location of the segment relative to the start of the beam
        /// </summary>
        public double? x2 { get; set; } = null;
        /// <summary>
        /// The distributed load magnitude at the start of the segment
        /// </summary>
        public double? w1 { get; set; } = null;
        /// <summary>
        /// The distributed load magnitude at the end of the segment
        /// </summary>
        public double? w2 { get; set; } = null;
        /// <summary>
        /// The distributed axial load magnitude at the start of the segment
        /// </summary>
        public double? p1 { get; set; } = null;
        /// <summary>
        /// The distributed axial load magnitude at the end of the segment
        /// </summary>
        public double? p2 { get; set; } = null;
        /// <summary>
        /// The internal shear force at the start of the segment
        /// </summary>
        public double? V1 { get; set; } = null;
        /// <summary>
        /// The internal moment at the start of the segment
        /// </summary>
        public double? M1 { get; set; } = null;
        /// <summary>
        /// The internal axial force at the start of the segment
        /// </summary>
        public double? P1 { get; set; } = null;
        /// <summary>
        /// Torsional moment at start of segment
        /// </summary>
        public double? T1 { get; set; } = null;
        /// <summary>
        /// The slope (radians) at the start of the segment
        /// </summary>
        public double? theta1 { get; set; } = null;
        /// <summary>
        /// The transverse displacement at the start of the segment
        /// </summary>
        public double? delta1 { get; set; } = null;
        /// <summary>
        /// The axial displacement at the start of the segment
        /// </summary>
        public double? delta_x1 { get; set; } = null;
        /// <summary>
        /// The flexural stiffness of the segment
        /// </summary>
        public double? EI { get; set; } = null;
        /// <summary>
        /// The axial stiffness of the segment
        /// </summary>
        public double? EA { get; set; } = null;

        #endregion
        public BeamSegZ() { }

        public double Length()
        {
            if (x1 == null)
            {
                throw new ArgumentNullException(nameof(x1),"x1 of the segment is null");
            }
            else if (x2 == null)
            {
                throw new ArgumentNullException(nameof(x2), "x2 of the segment is null");
            }
            return (double)(x2 - x1);
        }
        /// <summary>
        /// Returns the slope of the elastic curve at any point `x` along the segment.
        /// </summary>
        public double Slope(double x)
        {
            // theta_1 - (-V1*x**2/2 - w1*x**3/6 + x*M1 + x**4*(w1 - w2)/(24*L))/EI
            return (double)(theta1 - (
                -V1*Math.Pow(x,2)/2 - w1*Math.Pow(x,3)/6 + x*M1 + Math.Pow(x, 4)*(w1-w2)/(24*Length())
                )/EI);
        }
    }
}
