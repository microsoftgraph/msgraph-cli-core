using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Graph.Cli.Core.Authentication;
using Microsoft.Graph.Cli.Core.Configuration;
using Microsoft.Graph.Cli.Core.Utils;
using System.CommandLine;

namespace Microsoft.Graph.Cli.Core.Commands.Authentication;

public class LoginCommand
{
    private AuthenticationServiceFactory authenticationServiceFactory;

    public LoginCommand(AuthenticationServiceFactory authenticationServiceFactory) {
        this.authenticationServiceFactory = authenticationServiceFactory;
    }

    public Command Build() {
        var loginCommand = new Command("login", "Login and store the session for use in subsequent commands");
        var scopes = new Option<string[]>("--scopes", "The login scopes e.g. User.Read") {
            Arity = ArgumentArity.OneOrMore
        };
        scopes.IsRequired = true;
        loginCommand.AddOption(scopes);

        var strategy = new Option<AuthenticationStrategy>("--strategy", () => Constants.defaultAuthStrategy, "The authentication strategy to use.");
        loginCommand.AddOption(strategy);
        loginCommand.SetHandler<string[], AuthenticationStrategy, IHost, CancellationToken>(async (scopes, strategy, host, cancellationToken) =>
        {
            var options = host.Services.GetRequiredService<IOptionsMonitor<AuthenticationOptions>>().CurrentValue;
            var authService = await this.authenticationServiceFactory.GetAuthenticationServiceAsync(strategy, options?.TenantId, options?.ClientId, cancellationToken);
            await authService.LoginAsync(scopes, cancellationToken);
        }, scopes, strategy);

        return loginCommand;
    }
}