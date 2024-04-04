### csproj
* the start of file is `<Project Sdk="Microsoft.NET.Sdk.Web">` contains the tasks, targets and packages required to build and publish ASP .NET and ASP .NET Core web apps(including Microsoft.AspNetCore.OpenApi).
### SPA
* when we build our svelte app we copy the build dir it to our `wwwroot` folder in our servevr project.
* we use `app.UseStaticFiles();` and `app.MapFallbackToFile("index.html");` so when we everything that the api not support will mapped to the frontend.