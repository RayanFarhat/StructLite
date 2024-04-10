using System.Diagnostics;
using System.IO;
using System.Reflection;
using Autodesk.Revit.UI;

namespace StructLite
{
    public class GuiServerHandler
    {
        public static void StartGUI()
        {
            Process[] pname = Process.GetProcessesByName("StructLiteRevitServer");
            if (pname.Length == 0)
            {
                var proc = new Process();
                string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                proc.StartInfo.FileName = directory + "\\GUIServer\\StructLiteRevitServer.exe";
                proc.StartInfo.RedirectStandardOutput = false;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
            }
        }
    }
}