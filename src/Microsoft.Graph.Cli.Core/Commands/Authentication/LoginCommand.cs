using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph.Cli.Core.Authentication;
using Microsoft.Graph.Cli.Core.IO;
using Microsoft.Graph.Cli.Core.Utils;
using System.CommandLine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Microsoft.Graph.Cli.Core.Commands.Authentication;

public class LoginCommand
{
    private AuthenticationServiceFactory authenticationServiceFactory;

    public LoginCommand(AuthenticationServiceFactory authenticationServiceFactory)
    {
        this.authenticationServiceFactory = authenticationServiceFactory;
    }

    public Command Build()
    {
        var loginCommand = new Command("login", "Login and store the session for use in subsequent commands");
        var scopesOption = new Option<string[]>("--scopes", "The login scopes e.g. User.Read")
        {
            Arity = ArgumentArity.ZeroOrMore,
            AllowMultipleArgumentsPerToken = true
        };
        scopesOption.IsRequired = false;
        loginCommand.AddOption(scopesOption);

        var clientIdOption = new Option<string>("--client-id", "The client id");
        loginCommand.AddOption(clientIdOption);

        var tenantIdOption = new Option<string>("--tenant-id", "The tenant id");
        loginCommand.AddOption(tenantIdOption);

        var certificateNameOption = new Option<string>("--certificate-name", "The name of your certificate. The certificate will be retrieved from the current user's certificate store.");
        loginCommand.AddOption(certificateNameOption);

        var certificatePathOption = new Option<FileInfo?>("--certificate-path", "Path to an X.509 certificate file");
        loginCommand.AddOption(certificatePathOption);

        var certificateThumbPrintOption = new Option<string>("--certificate-thumb-print", "The thumbprint of your certificate. The certificate will be retrieved from the current user's certificate store.");
        loginCommand.AddOption(certificateThumbPrintOption);

        var strategyOption = new Option<AuthenticationStrategy>("--strategy", () => Constants.defaultAuthStrategy, "The authentication strategy to use.");
        loginCommand.AddOption(strategyOption);

        loginCommand.SetHandler(async (context) =>
        {
            string[] scopes = context.ParseResult.GetValueForOption(scopesOption) ?? new string[] { };
            var clientId = context.ParseResult.GetValueForOption(clientIdOption);
            var tenantId = context.ParseResult.GetValueForOption(tenantIdOption);
            var certificateName = context.ParseResult.GetValueForOption(certificateNameOption);
            var certificateFilePath = context.ParseResult.GetValueForOption(certificatePathOption)?.FullName;
            var certificateThumbPrint = context.ParseResult.GetValueForOption(certificateThumbPrintOption);
            var strategy = context.ParseResult.GetValueForOption(strategyOption);
            var host = context.BindingContext.GetRequiredService<IHost>();
            var cancellationToken = context.BindingContext.GetRequiredService<CancellationToken>();

            var authUtil = host.Services.GetRequiredService<IAuthenticationCacheUtility>();
            string? password = null;
            if (!string.IsNullOrWhiteSpace(certificateFilePath) && File.Exists(certificateFilePath))
            {
                // Get the password for the file.
                password = await ConsoleUtilities.ReadPasswordAsync("You have provided a path to a private certificate file. Please enter a password for the file if any.", cancellationToken);
            }

            var authService = await this.authenticationServiceFactory.GetAuthenticationServiceAsync(strategy, tenantId, clientId, certificateName, certificateThumbPrint, certificateFilePath, password, cancellationToken);
            await authService.LoginAsync(scopes, cancellationToken);
            await authUtil.SaveAuthenticationIdentifiersAsync(clientId, tenantId, certificateName, certificateFilePath, certificateThumbPrint, strategy, cancellationToken);
        });

        return loginCommand;
    }
}
