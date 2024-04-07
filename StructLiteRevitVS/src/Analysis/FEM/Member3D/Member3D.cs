using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    /// <summary>
    ///  A class representing a 3D frame element in a finite element model.(3d element)
    /// </summary>
    public partial class Member3D
    {
        #region Main Properties
        /// <summary>
        ///  A unique name for the member given by the user
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        ///  A unique index number for the member assigned by the program
        /// </summary>
        public int? ID { get; set; }
        public Node3D i_node { get; protected set; }
        public Node3D j_node { get; protected set; }

        /// <summary>
        ///  Members need a link to the model they belong to
        /// </summary>
        public FEModel3D Model { get; protected set; }
        /// <summary>
        ///  The current solved load combination
        /// </summary>
        public LoadCombo SolvedCombo { get; set; }

        #endregion
        #region Material Properties
        public string MaterialName { get; protected set; }
        /// <summary>
        ///  The Elastic modulus of the element
        /// </summary>
        public double E { get; protected set; }
        /// <summary>
        /// The Shear modulus of the element
        /// </summary>
        public double G { get; protected set; }
        #endregion
        #region Section Properties
        /// <summary>
        ///  The cross-sectional area
        /// </summary>
        public double A { get; protected set; }
        /// <summary>
        ///  The y-axis moment of inertia
        /// </summary>
        public double Iy { get; protected set; }
        /// <summary>
        ///  The z-axis moment of inertia
        /// </summary>
        public double Iz { get; protected set; }
        /// <summary>
        ///  The torsional constant
        /// </summary>
        public double Jx { get; protected set; }
        #endregion

        #region Loads Properties
        /// <summary>
        /// list that show the releaded degree of freedom.
        /// </summary>
        public List<bool> Releases { get; set; }

        /// <summary>
        /// A list of point loads applied to the member (Direction, P, X)
        /// </summary>
        public List<PointLoad> PtLoads { get; set; }
        /// <summary>
        /// A list of distributed loads applied to the member (Direction, w1,w2,x1,x2)
        /// </summary>
        public List<DistributedLoad> DistLoads { get; set; }
        /// <summary>
        /// A list of mathematically continuous beam segments for z-bending
        /// </summary>
        public List<BeamSegZ> SegmentsZ { get; protected set; }
        /// <summary>
        /// A list of mathematically continuous beam segments for y-bending
        /// </summary>
        public List<BeamSegY> SegmentsY { get; protected set; }
        /// <summary>
        /// A list of mathematically continuous beam segments for torsion
        /// </summary>
        public List<BeamSegZ> SegmentsX { get; protected set; }

        #endregion


        public Member3D(string name, Node3D i,Node3D j,string materialName,FEModel3D model,
            double Iy, double Iz, double Jx, double A) {
            this.Name = name;
            this.ID = null;
            this.Model = model;

            this.i_node = i;
            this.j_node = j;   
            //material
            this.MaterialName = materialName;
            if (model.Materials[materialName] == null)
            {
                throw new MemberAccessException($"{materialName} does not exist in the model.");
            }
            this.E = model.Materials[materialName].E;
            this.G = model.Materials[materialName].G;
            //define section
            this.A = A;
            this.Iy = Iy;
            this.Iz = Iz;
            this.Jx = Jx;
            // loads
            // order Dxi, Dyi, Dzi, Rxi, Ryi, Rzi, Dxj, Dyj, Dzj, Rxj, Ryj, Rzj
            Releases = new List<bool> {false,false, false, false, false, false, false, false, false, false, false, false };
            PtLoads = new List<PointLoad>();
            DistLoads = new List<DistributedLoad>();
            SegmentsZ = new List<BeamSegZ>();
            SegmentsY = new List<BeamSegY>();
            SegmentsX = new List<BeamSegZ>();

            // The 'Member3D' object will store results for one load combination at a time.
            // SolvedCombo variable will be used to track whether the member needs to be resegmented before running calculations for any given load combination.
            SolvedCombo = null;
        }

        /// <summary>
        /// Function that return the length of the member.
        /// </summary>
        /// <returns>length of the member</returns>
        public double L()
        {
            return this.i_node.Distance(this.j_node);
        }
        /// <summary>
        ///   Segment the member if necessary.
        /// </summary>
        private void Check_segments(string combo_name)
        {
            if (SolvedCombo == null || combo_name != SolvedCombo.Name)
            {
                Update_segments(combo_name);
                SolvedCombo = this.Model.LoadCombos[combo_name];
            }
        }
    }
}
