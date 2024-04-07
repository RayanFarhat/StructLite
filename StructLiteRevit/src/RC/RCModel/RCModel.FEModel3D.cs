using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using StructLite.Analysis;
using StructLite.Mathematics;
using StructLite.RevitAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace StructLite.RC
{
    public partial class RCModel
    {
        public void addAllLoads()
        {
            var ptLoads = Adapter.getAllPointLoads();
            foreach (var ptLoad in ptLoads)
            {
                var hostId = ptLoad.HostElementId.ToString();
                string nodeName = getNodeName(ptLoad.Point);
                // if node load
                if (FEModel.Nodes.ContainsKey(nodeName))
                {
                    if (ptLoad.ForceVector.X != 0)
                        FEModel.Add_node_load(nodeName, Direction.FX, ptLoad.ForceVector.X, ptLoad.LoadCaseName);
                    if (ptLoad.ForceVector.Y != 0)
                        FEModel.Add_node_load(nodeName, Direction.FY, ptLoad.ForceVector.Y, ptLoad.LoadCaseName);
                    if (ptLoad.ForceVector.Z != 0)
                        FEModel.Add_node_load(nodeName, Direction.FZ, ptLoad.ForceVector.Z, ptLoad.LoadCaseName);
                    if (ptLoad.MomentVector.X != 0)
                        FEModel.Add_node_load(nodeName, Direction.MX, ptLoad.MomentVector.X, ptLoad.LoadCaseName);
                    if (ptLoad.MomentVector.Y != 0)
                        FEModel.Add_node_load(nodeName, Direction.MY, ptLoad.MomentVector.Y, ptLoad.LoadCaseName);
                    if (ptLoad.MomentVector.Z != 0)
                        FEModel.Add_node_load(nodeName, Direction.MZ, ptLoad.MomentVector.Z, ptLoad.LoadCaseName);
                }
                else if (FEModel.Members.ContainsKey(hostId))
                {
                    var localXPosition = getDistance(getPointInMetric(ptLoad.Point), FEModel.Members[hostId].i_node);
                    localXPosition = Math.Round(localXPosition, 6);

                    if (ptLoad.OrientTo == LoadOrientTo.Project)
                    {
                        if (ptLoad.ForceVector.X != 0)
                            FEModel.add_member_pt_load(hostId, Direction.FX, ptLoad.ForceVector.X, localXPosition, ptLoad.LoadCaseName);
                        if (ptLoad.ForceVector.Y != 0)
                            FEModel.add_member_pt_load(hostId, Direction.FY, ptLoad.ForceVector.Y, localXPosition, ptLoad.LoadCaseName);
                        if (ptLoad.ForceVector.Z != 0)
                            FEModel.add_member_pt_load(hostId, Direction.FZ, ptLoad.ForceVector.Z, localXPosition, ptLoad.LoadCaseName);
                        if (ptLoad.MomentVector.X != 0)
                            FEModel.add_member_pt_load(hostId, Direction.MX, ptLoad.MomentVector.X, localXPosition, ptLoad.LoadCaseName);
                        if (ptLoad.MomentVector.Y != 0)
                            FEModel.add_member_pt_load(hostId, Direction.MY, ptLoad.MomentVector.Y, localXPosition, ptLoad.LoadCaseName);
                        if (ptLoad.MomentVector.Z != 0)
                            FEModel.add_member_pt_load(hostId, Direction.MZ, ptLoad.MomentVector.Z, localXPosition, ptLoad.LoadCaseName);
                    }
                    else
                    {
                        if (ptLoad.ForceVector.X != 0)
                            FEModel.add_member_pt_load(hostId, Direction.Fx, ptLoad.ForceVector.X, localXPosition, ptLoad.LoadCaseName);
                        if (ptLoad.ForceVector.Y != 0)
                            FEModel.add_member_pt_load(hostId, Direction.Fy, ptLoad.ForceVector.Y, localXPosition, ptLoad.LoadCaseName);
                        if (ptLoad.ForceVector.Z != 0)
                            FEModel.add_member_pt_load(hostId, Direction.Fz, ptLoad.ForceVector.Z, localXPosition, ptLoad.LoadCaseName);
                        if (ptLoad.MomentVector.X != 0)
                            FEModel.add_member_pt_load(hostId, Direction.Mx, ptLoad.MomentVector.X, localXPosition, ptLoad.LoadCaseName);
                        if (ptLoad.MomentVector.Y != 0)
                            FEModel.add_member_pt_load(hostId, Direction.My, ptLoad.MomentVector.Y, localXPosition, ptLoad.LoadCaseName);
                        if (ptLoad.MomentVector.Z != 0)
                            FEModel.add_member_pt_load(hostId, Direction.Mz, ptLoad.MomentVector.Z, localXPosition, ptLoad.LoadCaseName);
                    }
                }
            }
            var distLoads = Adapter.getAllLineLoads();
            foreach (var distLoad in distLoads)
            {
                var hostId = distLoad.HostElementId.ToString();
                if (FEModel.Members.ContainsKey(hostId))
                {
                    var localX1Position = getDistance(getPointInMetric(distLoad.StartPoint), FEModel.Members[hostId].i_node);
                    var localX2Position = getDistance(getPointInMetric(distLoad.EndPoint), FEModel.Members[hostId].i_node);
                    localX1Position = Math.Round(localX1Position, 6);
                    localX2Position = Math.Round(localX2Position, 6);

                    var forceVector1 = distLoad.ForceVector1;
                    var forceVector2 = distLoad.ForceVector2;
                    if (localX1Position > localX2Position)
                    {
                        var tmp = localX1Position;
                        localX1Position = localX2Position;
                        localX2Position = tmp;
                        forceVector1 = distLoad.ForceVector2;
                        forceVector2 = distLoad.ForceVector1;
                    }
                    if (distLoad.OrientTo == LoadOrientTo.Project)
                    {
                        if (forceVector1.X != 0 || forceVector2.X != 0)
                            FEModel.add_member_dist_load(hostId, Direction.FX, forceVector1.X, forceVector2.X, localX1Position, localX2Position, distLoad.LoadCaseName);
                        if (forceVector1.Y != 0 || forceVector2.Y != 0)
                            FEModel.add_member_dist_load(hostId, Direction.FY, forceVector1.Y, forceVector2.Y, localX1Position, localX2Position, distLoad.LoadCaseName);
                        if (forceVector1.Z != 0 || forceVector2.Z != 0)
                            FEModel.add_member_dist_load(hostId, Direction.FZ, forceVector1.Z, forceVector2.Z, localX1Position, localX2Position, distLoad.LoadCaseName);
                    }
                    else
                    {
                        if (forceVector1.X != 0 || forceVector2.X != 0)
                            FEModel.add_member_dist_load(hostId, Direction.Fx, forceVector1.X, forceVector2.X, localX1Position, localX2Position, distLoad.LoadCaseName);
                        if (forceVector1.Y != 0 || forceVector2.Y != 0)
                            FEModel.add_member_dist_load(hostId, Direction.Fy, forceVector1.Y, forceVector2.Y, localX1Position, localX2Position, distLoad.LoadCaseName);
                        if (forceVector1.Z != 0 || forceVector2.Z != 0)
                            FEModel.add_member_dist_load(hostId, Direction.Fz, forceVector1.Z, forceVector2.Z, localX1Position, localX2Position, distLoad.LoadCaseName);
                    }
                }
            }
        }
        public void addElement(Element elem, AnalyticalMember member)
        {
            Elements.Add(member.Id.ToString(), elem.Id.ToString());
            var n1 = member.GetCurve().GetEndPoint(0);
            var n2 = member.GetCurve().GetEndPoint(1);
            addNode(n1);
            addNode(n2);
            string elemMaterialName = elem.get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM).AsValueString();

            ElementType type = Adapter.doc.GetElement(elem.GetTypeId()) as ElementType;
            Parameter hPar = type.LookupParameter("h");
            double h = hPar.AsDouble();// in beam, h is on the z axis direction
            Parameter bPar = type.LookupParameter("b");
            double b = bPar.AsDouble();
            if (Adapter.IsUnitSystemMetric)
            {
                h = Adapter.ConvertToM(h);
                b = Adapter.ConvertToM(b);
            }
            else
            {
                h = Adapter.ConvertToFeet(h);
                b = Adapter.ConvertToFeet(b);
            }
            double A = h * b;//doesn't matter for beams as they dont get axial loads
            //Iy, Iz doesn't matter for columns as they dont get lateral loads
            double Iy = b * Math.Pow(h, 3) / 12;
            double Iz = h * Math.Pow(b, 3) / 12;
            double L;// length of long edge
            double S;// length of short edge
            if (b > h)
            {
                L = b;
                S = h;
            }
            else
            {
                L = h;
                S = b;
            }
            var JLastPart = 1 - (Math.Pow(S, 4) / (12 * Math.Pow(L, 4)));
            double J = L * Math.Pow(S, 3) * ((1.0 / 3.0) - (0.21 * S / L) * JLastPart);

            var FiniteMember = new StructLite.Analysis.Member3D(member.Id.ToString(), FEModel.Nodes[getNodeName(n1)], FEModel.Nodes[getNodeName(n2)],
                elemMaterialName, FEModel, Iy, Iz, J, A);
            FEModel.Members.Add(member.Id.ToString(), FiniteMember);
        }
        public void addRevitMaterialsToFEModel()
        {
            var materials = Adapter.getAllMaterials();
            foreach (var mat in materials)
            {
                var pse = Adapter.doc.GetElement(mat.StructuralAssetId) as PropertySetElement;
                if (pse == null)
                    continue;
                var props = pse.GetStructuralAsset();
                var ryMat = new StructLite.Analysis.Material(mat.Name, props.YoungModulus.Y, props.ShearModulus.Y, 0, 0);
                if (FEModel.Materials.ContainsKey(mat.Name))
                {
                    throw new Exception("StructLite Error, added same material twice!");
                }
                FEModel.Materials.Add(mat.Name, ryMat);
            }
        }
        public void addLoadCombos()
        {
            var combos = Adapter.getAllLoadCombinations();
            var cases = Adapter.getAllLoadCases();
            if (combos.Count == 0)
            {
                TaskDialog.Show("StructLite Error", "there is no Load Combination specified.");
            }
            foreach (var combo in combos)
            {
                var factors = new Dictionary<string, double>();
                // iterate over each case and his factor
                foreach (var component in combo.GetComponents())
                {
                    foreach (var loadCase in cases)
                    {
                        if (loadCase.Id == component.LoadCaseOrCombinationId)
                        {
                            factors.Add(loadCase.Name, component.Factor);
                        }
                    }
                }
                FEModel.add_load_combo(factors, combo.Name);
            }
        }
        /// <summary>
        /// get node name that inside the FEModel nodes
        /// </summary>
        /// <param name="p">3D point</param>
        /// <returns></returns>
        public string getNodeName(XYZ p)
        {
            var nodeX = p.X;
            var nodeY = p.Y;
            var nodeZ = p.Z;
            if (Adapter.IsUnitSystemMetric)
            {
                nodeX = Adapter.ConvertToM(nodeX);
                nodeY = Adapter.ConvertToM(nodeY);
                nodeZ = Adapter.ConvertToM(nodeZ);
            }
            else
            {
                nodeX = Adapter.ConvertToFeet(nodeX);
                nodeY = Adapter.ConvertToFeet(nodeY);
                nodeZ = Adapter.ConvertToFeet(nodeZ);
            }
            nodeX = Math.Round(nodeX, 6);
            nodeY = Math.Round(nodeY, 6);
            nodeZ = Math.Round(nodeZ, 6);
            var xStr = nodeX.ToString();
            var yStr = nodeY.ToString();
            var zStr = nodeZ.ToString();
            return xStr + "," + yStr + "," + zStr;
        }
        /// <summary>
        /// Add node to the FEModel
        /// </summary>
        /// <param name="p">The 3D point to add</param>
        public void addNode(XYZ p)
        {
            var nodeName = getNodeName(p);
            // if node already exist then doont add it
            if (FEModel.Nodes.ContainsKey(nodeName))
            {
                return;
            }
            var nodeX = p.X;
            var nodeY = p.Y;
            var nodeZ = p.Z;
            if (Adapter.IsUnitSystemMetric)
            {
                nodeX = Adapter.ConvertToM(nodeX);
                nodeY = Adapter.ConvertToM(nodeY);
                nodeZ = Adapter.ConvertToM(nodeZ);
            }
            else
            {
                nodeX = Adapter.ConvertToFeet(nodeX);
                nodeY = Adapter.ConvertToFeet(nodeY);
                nodeZ = Adapter.ConvertToFeet(nodeZ);
            }
            nodeX = Math.Round(nodeX, 6);
            nodeY = Math.Round(nodeY, 6);
            nodeZ = Math.Round(nodeZ, 6);
            var node = new Node3D(nodeName, nodeX, nodeY, nodeZ);
            FEModel.Nodes.Add(nodeName, node);
        }
        public XYZ getPointInMetric(XYZ p)
        {
            var nodeX = p.X;
            var nodeY = p.Y;
            var nodeZ = p.Z;
            if (Adapter.IsUnitSystemMetric)
            {
                nodeX = Adapter.ConvertToM(nodeX);
                nodeY = Adapter.ConvertToM(nodeY);
                nodeZ = Adapter.ConvertToM(nodeZ);
            }
            else
            {
                nodeX = Adapter.ConvertToFeet(nodeX);
                nodeY = Adapter.ConvertToFeet(nodeY);
                nodeZ = Adapter.ConvertToFeet(nodeZ);
            }
            nodeX = Math.Round(nodeX, 6);
            nodeY = Math.Round(nodeY, 6);
            nodeZ = Math.Round(nodeZ, 6);
            return new XYZ(nodeX, nodeY, nodeZ);
        }
        /// <summary>
        /// check distance between two points is less than 50cm
        /// </summary>
        /// <param name="p1">first point</param>
        /// <param name="p2">second point</param>
        /// <returns>True if two point close by 50cm,False else</returns>
        public bool isClose(XYZ p1, XYZ p2)
        {
            return Math.Pow(
                Math.Pow(p1.X - p2.X, 2) +
                Math.Pow(p1.Y - p2.Y, 2) +
                Math.Pow(p1.Z - p2.Z, 2)
                , 0.5) < Adapter.ConvertToXYZ(500);
        }
        /// <summary>
        /// check distance between two points is less than 50cm
        /// </summary>
        /// <param name="p">3D point</param>
        /// <param name="n">Node3D</param>
        /// <returns>True if two point close by 50cm,False else</returns>
        public bool isClose(XYZ p, Node3D n)
        {
            return Math.Pow(
                Math.Pow(p.X - n.X, 2) +
                Math.Pow(p.Y - n.Y, 2) +
                Math.Pow(p.Z - n.Z, 2)
                , 0.5) < Adapter.ConvertToXYZ(500);
        }
        public double getDistance(XYZ p1, XYZ p2)
        {
            return Math.Sqrt(
               Math.Pow(p1.X - p2.X, 2) +
               Math.Pow(p1.Y - p2.Y, 2) +
               Math.Pow(p1.Z - p2.Z, 2));
        }
        public double getDistance(XYZ p, Node3D n)
        {
            return Math.Sqrt(
               Math.Pow(p.X - n.X, 2) +
               Math.Pow(p.Y - n.Y, 2) +
               Math.Pow(p.Z - n.Z, 2));
        }
    }
}
