using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Graph.Cli.Core.Http;

namespace Microsoft.Graph.Cli.Core.IO;

public class GraphCliClientFactory
{
    public static IEnumerable<DelegatingHandler> GetDefaultMiddlewaresWithOptions(GraphClientOptions? options) => GraphClientFactory.CreateDefaultHandlers(options);

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
