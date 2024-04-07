using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructLite.Mathematics;

namespace StructLite.Analysis
{
    /// <summary>
    ///  Physical members can detect internal nodes and subdivide themselves into sub-members at those nodes.
    /// </summary>
    public partial class PhysMember : Member3D
    {
        #region Main Properties
        /// <summary>
        /// list that contain the sub members of the physical member.
        /// </summary>
        public Dictionary<string,Member3D> Sub_Members { get; protected set; }
        #endregion
        public PhysMember(string name, Node3D i, Node3D j, string materialName, FEModel3D model,
            double Iy, double Iz, double Jx, double A) 
            : base(name, i, j, materialName, model, Iy, Iz, Jx, A)
        {
            Sub_Members = new Dictionary<string, Member3D>();
        }
    }
}
