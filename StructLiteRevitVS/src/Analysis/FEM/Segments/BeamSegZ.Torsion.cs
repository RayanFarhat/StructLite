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
        ///   Returns the torsional moment in the segment.
        /// </summary>
        public double Torsion()
        {
            // The torsional moment is constant across the segment
            // This can be updated in the future for distributed torsional forces
            return (double)T1;
        }
        /// <summary>
        ///   Returns the maximum torsional moment in the segment.
        /// </summary>
        public double Max_Torsion()
        {
            // The torsional moment is constant across the segment
            // Since the torsional moment is constant on the segment, the maximum torsional moment is T1
            // This can be updated in the future for distributed torsional forces
            return (double)T1;
        }
        /// <summary>
        ///   Returns the minimum torsional moment in the segment.
        /// </summary>
        public double Min_Torsion()
        {
            // The torsional moment is constant across the segment
            // Since the torsional moment is constant on the segment, the minimum torsional moment is T1
            // This can be updated in the future for distributed torsional forces
            return (double)T1;
        }
    }
}
