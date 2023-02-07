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
            // If the current option is a flag and it's the last flag, we give it a value of true
            var addTrueFlagValue = i == args.Length - 1 ? true : args[i + 1].StartsWith('-');

            if (addTrueFlagValue)
            {
                argsSanitized.Add("true");
            }
        }

        return argsSanitized.ToArray();
    }
}
