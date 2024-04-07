using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    public struct DistributedLoad
    {
        public DistributedLoad(Direction direction, double x1, double x2, double w1, double w2, string CaseName) : this()
        {
            this.direction = direction;
            this.x1 = x1;
            this.x2 = x2;
            this.w1 = w1;
            this.w2 = w2;
            this.CaseName = CaseName;
        }

        /// <summary>
        /// e global direction the load is being applied in.
        /// </summary>
        public Direction direction { get; set; }
        /// <summary>
        ///The start place of the load.
        /// </summary>
        public double x1 { get; set; }
        /// <summary>
        ///The end place of the load.
        /// </summary>
        public double x2 { get; set; }
        /// <summary>
        ///The start numeric value (magnitude) of the load from x1.
        /// </summary>
        public double w1 { get; set; }
        /// <summary>
        ///The end numeric value (magnitude) of the load to x2.
        /// </summary>
        public double w2 { get; set; }
        /// <summary>
        /// the load case name of the point load
        /// </summary>
        public string CaseName { get; set; }
    }
}
