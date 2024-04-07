using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StructLite.Analysis
{
    /// <summary>
    /// A class representing a node in a 3D finite element model.
    /// </summary>
    public class Node3D
    {
        #region Properties
        /// <summary>
        /// A unique name for the node assigned by the user.
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// A unique index number for the node assigned by the program
        /// </summary>
        public int? ID { get; set; }
        /// <summary>
        /// Global X coordinate.
        /// </summary>
        public double X { get; protected set; }
        /// <summary>
        /// Global Y coordinate.
        /// </summary>
        public double Y { get; protected set; }
        /// <summary>
        /// Global Z coordinate.
        /// </summary>
        public double Z { get; protected set; }

        /// <summary>
        /// A list of loads applied to the node (Direction, P)
        /// </summary>
        public List<NodeLoad> NodeLoads { get; set; }

        public Dictionary<string, double> DX { get; set; }
        public Dictionary<string, double> DY { get; set; }
        public Dictionary<string, double> DZ { get; set; }
        public Dictionary<string, double> RX { get; set; }
        public Dictionary<string, double> RY { get; set; }
        public Dictionary<string, double> RZ { get; set; }
        public Dictionary<string, double> RxnFX { get; set; }
        public Dictionary<string, double> RxnFY { get; set; }
        public Dictionary<string, double> RxnFZ { get; set; }
        public Dictionary<string, double> RxnMX { get; set; }
        public Dictionary<string, double> RxnMY { get; set; }
        public Dictionary<string, double> RxnMZ { get; set; }
        public bool support_DX { get; set; }
        public bool support_DY { get; set; }
        public bool support_DZ { get; set; }
        public bool support_RX { get; set; }
        public bool support_RY { get; set; }
        public bool support_RZ { get; set; }

        public double? EnforcedDX { get; set; }
        public double? EnforcedDY { get; set; }
        public double? EnforcedDZ { get; set; }
        public double? EnforcedRX { get; set; }
        public double? EnforcedRY { get; set; }
        public double? EnforcedRZ { get; set; }

        /// <summary>
        /// used for contour smoothing.
        /// </summary>
        public List<double> Contour { get; protected set; }

        #endregion
        public Node3D(string name, double X, double Y, double Z) {
            this.Name = name;
            this.ID = null;
            this.X = X;
            this.Y = Y;
            this.Z = Z;

            NodeLoads = new List<NodeLoad>();

            // Initialize the dictionaries of calculated node displacements
            DX = new Dictionary<string, double>();
            DY = new Dictionary<string, double>();
            DZ = new Dictionary<string, double>();
            RX = new Dictionary<string, double>();
            RY = new Dictionary<string, double>();
            RZ = new Dictionary<string, double>();

            // Initialize the dictionaries of calculated node reactions
            RxnFX = new Dictionary<string, double>();
            RxnFY = new Dictionary<string, double>();
            RxnFZ = new Dictionary<string, double>();
            RxnMX = new Dictionary<string, double>();
            RxnMY = new Dictionary<string, double>();
            RxnMZ = new Dictionary<string, double>();

            // Initialize all support conditions to `False`
            support_DX = false; 
            support_DY = false;
            support_DZ = false;
            support_RX = false;
            support_RY = false;
            support_RZ = false;

            //  used when you don't know the magnitude of a force applied to a part,
            //  but you do know how much the part displaces as a result of that force.
            EnforcedDX = null;
            EnforcedDY = null;
            EnforcedDZ = null;
            EnforcedRX = null;
            EnforcedRY = null;
            EnforcedRZ = null;

            //Initialize the color contour value for the node.
            Contour = new List<double>();
        }

        /// <summary>
        /// Returns the distance to another node.
        /// </summary>
        public double Distance(Node3D other)
        {
            return Math.Pow(
                Math.Pow(this.X - other.X,2)+
                Math.Pow(this.Y - other.Y, 2) +
                Math.Pow(this.Z - other.Z, 2)
                , 0.5);
        }
        public bool isFixed()
        {
            if (support_DX == true && support_DY == true && support_DZ == true&&
                support_RX == true && support_RY == true && support_RZ == true)
            {
                return true;
            }
            return false;
        }
        public bool isFree()
        {
            if (support_DX == false && support_DY == false && support_DZ == false &&
                support_RX == false && support_RY == false && support_RZ == false)
            {
                return true;
            }
            return false;
        }
        public bool isPinned()
        {
            if (support_DX == true && support_DY == true && support_DZ == true && !isFixed())
            {
                return true;
            }
            return false;
        }
        public static bool operator ==(Node3D a, Node3D b)
        {
            if ((a as object) == null || (b as object) == null)
                return false;
            return a.Name == b.Name;
        }
        public static bool operator !=(Node3D a, Node3D b)
        {
            if ((a as object) == null || (b as object) == null)
                return false;
            return a.Name != b.Name;
        }
    }
}
