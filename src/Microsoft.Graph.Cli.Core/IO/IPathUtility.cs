namespace Microsoft.Graph.Cli.Core.IO;

/// <summary>
/// Path utility interface.
/// </summary>
public interface IPathUtility
{
    /// <summary>
    /// Returns the user's HOME directory.
    /// </summary>
    /// <returns>The user's HOME directory.</returns>
    string GetUserHomeDirectory();

    /// <summary>
    /// Returns the APPDATA directory.
    /// </summary>
    /// <returns>The APPDATA directory.</returns>
    string GetApplicationDataDirectory();
}
