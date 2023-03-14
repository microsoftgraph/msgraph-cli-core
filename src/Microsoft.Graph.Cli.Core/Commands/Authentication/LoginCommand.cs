using System.CommandLine;
using System.CommandLine.Builder;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Cli.Core.Authentication;
using Microsoft.Graph.Cli.Core.IO;
using Microsoft.Graph.Cli.Core.Utils;

namespace Microsoft.Graph.Cli.Core.Commands.Authentication;

public sealed class LoginCommand : Command
{
    private Option<string[]> scopesOption = new("--scopes", "The login scopes e.g. User.Read. Required scopes can be found in the docs linked against each verb (get, list, create...) command.")
    {
        Arity = ArgumentArity.ZeroOrMore,
        AllowMultipleArgumentsPerToken = true,
        IsRequired = false
    };
    private Option<string> clientIdOption = new("--client-id", "The client (application) id");

    private Option<string> tenantIdOption = new("--tenant-id", "The tenant (directory) id");

    private Option<string> certificateNameOption = new("--certificate-name", "The name of your certificate (e.g. CN=MyCertificate). The certificate will be retrieved from the current user's certificate store.");

    private Option<string> certificateThumbPrintOption = new("--certificate-thumb-print", "The thumbprint of your certificate. The certificate will be retrieved from the current user's certificate store.");

    private Option<AuthenticationStrategy> strategyOption = new("--strategy", () => Constants.defaultAuthStrategy);

    internal LoginCommand() : base("login", "Login and store the session for use in subsequent commands")
    {
        AddOption(scopesOption);
        AddOption(clientIdOption);
        AddOption(tenantIdOption);
        AddOption(certificateNameOption);
        AddOption(certificateThumbPrintOption);
        AddOption(strategyOption);
        this.SetHandler(async (context) =>
        {
            string[] scopes = context.ParseResult.GetValueForOption(scopesOption) ?? new string[] { };
            var clientId = context.ParseResult.GetValueForOption(clientIdOption);
            var tenantId = context.ParseResult.GetValueForOption(tenantIdOption);
            var certificateName = context.ParseResult.GetValueForOption(certificateNameOption);
            var certificateThumbPrint = context.ParseResult.GetValueForOption(certificateThumbPrintOption);
            var strategy = context.ParseResult.GetValueForOption(strategyOption);
            var cancellationToken = context.GetCancellationToken();

            var authUtil = context.BindingContext.GetRequiredService<IAuthenticationCacheManager>();
            var authSvcFactory = context.BindingContext.GetRequiredService<AuthenticationServiceFactory>();

            var authService = await authSvcFactory.GetAuthenticationServiceAsync(strategy, tenantId, clientId, certificateName, certificateThumbPrint, cancellationToken);
            await authService.LoginAsync(scopes, cancellationToken);
            await authUtil.SaveAuthenticationIdentifiersAsync(clientId, tenantId, certificateName, certificateThumbPrint, strategy, cancellationToken);
        });
    }

    public LoginCommand(CommandLineBuilder builder) : this()
    {
        builder?.UseHelp((ctx) =>
        {
            ctx.HelpBuilder.CustomizeSymbol(strategyOption, firstColumnText: (ctx) =>
            {
                return "--strategy <strategy>";
            }, secondColumnText: (ctx) =>
            {
                var builder = new StringBuilder($"The authentication strategy to use. [default: {Constants.defaultAuthStrategy}]\n\n  Available strateges:\n    ");
                builder.Append(nameof(AuthenticationStrategy.DeviceCode));
                builder.Append(":         Use a device code to log in.\n    ");
                builder.Append(nameof(AuthenticationStrategy.InteractiveBrowser));
                builder.Append(": Open a browser on this computer to log in.\n    ");
                builder.Append(nameof(AuthenticationStrategy.ClientCertificate));
                builder.Append(":  Use a certificate stored on this computer's certificate store to sign in. The certificate must have a private key available.\n    ");
                builder.Append(nameof(AuthenticationStrategy.Environment));
                builder.Append(":        Use values stored in environment variables to log in.");
                builder.Append($"\n      Supported environment variables:");
                builder.Append($"\n        {Constants.Environment.TenantId} ");
                builder.Append($"\n        {Constants.Environment.ClientId}");
                builder.Append($"\n        {Constants.Environment.ClientSecret}");
                builder.Append($"\n        {Constants.Environment.ClientCertificatePath}");
                builder.Append($"\n        {Constants.Environment.ClientCertificatePassword}");
                builder.Append($"\n        {Constants.Environment.ClientSendCertificateChain}\n\n");
                builder.Append("NOTE: The strategy value is not case sensitive. Therefore, --strategy DeviceCode and --strategy devicecode are equivalent.");
                return builder.ToString();
            });
        });
    }
}
