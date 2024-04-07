using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    public class Material
    {
        #region Properties
        public string Name { get; protected set; }
        /// <summary>
        /// The young's modulus of elasticity of the material
        /// </summary>
        public double E { get; protected set; }
        /// <summary>
        /// The shear modulus of the material
        /// </summary>
        public double G { get; protected set; }
        public double nu { get; protected set; }
        public double rho { get; protected set; }
        public double? fy { get; protected set; }

        #endregion
        /// <summary>
        /// contructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="E">young's modulus</param>
        /// <param name="G">shear modulus</param>
        /// <param name="nu"></param>
        /// <param name="rho"></param>
        /// <param name="fy"></param>
        public Material(string name, double E, double G, double nu, double rho, double? fy=null)
        {
            Name = name;
            this.E = E;
            this.G = G;
            this.nu = nu;
            this.rho = rho;
            this.fy = fy;
        }
    }
}
