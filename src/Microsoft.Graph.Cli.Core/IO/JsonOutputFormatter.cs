using System.CommandLine;

namespace Microsoft.Graph.Cli.Core.IO;

public class JsonOutputFormatter : IOutputFormatter
{
    public void WriteOutput(string content, IConsole console)
    {
        console.WriteLine(content);
    }

    public void WriteOutput(Stream content, IConsole console)
    {
        using var reader = new StreamReader(content);
        var strContent = reader.ReadToEnd();
        console.WriteLine(strContent);
    }
}
