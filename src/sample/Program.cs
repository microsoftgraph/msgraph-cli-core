﻿using Azure.Core.Diagnostics;
using Azure.Identity;
using DevLab.JmesPath;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Cli.Core.Authentication;
using Microsoft.Graph.Cli.Core.Commands.Authentication;
using Microsoft.Graph.Cli.Core.Configuration;
using Microsoft.Graph.Cli.Core.Http;
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
            ConfigureAppConfiguration(configBuilder, args);
            var config = configBuilder.Build();

            var authSettings = config.GetSection(nameof(AuthenticationOptions)).Get<AuthenticationOptions>();
            var extraOpt = new CommandArgumentsExtensions {
                DebugEnabled = config.GetValue<bool>("Debug")
            };
            var pathUtil = new PathUtility();
            var cacheUtility = new AuthenticationCacheUtility(pathUtil);
            var authServiceFactory = new AuthenticationServiceFactory(pathUtil, cacheUtility, authSettings);
            AuthenticationStrategy authStrategy = authSettings?.Strategy ?? AuthenticationStrategy.DeviceCode;

            using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger(extraOpt.DebugEnabled ? EventLevel.LogAlways : EventLevel.Warning);
            var credential = await authServiceFactory.GetTokenCredentialAsync(authStrategy, authSettings?.TenantId, authSettings?.ClientId, authSettings?.ClientCertificateName, authSettings?.ClientCertificateThumbPrint);
            IAuthenticationProvider? authProvider = new AzureIdentityAuthenticationProvider(credential, new string[] { "graph.microsoft.com" });

            var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var options = new GraphClientOptions
            {
                GraphProductPrefix = "graph-cli",
                GraphServiceLibraryClientVersion = $"{assemblyVersion?.Major ?? 0}.{assemblyVersion?.Minor ?? 0}.{assemblyVersion?.Build ?? 0}",
                GraphServiceTargetVersion = "1.0"
            };

            var commands = new List<Command>();
            var loginCommand = new LoginCommand(authServiceFactory);
            commands.Add(loginCommand.Build());

            var authCacheUtil = new AuthenticationCacheUtility(pathUtil);
            var logoutCommand = new LogoutCommand(new LogoutService(authCacheUtil));
            commands.Add(logoutCommand.Build());

            var loggingHandler = new LoggingHandler();
            using var httpClient = GraphCliClientFactory.GetDefaultClient(options, lowestPriorityMiddlewares: new[] { loggingHandler });
            var core = new HttpClientRequestAdapter(authProvider, httpClient: httpClient);
            commands.Add(UsersCommandBuilder.BuildUsersCommand(core));

            var builder = BuildCommandLine(commands).UseDefaults().UseHost(CreateHostBuilder);
            builder.AddMiddleware((invocation) =>
            {
                var host = invocation.GetHost();
                
                if (extraOpt?.DebugEnabled == true) {
                    loggingHandler.Logger = host.Services.GetService<ILogger<LoggingHandler>>();
                }

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
                    Console.ResetColor();
                }

                context.ExitCode = -1;
            });

            var parser = builder.Build();

            return await parser.InvokeAsync(args);
        }

        static CommandLineBuilder BuildCommandLine(IEnumerable<Command> commands)
        {
            var rootCommand = new RootCommand();
            rootCommand.Description = "Microsoft Graph CLI Core Sample";
            // Support specifying additional arguments as configuration arguments
            // When there's a conflict, both the configuration and the command line
            // option will be set.
            rootCommand.TreatUnmatchedTokensAsErrors = false;

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
                ConfigureAppConfiguration(config, args);
            }).ConfigureServices((ctx, services) =>
            {
                var authSection = ctx.Configuration.GetSection(nameof(AuthenticationOptions));
                services.Configure<AuthenticationOptions>(authSection);
                services.Configure<CommandArgumentsExtensions>(op => {
                    op.DebugEnabled = ctx.Configuration.GetValue<bool>("Debug");
                });
                services.AddSingleton<IPathUtility, PathUtility>();
                services.AddSingleton<IAuthenticationCacheUtility, AuthenticationCacheUtility>();
                services.AddSingleton<IOutputFilter, JmesPathOutputFilter>();
                services.AddSingleton<JmesPath>();
                services.AddSingleton<IOutputFormatterFactory, OutputFormatterFactory>();
                services.AddSingleton<IPagingService, GraphODataPagingService>();
            });

        static void ConfigureAppConfiguration(IConfigurationBuilder builder, string[] args)
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
            builder.AddCommandLine(GetExpandedCommandFlags(args));
        }

        static string[] GetExpandedCommandFlags(in string[] args) {
            // Supports providing bool options as flags. e.g. '--debug' instead of '--debug true'
            var argsSanitized = new List<string>();
            for (int i = 0; i < args.Length; ++i)
            {
                var curr = args[i];
                argsSanitized.Add(curr);
                if (!curr.StartsWith('-')) continue;
                string? next = null;
                if (i < args.Length - 1) {
                    next = args[i + 1];
                }

                if (curr.StartsWith('-') && (next?.StartsWith('-') == true || i == args.Length - 1)) {
                    argsSanitized.Add("true");
                }
            }

            return argsSanitized.ToArray();
        }
    }
}
