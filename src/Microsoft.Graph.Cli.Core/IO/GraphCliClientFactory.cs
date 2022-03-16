using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace Microsoft.Graph.Cli.Core.IO;

public class GraphCliClientFactory
{
    public static IEnumerable<DelegatingHandler> GetDefaultMiddlewaresWithOptions(GraphClientOptions options) => GraphClientFactory.CreateDefaultHandlers(options);

    public static HttpClient GetDefaultClient(GraphClientOptions options, string nationalCloud = GraphClientFactory.Global_Cloud, params DelegatingHandler[] middlewares)
    {
        IEnumerable<DelegatingHandler> m = middlewares;
        if (!middlewares.Any())
        {
            m = GetDefaultMiddlewaresWithOptions(options);
        }
        var finalHandler = KiotaClientFactory.ChainHandlersCollectionAndGetFirstLink(KiotaClientFactory.GetDefaultHttpMessageHandler(), middlewares);
        return GraphClientFactory.Create(graphClientOptions: options, nationalCloud: nationalCloud, finalHandler: finalHandler);
    }
}