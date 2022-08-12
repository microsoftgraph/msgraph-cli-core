using Azure.Core.Diagnostics;
using Azure.Identity;
using DevLab.JmesPath;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph.Cli.Core.Authentication;
using Microsoft.Graph.Cli.Core.Commands.Authentication;
using Microsoft.Graph.Cli.Core.Configuration;
using Microsoft.Graph.Cli.Core.IO;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Authentication.Azure;
using Microsoft.Kiota.Cli.Commons.IO;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Microsoft.Graph.Cli
{

    class Program
    {
        static async Task<int> Main(string[] args)
        {
            // We don't have access to a built host yet. Get configuration settings using a configuration builder.
            // Required to set initial token credentials.
            var configBuilder = new ConfigurationBuilder();
            ConfigureAppConfiguration(configBuilder);
            var config = configBuilder.Build();

            var authSettings = config.GetSection(nameof(AuthenticationOptions)).Get<AuthenticationOptions>();
            var authServiceFactory = new AuthenticationServiceFactory(new PathUtility(), authSettings);
            AuthenticationStrategy authStrategy = authSettings?.Strategy ?? AuthenticationStrategy.DeviceCode;

            using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger(EventLevel.LogAlways);
            string? password = null;
            if (!string.IsNullOrWhiteSpace(authSettings?.ClientCertificatePath) && !args.Any(a => a == "login"))
            {
                // Ask for password
                password = ConsoleUtilities.ReadPassword("You have provided a path to a private certificate file. Please enter a password for the file if any.");
            }

            IAuthenticationProvider? authProvider = null;
            try
            {
                var credential = await authServiceFactory.GetTokenCredentialAsync(authStrategy, authSettings?.TenantId, authSettings?.ClientId, authSettings?.ClientCertificateName, authSettings?.ClientCertificateThumbPrint, authSettings?.ClientCertificatePath, password);
                authProvider = new AzureIdentityAuthenticationProvider(credential, new string[] { "graph.microsoft.com" });
            }
            catch (CryptographicException)
            {
                await Console.Error.WriteLineAsync("Could not initialize certificate authentication. Check that the password provided for the certificate is correct then try again.");
                return -1;
            }

            var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var options = new GraphClientOptions
            {
                GraphProductPrefix = "graph-cli",
                GraphServiceLibraryClientVersion = $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}",
                GraphServiceTargetVersion = "1.0"
            };

            var commands = new List<Command>();
            var loginCommand = new LoginCommand(authServiceFactory);
            commands.Add(loginCommand.Build());

            var logoutCommand = new LogoutCommand(new LogoutService());
            commands.Add(logoutCommand.Build());

            using var httpClient = GraphCliClientFactory.GetDefaultClient(options);
            var core = new HttpClientRequestAdapter(authProvider, httpClient: httpClient);
            commands.Add(UsersCommandBuilder.BuildUsersCommand(core));

            var builder = BuildCommandLine(commands).UseDefaults().UseHost(CreateHostBuilder);
            builder.AddMiddleware((invocation) =>
            {
                var host = invocation.GetHost();
                var outputFilter = host.Services.GetRequiredService<IOutputFilter>();
                var outputFormatterFactory = host.Services.GetRequiredService<IOutputFormatterFactory>();
                var pagingService = host.Services.GetRequiredService<IPagingService>();
                invocation.BindingContext.AddService(_ => outputFilter);
                invocation.BindingContext.AddService(_ => outputFormatterFactory);
                invocation.BindingContext.AddService(_ => pagingService);
            });
            builder.UseExceptionHandler((ex, context) =>
            {
                if (ex is AuthenticationRequiredException)
                {
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    context.Console.Error.WriteLine("Token acquisition failed. Run mgc login command first to get an access token.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    context.Console.Error.WriteLine(ex.Message);
                    context.Console.Error.WriteLine(ex.StackTrace);
                    Console.ResetColor();
                }
            });

            var parser = builder.Build();

            return await parser.InvokeAsync(args);
        }

        static CommandLineBuilder BuildCommandLine(IEnumerable<Command> commands)
        {
            var rootCommand = new RootCommand();
            rootCommand.Description = "Microsoft Graph CLI Core Sample";

            foreach (var command in commands)
            {
                rootCommand.AddCommand(command);
            }

            return new CommandLineBuilder(rootCommand);
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder().ConfigureHostConfiguration((configHost) =>
            {
                configHost.SetBasePath(Directory.GetCurrentDirectory());
            }).ConfigureAppConfiguration((ctx, config) =>
            {
                ConfigureAppConfiguration(config);
            }).ConfigureServices((ctx, services) =>
            {
                var authSection = ctx.Configuration.GetSection(nameof(AuthenticationOptions));
                services.Configure<AuthenticationOptions>(authSection);
                services.AddSingleton<IPathUtility, PathUtility>();
                services.AddSingleton<IAuthenticationCacheUtility, AuthenticationCacheUtility>();
                services.AddSingleton<IOutputFilter, JmesPathOutputFilter>();
                services.AddSingleton<JmesPath>();
                services.AddSingleton<IOutputFormatterFactory, OutputFormatterFactory>();
                services.AddSingleton<IPagingService, GraphODataPagingService>();
            });

        static void ConfigureAppConfiguration(IConfigurationBuilder builder)
        {
            builder.Sources.Clear();
            builder.AddJsonFile(Path.Combine(System.AppContext.BaseDirectory, "app-settings.json"), optional: true);
            var pathUtil = new PathUtility();
            var authCache = new AuthenticationCacheUtility(pathUtil);
            var dataDir = pathUtil.GetApplicationDataDirectory();
            var userConfigPath = Path.Combine(dataDir, "settings.json");
            builder.AddJsonFile(userConfigPath, optional: true);
            builder.AddJsonFile(authCache.GetAuthenticationCacheFilePath(), optional: true, reloadOnChange: true);
            builder.AddEnvironmentVariables(prefix: "MGC_");
        }
    }
}
