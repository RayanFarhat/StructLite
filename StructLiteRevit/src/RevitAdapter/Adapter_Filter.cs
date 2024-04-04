using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
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
        public static List<Element> getAllColumns()
        {
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns);
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            return collector.WherePasses(filter).WhereElementIsNotElementType().ToElements().ToList();
        }
        public static IList<Element> getAllBeams()
        {
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            return collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();
        }
        public static IList<Floor> getAllSlabs()
        {
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Floors);
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> slabs = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();
            IList<Floor> structuralSlabs = new List<Floor>();
            // filter only strutural slabs
            foreach (Element e in slabs)
            {
                if (e.get_Parameter(BuiltInParameter.FLOOR_PARAM_IS_STRUCTURAL).AsValueString() == "Yes")
                {
                    structuralSlabs.Add(e as Floor);
                }
            }
            return structuralSlabs;
        }
        public static IList<AnalyticalMember> getAllAnalyticalMembers()
        {
            FilteredElementCollector collectorAnalyticalLinks = new FilteredElementCollector(doc);
            var list = collectorAnalyticalLinks.OfClass(typeof(AnalyticalMember)).ToElements();
            return list.Cast<AnalyticalMember>().ToList();
        }
        public static AnalyticalMember GetAnalyticalMember(ElementId memberId)
        {
            IList<AnalyticalMember> analyticalMembers = getAllAnalyticalMembers();

            foreach (AnalyticalMember member in analyticalMembers)
            {
                if (member.Id == memberId)
                    return member;
            }
            return null;
        }
        public static IList<Material> getAllMaterials()
        {
            FilteredElementCollector collectorAnalyticalLinks = new FilteredElementCollector(doc);
            var list = collectorAnalyticalLinks.OfClass(typeof(Material)).ToElements();
            return list.Cast<Material>().ToList();
        }
        public static List<BoundaryConditions> getAllBoundaryConditions()
        {
            FilteredElementCollector collectorAnalyticalLinks = new FilteredElementCollector(doc);
            var list = collectorAnalyticalLinks.OfClass(typeof(BoundaryConditions)).ToElements();
            return list.Cast<BoundaryConditions>().ToList();
        }
        public static List<PointLoad> getAllPointLoads()
        {
            FilteredElementCollector collectorAnalyticalLinks = new FilteredElementCollector(doc);
            var list = collectorAnalyticalLinks.OfClass(typeof(PointLoad)).ToElements();
            return list.Cast<PointLoad>().ToList();
        }
        
        public static PointLoadType getPointLoadType()
        {
            FilteredElementCollector collectorAnalyticalLinks = new FilteredElementCollector(doc);
            return collectorAnalyticalLinks.OfClass(typeof(PointLoadType)).ToElements()[0] as PointLoadType;
        }
        public static List<LineLoad> getAllLineLoads()
        {
            FilteredElementCollector collectorAnalyticalLinks = new FilteredElementCollector(doc);
            var list = collectorAnalyticalLinks.OfClass(typeof(LineLoad)).ToElements();
            return list.Cast<LineLoad>().ToList();
        }
        public static LineLoadType getLineLoadType()
        {
            FilteredElementCollector collectorAnalyticalLinks = new FilteredElementCollector(doc);
            return collectorAnalyticalLinks.OfClass(typeof(LineLoadType)).ToElements()[0] as LineLoadType;
        }
        public static List<LoadCombination> getAllLoadCombinations()
        {
            FilteredElementCollector collectorAnalyticalLinks = new FilteredElementCollector(doc);
            var list = collectorAnalyticalLinks.OfClass(typeof(LoadCombination)).ToElements();
            return list.Cast<LoadCombination>().ToList();
        }
        public static List<LoadCase> getAllLoadCases()
        {
            FilteredElementCollector collectorAnalyticalLinks = new FilteredElementCollector(doc);
            var list = collectorAnalyticalLinks.OfClass(typeof(LoadCase)).ToElements();
            return list.Cast<LoadCase>().ToList();
        }
        public static FilteredElementCollector GetLevels()
        {
            return new FilteredElementCollector(Adapter.doc)
                    .WhereElementIsNotElementType()
                    .OfCategory(BuiltInCategory.INVALID)
                    .OfClass(typeof(Level));
        }
        public static List<Element> getConcreteRectangularColumnsSymbols()
        {
            return new FilteredElementCollector(doc)
                    .OfClass(typeof(FamilySymbol))
                    .OfCategory(BuiltInCategory.OST_StructuralColumns)
                    .Where(e => e.get_Parameter(BuiltInParameter.ALL_MODEL_FAMILY_NAME).AsValueString() == "Concrete-Rectangular-Column").ToList();
        }
        public static List<Element> getConcreteRectangularBeamsSymbols()
        {
            return new FilteredElementCollector(doc)
                    .OfClass(typeof(FamilySymbol))
                    .OfCategory(BuiltInCategory.OST_StructuralFraming)
                    .Where(e => e.get_Parameter(BuiltInParameter.ALL_MODEL_FAMILY_NAME).AsValueString() == "Concrete-Rectangular Beam").ToList();
        }
        public static List<Element> getConcreteFloorsSymbols()
        {
            return new FilteredElementCollector(doc)
                    .OfClass(typeof(FloorType))//same as FamilySymbol for other elements
                    .ToList();
        }
    }
}
