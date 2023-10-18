using System;
using System.IO;
using Microsoft.Graph.Cli.Core.Utils;

namespace Microsoft.Graph.Cli.Core.IO;

/// <summary>
/// The path utility.
/// </summary>
public class PathUtility : IPathUtility
{
    /// <inheritdoc/>
    public string GetUserHomeDirectory()
    {
        return Environment.GetEnvironmentVariable("HOME") ?? Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
    }

    /// <inheritdoc/>
    public string GetApplicationDataDirectory()
    {
        var homeDir = this.GetUserHomeDirectory();
        var dataDir = Path.Combine(homeDir, Constants.ApplicationDataDirectory);

        Directory.CreateDirectory(dataDir);
        return dataDir;
    }
}
