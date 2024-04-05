

using System.Diagnostics;

Console.WriteLine("Hello, World!");
var proc = new Process();
proc.StartInfo.FileName = "C:\\Program Files\\Autodesk\\Revit 2024\\Revit.exe";
proc.StartInfo.Arguments = "C:\\Users\\ryanf\\Desktop\\Project1.rvt";
proc.Start();

proc.WaitForExit();
var exitCode = proc.ExitCode;
proc.Close();