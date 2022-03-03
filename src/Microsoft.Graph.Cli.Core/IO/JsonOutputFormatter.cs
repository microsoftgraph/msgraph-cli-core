using System.CommandLine;
using System.Text;
using System.Text.Json;
using Microsoft.Kiota.Cli.Commons.IO;
using Spectre.Console;

namespace Microsoft.Graph.Cli.Core.IO;

public class JsonOutputFormatter : IOutputFormatter
{
    public void WriteOutput(string content, OutputFormatterOptions options)
    {
        if (options is JsonOutputFormatterOptions jsonOptions && jsonOptions.OutputIndented)
        {
            var result = this.ProcessJson(content, jsonOptions.OutputIndented);
            AnsiConsole.WriteLine(result);
        }
        else
        {
            AnsiConsole.WriteLine(content);
        }
    }

    public void WriteOutput(Stream content, OutputFormatterOptions options)
    {
        using var reader = new StreamReader(content);
        var strContent = reader.ReadToEnd();
        if (options is JsonOutputFormatterOptions jsonOptions && jsonOptions.OutputIndented)
        {
            var result = this.ProcessJson(strContent, jsonOptions.OutputIndented);
            AnsiConsole.WriteLine(result);
        }
        else
        {
            AnsiConsole.WriteLine(strContent);
        }
    }

    private string ProcessJson(string input, bool indent = true)
    {
        var result = input;
        try
        {
            var jsonDoc = JsonDocument.Parse(input);
            result = JsonSerializer.Serialize(jsonDoc, options: new() { WriteIndented = indent });
        }
        catch (JsonException)
        {
        }

        return result;
    }
}
