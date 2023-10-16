using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Graph.Cli.Core.Http;

namespace Microsoft.Graph.Cli.Core.IO;

/// <summary>
/// HTTP client factory for the graph CLI.
/// </summary>
public class GraphCliClientFactory
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
    /// <param name="nationalCloud">National cloud in use.</param>
    /// <param name="finalHandler">Final HTTP handler</param>
    /// <param name="loggingHandler">Logging handler</param>
    /// <param name="middlewares">Other middlewares.</param>
    /// <returns>Returns a new HTTP client.</returns>
    public static HttpClient GetDefaultClient(GraphClientOptions? options = null, string version = "v1.0", string nationalCloud = GraphClientFactory.Global_Cloud, HttpMessageHandler? finalHandler = null, LoggingHandler? loggingHandler = null, params DelegatingHandler[] middlewares)
    {
        var m = new List<DelegatingHandler>();

        if (options is not null)
        {
            var defaultMiddlewares = GetDefaultMiddlewaresWithOptions(options);
            m.AddRange(defaultMiddlewares);
        }

        m.AddRange(middlewares);

        // Add logging handler.
        if (loggingHandler is LoggingHandler lh)
        {
            m.Add(lh);
        }

        // Set compression handler to be last (Allows logging handler to log request body)
        m.Sort((a, b) =>
        {
            var a_match = a is Kiota.Http.HttpClientLibrary.Middleware.CompressionHandler;
            var b_match = b is Kiota.Http.HttpClientLibrary.Middleware.CompressionHandler;
            if (a_match && !b_match)
            {
                return 1;
            }
            else if (b_match && !a_match)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        });

        return GraphClientFactory.Create(version: version, nationalCloud: nationalCloud, finalHandler: finalHandler, handlers: m);
    }
}
