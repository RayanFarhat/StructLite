using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace StructLite.RevitAdapter
{
    public partial class Adapter
    {
        public static XYZ selectPoint()
        {
            return uidoc.Selection.PickPoint("Pick a point");
        }
        public static IList<Element> MultipleSelect()
        {
            // user must select before press the command
            List<Element> elements = new List<Element>();
            ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();
            foreach (ElementId id in selectedIds)
            {
                Element element = uidoc.Document.GetElement(id);
                elements.Add(element);
            }
            return elements;
        }
        public static Element SelectElement()
        {
            Reference pickedRef = uidoc.Selection.PickObject(ObjectType.Element, "Please select one element");
            if (pickedRef == null)
            {
                return null;
            }
            return doc.GetElement(pickedRef.ElementId);
        }
    }
}
