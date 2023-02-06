using Azure.Identity;
using DevLab.JmesPath;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph.Cli.Core.Authentication;
using Microsoft.Graph.Cli.Core.Commands.Authentication;
using Microsoft.Graph.Cli.Core.Configuration;
using Microsoft.Graph.Cli.Core.Configuration.Extensions;
using Microsoft.Graph.Cli.Core.Http;
using Microsoft.Graph.Cli.Core.IO;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Authentication.Azure;
using Microsoft.Kiota.Cli.Commons.IO;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.Graph.Cli
{
    class ServiceBinder<T> : BinderBase<T?> where T: class
    {
        protected override T? GetBoundValue(BindingContext ctx)
        {
            var host = ctx.GetRequiredService(typeof(IHost)) as IHost;

            return (ctx.GetService(typeof(T)) ?? host?.Services.GetService(typeof(T))) as T;
        }
    }

    public static class Extensions {
        public static CommandLineBuilder UseRequestAdapter(this CommandLineBuilder builder, Func<InvocationContext, IRequestAdapter> builderFactory) {
            builder.AddMiddleware(async (context, next) => {
                var requestAdapter = builderFactory.Invoke(context);
                if (requestAdapter != null) {
                    context.BindingContext.AddService(typeof(IRequestAdapter), p => requestAdapter);
                }
                
                // Log warning in case registration failed.
                await next(context);
            });
            return builder;
        }

        public static IRequestAdapter GetRequestAdapter(this BindingContext context) => context.GetService(typeof(IRequestAdapter)) as IRequestAdapter ??
                        throw new InvalidOperationException("IRequest adapter not found. Register a request adapter instance");
    }

    class Program
    {
        static async Task<int> Main(string[] args)
        {
            // We don't have access to a built host yet. Get configuration settings using a configuration builder.
            // Required to set initial token credentials.
            var commands = new List<Command>();
            var loginCommand = new LoginCommand();
            commands.Add(loginCommand.Build());

            var logoutCommand = new LogoutCommand();
            commands.Add(logoutCommand.Build());
            commands.Add(UsersCommandBuilder.BuildUsersCommand());
            var builder = BuildCommandLine(commands).UseDefaults().UseHost(CreateHostBuilder).UseRequestAdapter(ic => {
                var host = ic.GetHost();
                return host.Services.GetRequiredService<IRequestAdapter>();
            });
            builder.AddMiddleware((invocation) =>
            {
                var host = invocation.GetHost();

                var outputFilter = host.Services.GetRequiredService<IOutputFilter>();
                var outputFormatterFactory = host.Services.GetRequiredService<IOutputFormatterFactory>();
                var pagingService = host.Services.GetRequiredService<IPagingService>();
                
                invocation.BindingContext.AddService(_ => outputFilter);
                invocation.BindingContext.AddService(_ => outputFormatterFactory);
                invocation.BindingContext.AddService(_ => pagingService);
                invocation.BindingContext.AddService(_ => host.Services.GetRequiredService<IAuthenticationCacheUtility>());
                invocation.BindingContext.AddService(_ => host.Services.GetRequiredService<AuthenticationServiceFactory>());
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
                services.Configure<ExtraOptions>(op =>
                {
                    op.DebugEnabled = ctx.Configuration.GetValue<bool>("Debug");
                });
                services.AddSingleton<GraphClientOptions>(p =>
                {
                    var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
                    return new GraphClientOptions
                    {
                        GraphProductPrefix = "graph-cli",
                        GraphServiceLibraryClientVersion = $"{assemblyVersion?.Major ?? 0}.{assemblyVersion?.Minor ?? 0}.{assemblyVersion?.Build ?? 0}",
                        GraphServiceTargetVersion = "1.0"
                    };
                });
                services.AddSingleton<IAuthenticationProvider>(p => {
                    var authSettings = p.GetRequiredService<IOptions<AuthenticationOptions>>()?.Value;
                    var serviceFactory = p.GetRequiredService<AuthenticationServiceFactory>();
                    AuthenticationStrategy authStrategy = authSettings?.Strategy ?? AuthenticationStrategy.DeviceCode;
                    var credential = serviceFactory.GetTokenCredentialAsync(authStrategy, authSettings?.TenantId, authSettings?.ClientId, authSettings?.ClientCertificateName, authSettings?.ClientCertificateThumbPrint);
                    credential.Wait();
                    return new AzureIdentityAuthenticationProvider(credential.Result, new string[] { "graph.microsoft.com" });
                });
                services.AddHttpClient<IRequestAdapter, HttpClientRequestAdapter>((c, p) => {
                    GraphCliClientFactory.ConfigureClient(c);
                    var authProvider = p.GetRequiredService<IAuthenticationProvider>();
                    return new HttpClientRequestAdapter(authProvider, httpClient: c);
                }).ConfigurePrimaryHttpMessageHandler((p) => {
                    var options = p.GetRequiredService<GraphClientOptions>();
                    var logHandler = p.GetRequiredService<LoggingHandler>();
                    return GraphCliClientFactory.GetDefaultGraphHandler(options, lowestPriorityMiddlewares: new[] { logHandler });
                });
                services.AddSingleton<IPathUtility, PathUtility>();
                services.AddTransient<LoggingHandler>();
                services.AddSingleton<IAuthenticationCacheUtility, AuthenticationCacheUtility>();
                services.AddSingleton<AuthenticationServiceFactory>(p => {
                    var authSettings = p.GetRequiredService<IOptions<AuthenticationOptions>>()?.Value;
                    var pathUtil = p.GetRequiredService<IPathUtility>();
                    var cacheUtil = p.GetRequiredService<IAuthenticationCacheUtility>();
                    return new AuthenticationServiceFactory(pathUtil, cacheUtil, authSettings);
                });
                services.AddSingleton<IOutputFilter, JmesPathOutputFilter>();
                services.AddSingleton<JmesPath>();
                services.AddSingleton<IOutputFormatterFactory, OutputFormatterFactory>();
                services.AddSingleton<IPagingService, GraphODataPagingService>();
            }).ConfigureLogging((ctx, logBuilder) =>
            {
                logBuilder.SetMinimumLevel(LogLevel.Warning);
                var options = ctx.GetInvocationContext().BindingContext.GetService<IOptions<ExtraOptions>>();
                if (options?.Value?.DebugEnabled == true)
                {
                    logBuilder.AddFilter("Microsoft.Graph.Cli", LogLevel.Debug);
                }
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
            builder.AddCommandLine(args.ExpandFlagsForConfiguration());
        }
    }
}
