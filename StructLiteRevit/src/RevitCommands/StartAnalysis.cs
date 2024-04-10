using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructLite.RC;
using StructLite.RevitAdapter;

namespace StructLite.RevitCommands
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)]
    public class StartAnalysis : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData,
                  ref string message, ElementSet elements)
        {
            try
            {
                GuiServerHandler.StartGUI();
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Adapter.Init(commandData.Application);
                RCModel.Instance.SynchronizeModels();
                RCModel.Instance.FEModel.Analyze();
                UIAdapter.TextBoxes["combo name"].Value = RCModel.Instance.FEModel.LoadCombos.First().Key;
                TaskDialog.Show("StructLite", "Analysis is done.");
                TaskDialog.Show("StructLite", $"{RCModel.Instance.FEModel}");
            }
            catch (Exception e)
            {
                message = e.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }

            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
}
