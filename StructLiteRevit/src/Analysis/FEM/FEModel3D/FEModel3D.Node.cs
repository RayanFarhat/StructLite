using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StructLite.Analysis
{ 
    public partial class FEModel3D
    {
        /// <summary>
        /// Adds a new node to the model.
        /// </summary>
        ///   <param name="x">The node's global X-coordinate.</param>
        ///   <param name="y">The node's global Y-coordinate.</param>
        ///   <param name="z">The node's global Z-coordinate.</param>
        ///   <param name="name">A unique user-defined name for the node. If set to Null, a name will be automatically assigned.</param>
        public void AddNode(double x, double y, double z, string name = null)
        {
            // Name the node or check it doesn't already exist
            if (name != null)
            {
                if (Nodes.ContainsKey(name))
                {
                    throw new Exception($"Node name '{name}' already exists");
                }
            }
            else
            {
                // As a guess, start with the length of the dictionary
                name = "N" + Nodes.Count;
                var count = 1;
                while (Nodes.ContainsKey(name))
                {
                    name = "N" + Nodes.Count + count.ToString();
                    count++;
                }
            }
            // Create a new node
            var new_node = new Node3D(name, x, y, z);
            // Add the new node to the list
            Nodes.Add(name, new_node);
        }
        /// <summary>
        /// Removes a node from the model. All nodal loads associated with the node and elements attached to the node will also be removed.
        /// </summary>
        ///   <param name="name">The name of the node to be removed.</param>
        public void DeleteNode(string name)
        {
            // Remove the node. Nodal loads are stored within the node, so they
            // will be deleted automatically when the node is deleted.
            this.Nodes.Remove(name);

            // Find any elements attached to the node and remove them
            this.Members = Members.Where(pair => pair.Value.i_node.Name != name && pair.Value.j_node.Name != name)
                                    .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        /// <summary>
        /// Defines the support conditions at a node. Nodes will default to fully unsupported unless specified otherwise.
        /// </summary>
        ///   <param name="node_name">The name of the node where the support is being defined.</param>
        ///   <param name="support_DX">Indicates whether the node is supported against translation in the global X-direction.Defaults to False.</param>
        ///   <param name="support_DY">Indicates whether the node is supported against translation in the global Y-direction.Defaults to False.</param>
        ///   <param name="support_DZ">Indicates whether the node is supported against translation in the global Z-direction.Defaults to False.</param>
        ///   <param name="support_RX">Indicates whether the node is supported against rotation about the global X-axis.Defaults to False.</param>
        ///   <param name="support_RY">Indicates whether the node is supported against rotation about the global Y-axis.Defaults to False.</param>
        ///   <param name="support_RZ">Indicates whether the node is supported against rotation about the global Z-axis.Defaults to False.</param>
        public void def_support(string node_name, bool support_DX = false, bool support_DY = false, bool support_DZ = false,
            bool support_RX = false, bool support_RY = false, bool support_RZ = false)
        {
            Nodes[node_name].support_DX = support_DX;
            Nodes[node_name].support_DY = support_DY;
            Nodes[node_name].support_DZ = support_DZ;
            Nodes[node_name].support_RX = support_RX;
            Nodes[node_name].support_RY = support_RY;
            Nodes[node_name].support_RZ = support_RZ;
        }
        /// <summary>
        /// Adds a nodal load to the model.
        /// </summary>
        ///   <param name="node_name">The name of the node where the load is being applied.</param>
        ///   <param name="D"> The global direction the load is being applied in. Forces are `'FX'`,`'FY'`, and `'FZ'`. Moments are `'MX'`, `'MY'`, and `'MZ'`.</param>
        ///   <param name="P">The numeric value (magnitude) of the load.</param>
        ///   <param name="caseName">The name of the load case the load belongs to. Defaults to 'Case 1'.</param>
        public void Add_node_load(string node_name, Direction D, double P, string caseName = "Case 1")
        {
            if (D == Direction.Fx || D == Direction.Fy || D == Direction.Fz
                || D == Direction.Mx || D == Direction.My || D == Direction.Mz)
            {
                throw new Exception($"Direction must be 'FX', 'FY', 'FZ', 'MX', 'MY', or 'MZ'. {D} was given.");
            }
            // Add the node load to the model
            Nodes[node_name].NodeLoads.Add(new NodeLoad(D, P, caseName));
        }
        /// <summary>
        /// Defines a nodal displacement at a node.
        /// </summary>
        ///   <param name="node_name">The name of the node where the nodal displacement is being applied.</param>
        ///   <param name="D">The global direction the nodal displacement is being applied in. Displacements are 'FX', 'FY', and 'FZ'. Rotations are 'MX', 'MY', and 'MZ'.</param>
        ///   <param name="magnitude">The magnitude of the displacement.</param>
        public void def_node_disp(string node_name, Direction D, double magnitude)
        {
            if (D == Direction.FX)
            {
                Nodes[node_name].EnforcedDX = magnitude;
            }
            else if (D == Direction.FY)
            {
                Nodes[node_name].EnforcedDY = magnitude;
            }
            else if (D == Direction.FZ)
            {
                Nodes[node_name].EnforcedDZ = magnitude;
            }
            else if (D == Direction.MX)
            {
                Nodes[node_name].EnforcedRX = magnitude;
            }
            else if (D == Direction.MY)
            {
                Nodes[node_name].EnforcedRY = magnitude;
            }
            else if (D == Direction.MZ)
            {
                Nodes[node_name].EnforcedRZ = magnitude;
            }
            else
            {
                throw new Exception($"Direction must be 'FX', 'FY', 'FZ', 'RX', 'RY', or 'RZ'. {D} was given.");
            }
        }
        /// <summary>
        /// Returns a list of the names of nodes that are not attached to any elements.
        /// </summary>
        public List<Node3D> orphaned_nodes()
        {
            var orphans = new List<Node3D>();
            foreach (var node  in Nodes.Values)
            {
                // Check to see if the node is attached to any elements
                var tmpMembers = Members.Where(pair => pair.Value.i_node.Name == node.Name || pair.Value.j_node.Name == node.Name).ToList();
                //  Determine if the node is orphaned
                if (tmpMembers.Count == 0)
                {
                    orphans.Add(node);
                }
            }
            return orphans;
        }
    }
}
