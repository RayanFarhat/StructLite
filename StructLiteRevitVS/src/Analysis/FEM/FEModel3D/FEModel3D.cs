using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    /// <summary>
    ///  A 3D finite element model object. This object has methods and dictionaries to create, store,
    ///  and retrieve results from a finite element model.
    /// </summary>
    public partial class FEModel3D
    {

        #region Properties
        /// <summary>
        /// A dictionary of the model's nodes.
        /// </summary>
        public Dictionary<string, Node3D> Nodes { get; set; }
        /// <summary>
        /// A dictionary of the model's materials.
        /// </summary>
        public Dictionary<string, Material> Materials { get; set; }
        /// <summary>
        /// A dictionary of the model's load combinations.
        /// </summary>
        public Dictionary<string, LoadCombo> LoadCombos { get; set; }
        /// <summary>
        /// A dictionary of the model's physical members.
        /// </summary>
        public Dictionary<string, Member3D> Members { get; set; }
        /// <summary>
        /// A dictionary of the model's nodal displacements by load combination.
        /// </summary>
        public Dictionary<string, Vector> _D { get; set; }
        #endregion
        public FEModel3D() {
            Nodes = new Dictionary<string, Node3D>();
            Materials = new Dictionary<string, Material>();
            LoadCombos = new Dictionary<string, LoadCombo>();
            Members = new Dictionary<string, Member3D>();
            _D = new Dictionary<string, Vector>();
        }
    }
}
