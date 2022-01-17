using System;

namespace Microsoft.Graph.Cli.Core.IO;

public class PathUtility : IPathUtility {
    public string GetUserHomeDirectory() {
        return Environment.GetEnvironmentVariable("HOME") ?? Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
    }
}