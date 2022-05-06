using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Graph.Cli.Core.Authentication;
using Microsoft.Graph.Cli.Core.Configuration;
using Microsoft.Graph.Cli.Core.IO;
using Microsoft.Graph.Cli.Core.Utils;
using System.CommandLine;
using System.Threading;

namespace Microsoft.Graph.Cli.Core.Commands.Authentication;

public class LoginCommand
{
    private AuthenticationServiceFactory authenticationServiceFactory;

    public LoginCommand(AuthenticationServiceFactory authenticationServiceFactory) {
        this.authenticationServiceFactory = authenticationServiceFactory;
    }

    public Command Build() {
        var loginCommand = new Command("login", "Login and store the session for use in subsequent commands");
        var scopesOption = new Option<string[]>("--scopes", "The login scopes e.g. User.Read") {
            Arity = ArgumentArity.OneOrMore
        };
        scopesOption.IsRequired = false;
        loginCommand.AddOption(scopesOption);

        var clientIdOption = new Option<string>("--client-id", "The client id");
        loginCommand.AddOption(clientIdOption);

        var tenantIdOption = new Option<string>("--tenant-id", "The tenant id");
        loginCommand.AddOption(tenantIdOption);

        var strategyOption = new Option<AuthenticationStrategy>("--strategy", () => Constants.defaultAuthStrategy, "The authentication strategy to use.");
        loginCommand.AddOption(strategyOption);

        loginCommand.SetHandler<string[], string?, string?, AuthenticationStrategy, IHost, CancellationToken>(async (scopes, clientId, tenantId, strategy, host, cancellationToken) =>
        {
            var authUtil = host.Services.GetRequiredService<IAuthenticationCacheUtility>();
            var authService = await this.authenticationServiceFactory.GetAuthenticationServiceAsync(strategy, tenantId, clientId, cancellationToken);
            await authService.LoginAsync(scopes, cancellationToken);
            await authUtil.SaveAuthenticationIdentifiersAsync(clientId, tenantId, cancellationToken);
        }, scopesOption, clientIdOption, tenantIdOption, strategyOption);

        return loginCommand;
    }
}