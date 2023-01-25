using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Cli.Core.Authentication;
using System.CommandLine;
using System.Threading;

namespace Microsoft.Graph.Cli.Core.Commands.Authentication;

public class LogoutCommand
{
    private readonly LogoutService logoutService;

    public LogoutCommand(LogoutService logoutService) {
        this.logoutService = logoutService;
    }

    public Command Build() {
        var logoutCommand = new Command("logout", "Logout by deleting the stored session used in commands");
        logoutCommand.SetHandler(async (context) =>
        {
            var cancellationToken = context.BindingContext.GetRequiredService<CancellationToken>();
            await this.logoutService.Logout(cancellationToken);
        });

        return logoutCommand;
    }
}
