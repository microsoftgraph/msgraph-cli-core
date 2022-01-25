using System.CommandLine;

namespace Microsoft.Graph.Cli.Core.IO;

public class JsonOutputFormatter : IOutputFormatter
{
    public void WriteOutput(string content, IConsole console)
    {
        console.WriteLine(content);
    }
}
