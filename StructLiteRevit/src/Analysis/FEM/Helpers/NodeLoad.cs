using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    public struct NodeLoad
    {
        /// <summary>
        /// e global direction the load is being applied in.
        /// </summary>
        public Direction direction { get; set; }
        /// <summary>
        ///The numeric value (magnitude) of the load.
        /// </summary>
        public double Value { get; set; }
        /// <summary>
        ///The case name of the load.
        /// </summary>
        public string CaseName { get; set; }

        public NodeLoad(Direction direction, double value, string caseName)
        {
            this.direction = direction;
            this.Value = value;
            this.CaseName = caseName;
        }
    }
}
