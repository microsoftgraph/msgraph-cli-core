using System.Collections.Generic;

namespace Microsoft.Graph.Cli.Core.Configuration.Extensions;

public static class CommandArgumentsExtensions
{
    public static string[] ExpandFlagsForConfiguration(this string[] args)
    {
        // Supports providing bool options as flags. e.g. '--debug' instead of '--debug true'
        var argsSanitized = new List<string>();
        for (int i = 0; i < args.Length; ++i)
        {
            var curr = args[i];
            argsSanitized.Add(curr);
            if (!curr.StartsWith('-')) continue;
            string? next = null;
            if (i < args.Length - 1)
            {
                next = args[i + 1];
            }

            if (curr.StartsWith('-') && (next?.StartsWith('-') == true || i == args.Length - 1))
            {
                argsSanitized.Add("true");
            }
        }

        return argsSanitized.ToArray();
    }
}
