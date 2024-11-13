using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Graph.Cli.Core.Http;
using Microsoft.Graph.Cli.Core.Http.UriReplacement;
using Microsoft.Kiota.Http.HttpClientLibrary.Middleware;

namespace Microsoft.Graph.Cli.Core.IO;

/// <summary>
/// HTTP client factory for the graph CLI.
/// </summary>
public static class GraphCliClientFactory
{
    /// <summary>
    /// Gets default middlewares.
    /// </summary>
    /// <param name="options">Client options.</param>
    /// <returns>Returns default middlewares.</returns>
    public static IEnumerable<DelegatingHandler> GetDefaultMiddlewaresWithOptions(GraphClientOptions? options) => GraphClientFactory.CreateDefaultHandlers(options);

    /// <summary>
    /// Creates a default client for use in the Microsoft Graph CLI.
    /// </summary>
    /// <param name="options">Graph client options</param>
    /// <param name="version">API version</param>
    /// <param name="environment">The cloud environment in use.</param>
    /// <param name="finalHandler">Final HTTP handler</param>
    /// <param name="loggingHandler">Logging handler</param>
    /// <param name="middlewares">Other middlewares.</param>
    /// <returns>Returns a new HTTP client.</returns>
    public static HttpClient GetDefaultClient(GraphClientOptions? options = null, string version = "v1.0", CloudEnvironment environment = CloudEnvironment.Global, HttpMessageHandler? finalHandler = null, LoggingHandler? loggingHandler = null, params DelegatingHandler[] middlewares)
    {
        var m = new List<DelegatingHandler>();

        if (options is not null)
        {
            var defaultMiddlewares = GetDefaultMiddlewaresWithOptions(options);
            m.AddRange(defaultMiddlewares);
        }

        m.AddRange(middlewares);

        // Add replacement handler for /users/me to /me
        m.Add(new UriReplacementHandler<MeUriReplacementOption>(new MeUriReplacementOption()));

        // Add logging handler.
        if (loggingHandler is { } lh)
        {
            m.Add(lh);
        }

        return GraphClientFactory.Create(version: version, nationalCloud: environment.GraphClientCloud(), finalHandler: finalHandler, handlers: m);
    }
}
