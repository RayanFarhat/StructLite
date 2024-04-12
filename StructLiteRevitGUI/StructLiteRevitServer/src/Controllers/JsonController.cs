using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Text;

namespace GUIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class JsonController : ControllerBase
{

    public JsonController()
    {
    }

    [HttpGet()]
    public string Get()
    {
        string jsonString = "";
        try
        {
            jsonString = System.IO.File.ReadAllText(@"localStorage.json");
        }
        catch
        {
            System.IO.File.Delete(@"localStorage.json");
            System.Console.WriteLine("\nError reading the saved changed!\n");
        }
        return jsonString;
    }
    [HttpPost()]
    public string Post()
    {
        string jsonString = "";
        try
        {
            jsonString = System.IO.File.ReadAllText(@"localStorage.json");
        }
        catch
        {
            System.IO.File.Delete(@"localStorage.json");
            System.Console.WriteLine("\nError reading the saved changed!\n");
        }
        return jsonString;
    }
}
