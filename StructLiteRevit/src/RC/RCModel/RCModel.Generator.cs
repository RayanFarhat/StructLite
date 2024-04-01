using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using StructLite.RevitAdapter;

namespace StructLite.RC
{
    public partial class RCModel
    {
        /// <summary>
        ///Generate Analytical Members from columnes and beams, after delete all other analytical members
        /// </summary>
        public void GenerateAnalyticalMembers()
        {
            FEModel.Nodes.Clear();
            Elements.Clear();
            var list = Adapter.getAllAnalyticalMembers();
            foreach (var mem in list)
            {
                mem.DeleteSubelements(mem.GetSubelements());
                Adapter.doc.Delete(mem.Id);
            }
            AddColsElems();
            AddBeamsElems();
        }
        private void AddBeamsElems()
        {
            var beams = Adapter.getAllBeams();

            foreach (var beam in beams)
            {
                var line = Adapter.getTwoPointsCurveOfElement(beam);
                bool foundSharedP1 = false;
                bool foundSharedP2 = false;
                var p1 = line.p1;
                var p2 = line.p2;
                var newP1 = p1;
                var newP2 = p2;
                foreach (var p in FEModel.Nodes.Values)
                {
                    if (isClose(p1, p))
                    {
                        newP1 = new XYZ(p.X, p.Y, p.Z);
                        foundSharedP1 = true;
                    }
                    else if (isClose(p2, p))
                    {
                        newP2 = new XYZ(p.X, p.Y, p.Z);
                        foundSharedP2 = true;
                    }
                }
                if (foundSharedP1 && foundSharedP2)
                {
                    var member = Adapter.CreateAnalyticalMember(newP1, newP2, AnalyticalStructuralRole.StructuralRoleColumn);
                    Elements.Add(member.Id.ToString(), beam.Id.ToString());
                }
                else if (foundSharedP1)
                {
                    newP2 = new XYZ(p2.X, p2.Y, newP1.Z);
                    addNode(newP2);
                    var member = Adapter.CreateAnalyticalMember(newP1, newP2, AnalyticalStructuralRole.StructuralRoleColumn);
                    Elements.Add(member.Id.ToString(), beam.Id.ToString());
                }
                else if (foundSharedP2)
                {
                    newP1 = new XYZ(p2.X, p2.Y, newP2.Z);
                    addNode(newP1);
                    var member = Adapter.CreateAnalyticalMember(newP1, newP2, AnalyticalStructuralRole.StructuralRoleColumn);
                    Elements.Add(member.Id.ToString(), beam.Id.ToString());
                }
            }
        }
        private void AddColsElems()
        {
            var columns = Adapter.getAllColumns();

            foreach (var col in columns)
            {
                if (Elements.Count == 0)
                {
                    var p = Adapter.getTwoPointsCurveOfElement(col);
                    var p1 = p.p1;
                    var p2 = p.p2;

                    addNode(p1);
                    addNode(p2);
                    var member = Adapter.CreateAnalyticalMember(p1, p2, AnalyticalStructuralRole.StructuralRoleColumn);
                    Elements.Add(member.Id.ToString(), col.Id.ToString());
                }
                // if there is existing elements, try finding shared node
                else
                {
                    bool found = false;
                    var line = Adapter.getTwoPointsCurveOfElement(col);
                    var p1 = line.p1;
                    var p2 = line.p2;
                    foreach (var p in FEModel.Nodes.Values)
                    {
                        if (isClose(p1, p))
                        {
                            var diffX = p.X - p1.X;
                            var diffY = p.Y - p1.Y;
                            var diffZ = p.Z - p1.Z;
                            var newP2 = new XYZ(diffX + p2.X, diffY + p2.Y, diffZ + p2.Z);
                            var member = Adapter.CreateAnalyticalMember(new XYZ(p.X, p.Y, p.Z), newP2, AnalyticalStructuralRole.StructuralRoleColumn);
                            Elements.Add(member.Id.ToString(), col.Id.ToString());
                            addNode(newP2);
                            found = true;
                            break;
                        }
                        else if (isClose(p2, p))
                        {
                            var diffX = p.X - p2.X;
                            var diffY = p.Y - p2.Y;
                            var diffZ = p.Z - p2.Z;
                            var newP1 = new XYZ(diffX + p1.X, diffY + p1.Y, diffZ + p1.Z);
                            var member = Adapter.CreateAnalyticalMember(newP1, new XYZ(p.X,p.Y,p.Z), AnalyticalStructuralRole.StructuralRoleColumn);
                            Elements.Add(member.Id.ToString(), col.Id.ToString());
                            addNode(newP1);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        addNode(p1);
                        addNode(p2);
                        var member = Adapter.CreateAnalyticalMember(p1, p2, AnalyticalStructuralRole.StructuralRoleColumn);
                        Elements.Add(member.Id.ToString(), col.Id.ToString());
                    }
                }
            }
        }
    }
}
