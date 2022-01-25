using System.CommandLine;

namespace Microsoft.Graph.Cli.Core.IO;

public interface IOutputFormatter
{
    void WriteOutput(string content, IConsole console);
}
