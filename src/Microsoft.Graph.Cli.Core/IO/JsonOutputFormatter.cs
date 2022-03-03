using System.CommandLine;
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
            AnsiConsole.WriteLine(JsonSerializer.Serialize(content, options: new() { WriteIndented = true }));
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
            AnsiConsole.WriteLine(JsonSerializer.Serialize(strContent, options: new() { WriteIndented = true }));
        }
        else
        {
            AnsiConsole.WriteLine(strContent);
        }
    }
}
