using Microsoft.Kiota.Cli.Commons.IO;

namespace Microsoft.Graph.Cli.Core.IO;

public sealed class OutputFormatterFactory : IOutputFormatterFactory
{
    public IOutputFormatter GetFormatter(FormatterType formatterType)
    {
        switch (formatterType)
        {
            case FormatterType.JSON:
                return new JsonOutputFormatter();
            case FormatterType.TABLE:
                return new TableOutputFormatter();
            default:
                throw new System.NotSupportedException();
        }
    }

    public IOutputFormatter GetFormatter(string format) {
        FormatterType type;
        var success = Enum.TryParse<FormatterType>(format, true, out type);
        if (!success) {
            throw new NotSupportedException();
        }
        return this.GetFormatter(type);
    }
}
