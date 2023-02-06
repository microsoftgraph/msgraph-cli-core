using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Cli.Core.Authentication;
using System.CommandLine;
using System.Threading;

namespace Microsoft.Graph.Cli.Core.Commands.Authentication;

public class LogoutCommand
{
    public Command Build() {
        var logoutCommand = new Command("logout", "Logout by deleting the stored session used in commands");
        logoutCommand.SetHandler(async (context) =>
        {
            var logoutService = context.BindingContext.GetRequiredService<LogoutService>();
            var cancellationToken = context.BindingContext.GetRequiredService<CancellationToken>();
            await logoutService.Logout(cancellationToken);
        });

        return logoutCommand;
    }
}
