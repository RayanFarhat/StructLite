using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StructLite.RevitAdapter;
using StructLite.Analysis;
using Autodesk.Revit.DB.Structure;
using StructLite.RC;
using System.Data.Common;
using System.IO;
using System.Diagnostics;

namespace StructLite
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Test : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData,
            ref string message, ElementSet elements)
        {
            try
            {
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Adapter.Init(commandData.Application);

                var model = new FEModel3D();
                model.AddNode(-23.079116, 9.779291, 9.84252, "n1");
                model.AddNode(-13.845813, 9.779291, 9.84252, "n2");
                model.add_material(29000.0, 11200, 0.3, 2.836e-4, null, "mat");
                model.AddMember("n1", "n2", "mat", 100, 100, 250, 20, "elem");
                model.def_support("n1", true, true, true, false, false, false);
                model.def_support("n2", true, true, true, true, false, false);

                var factors = new Dictionary<string, double>
                {
                    { "D", 1.4 }
                };
                // model.add_load_combo(factors, "1.4D");
                model.add_member_dist_load("elem", Direction.Fy, -1000, -3000, 2.223, 6.16);
                model.add_member_pt_load("elem", Direction.Fy, -1000, 7.3187);
                model.add_member_dist_load("elem", Direction.Fz, -1000, -3000, 2.223, 6.16);
                model.add_member_pt_load("elem", Direction.Fz, -1000, 7.3187);
                //model.Add_node_load("n2", Direction.FY, -100);

                model.Analyze();


                Units units = Adapter.doc.GetUnits();
                FormatOptions myFormat = units.GetFormatOptions(SpecTypeId.Force);
                ForgeTypeId mySymbol = myFormat.GetSymbolTypeId();
                string unitSymbol = LabelUtils.GetLabelForSymbol(mySymbol);
                string unitSymbol2 = LabelUtils.GetLabelForUnit(UnitTypeId.KilonewtonsPerMeter);
                //TaskDialog.Show("ss", $"{unitSymbol}");


                //model.Members["elem"].plot_Shear(Direction.Fy);

                //model.Members["elem"].plot_Moment(Direction.Mz);
                //model.Members["elem"].plot_Moment(Direction.My);
                //TaskDialog.Show("dd",$"mz {model.Members["elem"].Moment(Direction.Mz,4)}\n my {model.Members["elem"].Moment(Direction.My, 4)}");
                model.Members["elem"].plot_Deflection(Direction.Fy);
                //model.Members["elem"].plot_Deflection(Direction.Fz);

                //TaskDialog.Show("ccs",model.Members["elem"].Deflection(Direction.Fy,6).ToString());
                //model.Members["elem"].Deflection(Direction.Fy, 4);
                //TaskDialog.Show("ccs", model.Members["elem"].fer().ToString());

                using (Transaction transaction = new Transaction(Adapter.doc, "Create Curve"))
                {
                    transaction.Start();
                    transaction.Commit();
                }
            }

            catch (Exception e)
            {
                message = e.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }

            return Autodesk.Revit.UI.Result.Succeeded;
        }
        /// </ExampleMethod>
    }
    /// <remarks>
    /// This application's main class. The class must be Public.
    /// </remarks>
    public class Main : IExternalApplication
    {
        // Both OnStartup and OnShutdown must be implemented as public method
        public Result OnStartup(UIControlledApplication application)
        {
            string assembly = Assembly.GetExecutingAssembly().Location;
            UIAdapter.Init(application);
            UIAdapter.CreateTab("StructLite");

            // RectangularConcrete panel
            UIAdapter.AddPanel("Reading");
            UIAdapter.AddPushBtn("StartAnalysis", "Reading", "Analyze", "StructLite.RevitCommands.StartAnalysis",
                "Start the structural analysis after synchronizing the model.");

            UIAdapter.AddPanel("Plot Diagrams");

            UIAdapter.AddTextBox("combo name", "Plot Diagrams", "comboName", "enter used combo", "check your load combinations names and select one",
                "Put here the load combination thet you want to see the results of.");
            UIAdapter.AddPushBtn("PlotMember", "Plot Diagrams", "Plot Member", "StructLite.RevitCommands.PlotMember",
                "Select member and plot his diagrams.");

            UIAdapter.panels["Plot Diagrams"].AddSlideOut();

            RadioButtonGroupData radioData = new RadioButtonGroupData("radioGroup");
            RadioButtonGroup radioButtonGroup = UIAdapter.panels["Plot Diagrams"].AddItem(radioData) as RadioButtonGroup;
            ToggleButtonData tbX = new ToggleButtonData("toggleButtonX", "X");
            tbX.ToolTip = "Toggle to see the results on the Host Local X axis ";
            ToggleButtonData tbY = new ToggleButtonData("toggleButtonY", "Y");
            tbY.ToolTip = "Toggle to see the results on the Host Local Y axis";
            ToggleButtonData tbZ = new ToggleButtonData("toggleButtonZ", "Z");
            tbZ.ToolTip = "Toggle to see the results on the Host Local Z axis";
            radioButtonGroup.AddItem(tbZ);
            radioButtonGroup.AddItem(tbY);
            radioButtonGroup.AddItem(tbX);
            UIAdapter.AddRadioButtonGroup("axisRadio", radioButtonGroup);

            //text panel
            UIAdapter.AddPanel("Test");
            UIAdapter.AddPushBtn("Test", "Test", "btn", "StructLite.Test", "press this btn");

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            GuiServerHandler.Shutdown();
            return Result.Succeeded;
        }
    }
}
//TODO : equivalent frame method for flat slabs