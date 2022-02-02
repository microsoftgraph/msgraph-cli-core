namespace Microsoft.Graph.Cli.Core.IO;

public interface IOutputFormatterFactory
{
    IOutputFormatter GetFormatter(FormatterType formatterType);
}
