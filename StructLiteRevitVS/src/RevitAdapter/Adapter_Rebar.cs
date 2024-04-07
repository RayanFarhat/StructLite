using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.RevitAdapter
{
    public partial class Adapter
    {
        public static RebarContainer CreateRebarContainer(Element conccrete)
        {
            // Create a new rebar container
            ElementId defaultRebarContainerTypeId = RebarContainerType.CreateDefaultRebarContainerType(doc);
            RebarContainer container = RebarContainer.Create(doc, conccrete as FamilyInstance, defaultRebarContainerTypeId);

            // Any items for this container should be presented in schedules and tags as separate subelements
            container.PresentItemsAsSubelements = true;

            return container;
        }

    }
}
