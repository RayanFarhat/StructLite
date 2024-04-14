All dev is on VSCODE, but i only use VS to apply code changes without restarting revit.
1- open vs and click run
2- dev on VSCODE.
3- open 'any' file in VS and save it and all the hot reload works !!!
* make sure that Tools > Options > Environment > Documents > Detect when file is changed outside the environment > Reload modified files unless there are unsaved changes. is ON.

### StructLiteRevitServer
copy from StructLiteRevitServer\bin\Release\net8.0\publish\ to GUIServer dir  after call `dotnet publish -c Release`