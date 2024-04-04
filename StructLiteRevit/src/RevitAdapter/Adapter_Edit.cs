using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.RevitAdapter
{
    public partial class Adapter
    {
        public static void MoveByPoint(FamilyInstance elem,XYZ p)
        {
            LocationPoint elemPoint = elem.Location as LocationPoint;
            if (null != elemPoint)
            {
                elemPoint.Point = p;
            }
            else
            {
                TaskDialog.Show("s", "null");
            }
        }
        public static void MoveUsingLocationCurve(Element elem, XYZ translation)
        {
            //it will not mobe to translation point, it will move elem.X = elem.X + translation.X and ...
            LocationCurve locCurve = elem.Location as LocationCurve;
            if (null != locCurve)
            {
                locCurve.Move(translation);
            }
        }
        public static void MoveToLine(FamilyInstance elem, XYZ p1, XYZ p2)
        {
            // move all the element line to the new line
            LocationCurve locCurve = elem.Location as LocationCurve;
            if (null != locCurve)
            {
                Line newLocCurve = Line.CreateBound(p1, p2);
                locCurve.Curve = newLocCurve;
            }
        }
    }
}
