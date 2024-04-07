using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    public struct PointLoad
    {
        public PointLoad(Direction direction, double P, double X, string CaseName) : this()
        {
            this.direction = direction;
            this.P = P;
            this.X = X;
            this.CaseName = CaseName;
        }
        /// <summary>
        /// e global direction the load is being applied in.
        /// </summary>
        public Direction direction { get; set; }
        /// <summary>
        ///The numeric value (magnitude) of the load.
        /// </summary>
        public double P { get; set; }
        /// <summary>
        /// the place of the point load on the axial beam
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// the load case name of the point load
        /// </summary>
        public string CaseName { get; set; }
    }
}
