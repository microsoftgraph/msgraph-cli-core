namespace Microsoft.Graph.Cli.Core.IO;

public sealed class OutputFormatterFactory
{
    private static readonly Lazy<OutputFormatterFactory> instance = new Lazy<OutputFormatterFactory>(() => new OutputFormatterFactory());

    private OutputFormatterFactory()
    {
    }

    public static OutputFormatterFactory Instance
    {
        get
        {
            return instance.Value;
        }
    }

    public IOutputFormatter GetFormatter(FormatterType formatterType)
    {
        switch (formatterType)
        {
            case FormatterType.JSON:
                return new JsonOutputFormatter();
            default:
                throw new System.NotSupportedException();
        }
    }
}
