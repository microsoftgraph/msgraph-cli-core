using System;
using System.IO;
using Microsoft.Graph.Cli.Core.Utils;

namespace Microsoft.Graph.Cli.Core.IO;

public class PathUtility : IPathUtility
{
    public string GetUserHomeDirectory()
    {
        return Environment.GetEnvironmentVariable("HOME") ?? Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
    }

    public string GetApplicationDataDirectory()
    {
        var homeDir = this.GetUserHomeDirectory();
        var dataDir = Path.Combine(homeDir, Constants.ApplicationDataDirectory);

        Directory.CreateDirectory(dataDir);
        return dataDir;
    }
}