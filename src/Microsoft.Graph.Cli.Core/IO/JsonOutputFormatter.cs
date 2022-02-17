using System.CommandLine;
using Microsoft.Kiota.Cli.Commons.IO;
using Spectre.Console;

namespace Microsoft.Graph.Cli.Core.IO;

public class JsonOutputFormatter : IOutputFormatter
{
    public void WriteOutput(string content)
    {
        AnsiConsole.WriteLine(content);
    }

    public void WriteOutput(Stream content)
    {
        using var reader = new StreamReader(content);
        var strContent = reader.ReadToEnd();
        AnsiConsole.WriteLine(strContent);
    }
}
