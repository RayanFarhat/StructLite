using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace StructLite
{
    public class GuiServerHandler
    {
        public static LocalData localData = new LocalData();
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
        public static void readData()
        {
            string jsonString = File.ReadAllText(@"localStorage.json");
            LocalData localData = JsonSerializer.Deserialize<LocalData>(jsonString);
            if (localData is null)
                throw new Exception("Error reading the saved changed!");
            GuiServerHandler.localData = localData;
        }
        public static void writeData()
        {
            string json = JsonSerializer.Serialize(localData);
            //File.WriteAllText(@"./GUIServer/localStorage.json", json);
            File.WriteAllText(@"localStorage.json", json);

        }
        public static void Shutdown()
        {
            Process[] pname = Process.GetProcessesByName("StructLiteRevitServer");
            if (pname.Length != 0)
            {
                foreach (var proc in pname)
                {
                    proc.Kill();
                    proc.Dispose();
                }
            }
        }
    }
    public class LocalData
    {
        // z axis
        public List<Point2D> ZShearData { get; set; } = new List<Point2D>();
        public List<Point2D> YMomentData { get; set; } = new List<Point2D>();
        public List<Point2D> ZDeflectionData { get; set; } = new List<Point2D>();
        // y axis
        public List<Point2D> YShearData { get; set; } = new List<Point2D>();
        public List<Point2D> ZMomentData { get; set; } = new List<Point2D>();
        public List<Point2D> YDeflectionData { get; set; } = new List<Point2D>();
        // x axis
        public List<Point2D> XDeflectionData { get; set; } = new List<Point2D>();
    }
    public struct Point2D
    {
        public Point2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public double x { get; set; }
        public double y { get; set; }
    }
}