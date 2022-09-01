using System.Linq;
using System.Collections.Generic;
using System.Net.Http;

namespace Microsoft.Graph.Cli.Core.IO;

public class GraphCliClientFactory
{
    public static IEnumerable<DelegatingHandler> GetDefaultMiddlewaresWithOptions(GraphClientOptions options) => GraphClientFactory.CreateDefaultHandlers(options);

    public static HttpClient GetDefaultClient(GraphClientOptions? options = null, string nationalCloud = GraphClientFactory.Global_Cloud, HttpMessageHandler? finalHandler = null, DelegatingHandler[]? lowestPriorityMiddlewares = null, params DelegatingHandler[] middlewares)
    {
        var m = new List<DelegatingHandler>();
        if (lowestPriorityMiddlewares?.Any() == true)
            m.AddRange(lowestPriorityMiddlewares);

        if (options is not null)
        {
            var defaultMiddlewares = GetDefaultMiddlewaresWithOptions(options);
            m.AddRange(defaultMiddlewares);
        }

        m.AddRange(middlewares);

        return GraphClientFactory.Create(nationalCloud: nationalCloud, finalHandler: finalHandler, handlers: m);
    }
}
