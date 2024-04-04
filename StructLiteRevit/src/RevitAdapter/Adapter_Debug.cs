using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.RevitAdapter
{
    public partial class Adapter
    {
        public static void ShowParameters(Element e)
        {
            string str = "Parameters for " + e.Name + "\n";
            if (e is FamilyInstance)
            {
                var location = getMinPoint(e);
                str += $"at : X: {location.X}, Y: {location.Y}, Z: {location.Z}\n";
                var box = getWidthDepthHeight(e);
                str += $"Width: {box.X}\nDepth: {box.Y}\nHeight: {box.Z}\n";

            }

            foreach (Parameter para in e.Parameters)
            {
                str += para.Definition.Name + " : " + para.AsValueString() + "\n";
            }
            TaskDialog.Show("RYBIM Debug", str);
        }
        public static void ShowXYZs(Element e)
        {
            string str = "Vertices for " + e.Name + "\n";
            List<XYZ> vertices = GetXYZsFromElemnt(e);
            foreach (XYZ v in vertices)
            {
                str += $"X: {v.X}, Y: {v.Y}, Z: {v.Z}\n";
            }
            TaskDialog.Show("RYBIM Debug", str);
        }
    }
}
