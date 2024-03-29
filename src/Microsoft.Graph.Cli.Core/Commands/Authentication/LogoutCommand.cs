using System.CommandLine;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Cli.Core.Authentication;

namespace Microsoft.Graph.Cli.Core.Commands.Authentication;

/// <summary>
/// Logout command.
/// </summary>
public sealed class LogoutCommand : Command
{
    /// <summary>
    /// Creates a new logout command instance.
    /// </summary>
    public LogoutCommand() : base("logout", "Logout by deleting the stored session used by commands")
    {
        this.SetHandler(async (context) =>
        {
            var logoutService = context.BindingContext.GetRequiredService<LogoutService>();
            var cancellationToken = context.BindingContext.GetRequiredService<CancellationToken>();
            await logoutService.Logout(cancellationToken);
        });
    }
}
