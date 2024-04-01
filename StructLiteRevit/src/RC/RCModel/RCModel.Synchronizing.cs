using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using StructLite.Analysis;
using StructLite.RevitAdapter;

namespace StructLite.RC
{
    public partial class RCModel
    {
        /// <summary>
        /// Ensure data between physical and analytical and StructLite elements is consistent and up-to-date.
        /// </summary>
        public void SynchronizeModels()
        {
            Elements.Clear();
            FEModel.Nodes.Clear();
            FEModel.Members.Clear();
            FEModel.Materials.Clear();
            FEModel.LoadCombos.Clear();
            addLoadCombos();
            addRevitMaterialsToFEModel();

            var members = Adapter.getAllAnalyticalMembers();
            var columns = Adapter.getAllColumns();
            var beams = Adapter.getAllBeams();
            var elements = columns.Concat(beams).ToList();

            if (elements.Count != members.Count)
            {
                throw new Exception("number of analytical members is not equal to the number of beams and columns");
            }
            foreach (var elem in elements)
            {
                var minDistance = Double.MaxValue;
                var closestMember = members[0];
                foreach (var mem in members)
                {
                    var p1 = mem.GetCurve().GetEndPoint(0);
                    var p2 = mem.GetCurve().GetEndPoint(1);
                    var centerMemberPoint = new XYZ((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
                    var distance = getDistance(centerMemberPoint, Adapter.getCentroid(elem));
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestMember = mem;
                    }
                }

                members.Remove(closestMember);// remove for faster search
                if (Elements.ContainsKey(closestMember.Id.ToString()))
                {
                    throw new Exception($"some member added two times for some reason.");
                }
                addElement(elem, closestMember);
            }

            // add Boundary Conditions to model nodes
            var boundaries = Adapter.getAllBoundaryConditions();
            foreach (var boundary in boundaries)
            {
                var nodeName = getNodeName(boundary.Point);
                if (FEModel.Nodes.ContainsKey(nodeName))
                {
                    bool support_DX = boundary.LookupParameter("X Translation").AsValueString() == "Fixed" ? true : false;
                    bool support_DY = boundary.LookupParameter("Y Translation").AsValueString() == "Fixed" ? true : false;
                    bool support_DZ = boundary.LookupParameter("Z Translation").AsValueString() == "Fixed" ? true : false;
                    bool support_RX = boundary.LookupParameter("X Rotation").AsValueString() == "Fixed" ? true : false;
                    bool support_RY = boundary.LookupParameter("Y Rotation").AsValueString() == "Fixed" ? true : false;
                    bool support_RZ = boundary.LookupParameter("Z Rotation").AsValueString() == "Fixed" ? true : false;
                    FEModel.def_support(nodeName, support_DX, support_DY, support_DZ, support_RX, support_RY, support_RZ);
                }
            }
            addAllLoads();
        }
    }
}
