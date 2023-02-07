namespace Microsoft.Graph.Cli.Core.IO;

public interface IPathUtility
{
    string GetUserHomeDirectory();

    string GetApplicationDataDirectory();
}
