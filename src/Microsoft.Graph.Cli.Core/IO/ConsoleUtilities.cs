using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Graph.Cli.Core.IO;

public static class ConsoleUtilities
{
    public static async Task<string> ReadPasswordAsync(string? message = null, CancellationToken cancellationToken = default)
    {
        var pass = new StringBuilder();
        await Console.Out.WriteLineAsync(new ReadOnlyMemory<char>(message?.ToCharArray()), cancellationToken);
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            // Backspace Should Not Work
            var clearChars = new char[] { '\b', ' ', '\b' };
            if (!char.IsControl(key.KeyChar))
            {
                pass.Append(key.KeyChar);
                await Console.Out.WriteAsync(new ReadOnlyMemory<char>(new char[] { '*' }), cancellationToken);
            }
            else if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
            {
                pass.Remove(pass.Length - 1, 1);
                await Console.Out.WriteAsync(new ReadOnlyMemory<char>(clearChars), cancellationToken);
            }
            else if (key.Key == ConsoleKey.Escape && pass.Length > 0)
            {
                var length = pass.Length;
                pass.Clear();
                while (length > 0)
                {
                    await Console.Out.WriteAsync(new ReadOnlyMemory<char>(clearChars), cancellationToken);
                    length -= 1;
                }
            }
        }
        // Stops Receving Keys Once Enter is Pressed
        while (key.Key != ConsoleKey.Enter);
        return pass.ToString();
    }
}
