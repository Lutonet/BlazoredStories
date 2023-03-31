using System.Text.Json;
using Spectre.Console;
using Spectre.Console.Json;

public class Program
{
  public static void Main(string[] args)
  {
    var json = "{\"name\":\"John Smith\",\"age\":30,\"city\":\"New York\"}";
    var options = new JsonSerializerOptions { WriteIndented = true };
    var formattedJson = JsonSerializer.Serialize(JsonSerializer.Deserialize<object>(json), options);

    var jsonText = new JsonText(formattedJson);
    AnsiConsole.Render(jsonText);
  }
}