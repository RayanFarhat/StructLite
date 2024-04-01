using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    public partial class FEModel3D
    {
        /// <summary>
        /// Adds a new physical member to the model.
        /// </summary>
        ///   <param name="i_node_name">The name of the i-node (start node).</param>
        ///   <param name="j_node_name">The name of the j-node (end node).</param>
        ///   <param name="material_name">The name of the material of the member.</param>
        ///   <param name="Iy">The moment of inertia of the member about its local y-axis.</param>
        ///   <param name="Iz">The moment of inertia of the member about its local z-axis.</param>
        ///   <param name="J">The polar moment of inertia of the member.</param>
        ///   <param name="A">The cross-sectional area of the member.</param>
        ///   <param name="name">A unique user-defined name for the member. If set to Null, a name will be automatically assigned.</param>
        public void AddMember(string i_node_name, string j_node_name, string material_name, 
            double Iy, double Iz, double J, double A, string name = null )
        {
            // Name the member or check it doesn't already exist
            if (name != null)
            {
                if (Members.ContainsKey(name))
                {
                    throw new Exception($"Member name '{name}' already exists");
                }
            }
            else
            {
                // As a guess, start with the length of the dictionary
                name = "N" + Members.Count;
                var count = 1;
                while (Members.ContainsKey(name))
                {
                    name = "N" + Members.Count + count.ToString();
                    count++;
                }
            }
            var new_member = new PhysMember(name, Nodes[i_node_name], Nodes[j_node_name], material_name,
                this, Iy, Iz, J, A);
            this.Members.Add(name, new_member);
        }
        /// <summary>
        /// Removes a member from the model. All member loads associated with the member will also be removed.
        /// </summary>
        ///   <param name="name">The name of the member to be removed.</param>
        public void DeleteMember(string name)
        {
            // Remove the member. Member loads are stored within the member, so they
            // will be deleted automatically when the member is deleted.
            this.Members.Remove(name);
        }
        /// <summary>
        /// Defines member end realeses for a member. All member end releases will default to unreleased unless specified otherwise.
        /// </summary>
        ///   <param name="member_name">The name of the member to have its releases modified.</param>
        ///   <param name="Dxi">Indicates whether the member is released axially at its start. Defaults to False.</param>
        ///   <param name="Dyi">Indicates whether the member is released for shear in the local y-axis at its start. Defaults to False.</param>
        ///   <param name="Dzi">Indicates whether the member is released for shear in the local z-axis at its start. Defaults to False.</param>
        ///   <param name="Rxi">Indicates whether the member is released for torsion at its start. Defaults to False.</param>
        ///   <param name="Ryi">Indicates whether the member is released for moment about the local y-axis at its start. Defaults to False.</param>
        ///   <param name="Rzi">Indicates whether the member is released for moment about the local z-axis at its start. Defaults to False.</param>
        ///   <param name="Dxj">Indicates whether the member is released axially at its end. Defaults to False.</param>
        ///   <param name="Dyj">Indicates whether the member is released for shear in the local y-axis at its end. Defaults to False.</param>
        ///   <param name="Dzj">Indicates whether the member is released for shear in the local z-axis at its end. Defaults to False.</param>
        ///   <param name="Rxj">Indicates whether the member is released for torsion at its end. Defaults to False.</param>
        ///   <param name="Ryj">Indicates whether the member is released for moment about the local y-axis at its end. Defaults to False.</param>
        ///   <param name="Rzj">Indicates whether the member is released for moment about the local z-axis at its end. Defaults to False.</param>
        public void def_releases(string member_name, bool Dxi= false, bool Dyi = false, bool Dzi = false, bool Rxi = false, bool Ryi = false,
            bool Rzi = false, bool Dxj = false, bool Dyj = false, bool Dzj = false, bool Rxj = false, bool Ryj = false, bool Rzj = false)
        {
            this.Members[member_name].Releases = new List<bool> { Dxi, Dyi, Dzi, Rxi, Ryi, Rzi, Dxj, Dyj, Dzj, Rxj, Ryj, Rzj };
        }
        /// <summary>
        /// Adds a member point load to the model.
        /// </summary>
        ///   <param name="member_name">The name of the member the load is being applied to.</param>
        ///   <param name="D">The direction in which the load is to be applied. Valid values are `'Fx'`,
        ///    `'Fy'`, `'Fz'`, `'Mx'`, `'My'`, `'Mz'`, `'FX'`, `'FY'`, `'FZ'`, `'MX'`, `'MY'`, or `'MZ'`.
        ///   Note that lower-case notation indicates use of the beam's local
        ///   coordinate system, while upper-case indicates use of the model's globl coordinate system.</param>
        ///   <param name="P">The numeric value (magnitude) of the load.</param>
        ///   <param name="x">The load's location along the member's local x-axis.</param>
        ///   <param name="caseName">The name of the load case the load belongs to. Defaults to 'Case 1'.</param>
        public void add_member_pt_load(string member_name, Direction D, double P, double x, string caseName = "Case 1")
        {
            // Add the node load to the model
            Members[member_name].PtLoads.Add(new PointLoad(D,P,x,caseName));
        }
        /// <summary>
        /// Adds a member distributed load to the model.
        /// </summary>
        ///   <param name="member_name">The name of the member the load is being applied to.</param>
        ///   <param name="D">The direction in which the load is to be applied. Valid values are `'Fx'`,
        ///   `'Fy'`, `'Fz'`, `'FX'`, `'FY'`, or `'FZ'`.
        ///   Note that lower-case notation indicates use of the beam's local
        ///   coordinate system, while upper-case indicates use of the model's globl coordinate system.</param>
        ///   <param name="w1">The starting value (magnitude) of the load.</param>
        ///   <param name="w2">The ending value (magnitude) of the load.</param>
        ///   <param name="x1">The load's start location along the member's local x-axis. If this argument is not specified, the start of the member will be used.Defaults to `Null`</param>
        ///   <param name="x2">The load's end location along the member's local x-axis. If this argument is not specified, the end of the member will be used.Defaults to `Null`.</param>
        ///   <param name="caseName">The name of the load case the load belongs to. Defaults to 'Case 1'.</param>
        public void add_member_dist_load(string member_name, Direction D, double w1, double w2, double x1, double x2, string caseName = "Case 1")
        {
            if (D == Direction.MX || D == Direction.MY || D == Direction.MZ
                || D == Direction.Mx || D == Direction.My || D == Direction.Mz)
            {
                throw new Exception($"Direction must be 'Fx', 'Fy', 'Fz', 'FX', 'FY', or 'FZ'. {D} was given.");
            }

            // Add the distributed load to the member
            Members[member_name].DistLoads.Add(new DistributedLoad(D,x1,x2,w1,w2, caseName));
        }
        /// <summary>
        /// Adds self weight to all members in the model. Note that this only works for members.
        /// </summary>
        /// <param name="global_direction">The global direction to apply the member load in: 'FX', 'FY', or 'FZ'.</param>
        /// <param name="factor">A factor to apply to the member self-weight. Can be used to account for items like connections.</param>
        /// <param name="caseName">The load case to apply the self-weight to. Defaults to 'Case 1'</param>
        public void add_member_self_weight(Direction global_direction, double factor, string caseName = "Case 1")
        {
            // Step through each member in the model
            foreach (var member in this.Members.Values)
            {
                // Calculate the self weight of the member
                var self_weight = factor * this.Materials[member.MaterialName].rho * member.A;
                // Add the self-weight load to the member
                add_member_dist_load(member.Name,global_direction,self_weight,self_weight,0,member.L(),caseName);
            }
        }
    }
}
