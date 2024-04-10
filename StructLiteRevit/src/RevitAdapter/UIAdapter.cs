using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI.Events;
using System.Xml.Linq;

namespace StructLite.RevitAdapter
{
    // used in OnStartup funx inside IExternalApplication class
    public class UIAdapter
    {
        public static UIControlledApplication app;
        public static string tabName;
        public static Dictionary<string, RibbonPanel> panels = new Dictionary<string, RibbonPanel>();
        public static Dictionary<string, PushButton> PushButtons = new Dictionary<string, PushButton>();
        public static Dictionary<string, TextBox> TextBoxes = new Dictionary<string, TextBox>();
        public static Dictionary<string, RadioButtonGroup> RadioButtonGroups = new Dictionary<string, RadioButtonGroup>();
        public static void Init(UIControlledApplication uiControlledApplication)
        {
            app = uiControlledApplication;
        }
        public static void CreateTab(string tabName)
        {
            app.CreateRibbonTab(tabName);
            UIAdapter.tabName = tabName;
        }
        public static void AddPanel(string panelName)
        {
            panels.Add(panelName, app.CreateRibbonPanel(tabName, panelName));
        }
        public static void AddPushBtn(string btnName, string panelName, string title, string className, string tooltip = "")
        {
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            PushButtonData buttonData = new PushButtonData(title.Replace(" ", ""),
               title, thisAssemblyPath, className);
            PushButton pushButton = panels[panelName].AddItem(buttonData) as PushButton;
            if (tooltip != "")
            {
                pushButton.ToolTip = tooltip;
            }
            PushButtons.Add(btnName, pushButton);
        }
        // image be 32X32
        public static void AddPushBtnImage(string btnName, string panelName, string title, string className, string imgPath, string tooltip = "")
        {
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            PushButtonData buttonData = new PushButtonData(title.Replace(" ", ""),
               title, thisAssemblyPath, className);
            PushButton pushButton = panels[panelName].AddItem(buttonData) as PushButton;
            Uri uriImage = new Uri(imgPath);
            BitmapImage largeImage = new BitmapImage(uriImage);
            pushButton.LargeImage = largeImage;
            if (tooltip != "")
            {
                pushButton.ToolTip = tooltip;
            }
            PushButtons.Add(btnName, pushButton);
        }

        // in EnterPressEvent add  void ProcessText(object sender, Autodesk.Revit.UI.Events.TextBoxEnterPressedEventArgs args){ }
        public static void AddTextBox(string TextBoxName, string panelName, string name, string PromptText, string tooltip = "", string longDescription = "")
        {
            TextBoxData textData = new TextBoxData(name);
            if (tooltip != "")
            {
                textData.ToolTip = tooltip;
            }
            if (longDescription != "")
            {
                textData.LongDescription = longDescription;
            }
            TextBox tBox = panels[panelName].AddItem(textData) as TextBox;
            tBox.Width = 100;
            tBox.PromptText = PromptText;
            TextBoxes.Add(TextBoxName, tBox);
        }
        public static void AddRadioButtonGroup(string Name, RadioButtonGroup g)
        {
            RadioButtonGroups.Add(Name, g);
        }
    }
}
