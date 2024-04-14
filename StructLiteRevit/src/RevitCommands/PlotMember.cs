using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using StructLite.Analysis;
using StructLite.RC;
using StructLite.RevitAdapter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace StructLite.RevitCommands
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)]
    public class PlotMember : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData,
          ref string message, ElementSet elements)
        {
            try
            {
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Adapter.Init(commandData.Application);

                Element selectedElement = Adapter.SelectElement();
                if (selectedElement == null)
                {
                    message = "No element selected.";
                    return Result.Cancelled;
                }
                var elems = RCModel.Instance.Elements;
                var id = selectedElement.Id.ToString();
                string comboName = "";
                if (UIAdapter.TextBoxes["combo name"].Value is string value)
                    comboName = value.ToString();

                if (!RCModel.Instance.FEModel.LoadCombos.ContainsKey(comboName))
                {
                    TaskDialog.Show("StructLite Error", "Please put existing load combination!");
                    return Result.Failed;
                }
                // is element analytical member
                if (elems.ContainsKey(id))
                {
                    var m = RCModel.Instance.FEModel.Members[id];
                    plot(m, comboName);
                    return Result.Succeeded;
                }
                // is physical element
                if (elems.ContainsValue(id))
                {
                    var memberId = elems.FirstOrDefault(x => x.Value == id).Key;
                    var m = RCModel.Instance.FEModel.Members[memberId];
                    plot(m, comboName);
                    return Result.Succeeded;
                }
                TaskDialog.Show("StructLite Error", "Element is not related to the analytical model.");
                return Result.Failed;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
        }
        private void plot(Member3D m, string comboName)
        {
            m.plot_Deflection(Direction.Fx, comboName);

            m.plot_Shear(Direction.Fy, comboName);
            m.plot_Moment(Direction.Mz, comboName);
            m.plot_Deflection(Direction.Fy, comboName);

            m.plot_Shear(Direction.Fz, comboName);
            m.plot_Moment(Direction.My, comboName);
            m.plot_Deflection(Direction.Fz, comboName);

            GuiServerHandler.writeData();
            Process.Start(new ProcessStartInfo("http://localhost:5555") { UseShellExecute = true });
        }
    }
}
