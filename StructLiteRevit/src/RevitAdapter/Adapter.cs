using Autodesk.Revit.ApplicationServices;
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
        public static UIApplication uiapp;
        public static UIDocument uidoc;
        public static Application app;
        public static Document doc;
        public static bool IsUnitSystemMetric;
        public static void Init(UIApplication uiApplication)
        {
            uiapp = uiApplication;
            uidoc = uiapp.ActiveUIDocument;
            app = uiapp.Application;
            doc = uidoc.Document;
            IsUnitSystemMetric = doc.DisplayUnitSystem == DisplayUnit.METRIC? true: false;
        }
        public static double ConvertToM(double value) { return UnitUtils.ConvertFromInternalUnits(value, UnitTypeId.Meters); }
        public static double ConvertToFeet(double value) { return UnitUtils.ConvertFromInternalUnits(value, UnitTypeId.Feet); }
        public static double ConvertToXYZ(double value) { return UnitUtils.ConvertToInternalUnits(value, UnitTypeId.Meters); }
    }
}
