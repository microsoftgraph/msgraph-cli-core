using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System;
using Microsoft.Kiota.Http.HttpClientLibrary.Middleware;
using System.Net.Http.Headers;

namespace Microsoft.Graph.Cli.Core.IO;

public class GraphCliClientFactory
{
    /// Microsoft Graph service national cloud endpoints
    private static readonly Dictionary<string, string> cloudList = new Dictionary<string, string>
        {
            { GraphClientFactory.Global_Cloud, "https://graph.microsoft.com" },
            { GraphClientFactory.USGOV_Cloud, "https://graph.microsoft.us" },
            { GraphClientFactory.China_Cloud, "https://microsoftgraph.chinacloudapi.cn" },
            { GraphClientFactory.Germany_Cloud, "https://graph.microsoft.de" },
            { GraphClientFactory.USGOV_DOD_Cloud, "https://dod-graph.microsoft.us" },
        };

    /// The default value for the overall request timeout.
    private static readonly TimeSpan defaultTimeout = TimeSpan.FromSeconds(100);

    public static IEnumerable<DelegatingHandler> GetDefaultMiddlewaresWithOptions(GraphClientOptions? options) => GraphClientFactory.CreateDefaultHandlers(options);

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

    public static HttpMessageHandler GetDefaultGraphHandler(GraphClientOptions? options = null, HttpMessageHandler? finalHandler = null, DelegatingHandler[]? lowestPriorityMiddlewares = null, params DelegatingHandler[] middlewares)
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
        return GraphClientFactory.CreatePipeline(m, finalHandler);
    }

    public static void ConfigureClient(HttpClient client, GraphClientOptions? options = null, string version = "v1.0", string nationalCloud = GraphClientFactory.Global_Cloud)
    {
        client.BaseAddress = DetermineBaseAddress(nationalCloud, version);
        client.DefaultRequestHeaders.Add(CoreConstants.Headers.SdkVersionHeaderName, options?.GraphCoreClientVersion);
        client.Timeout = defaultTimeout;
        client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true, NoStore = true };
    }

    private static Uri DetermineBaseAddress(string nationalCloud, string version)
    {
        string? cloud = "";
        if (!cloudList.TryGetValue(nationalCloud, out cloud))
        {
            throw new ArgumentException(String.Format("{0} is an unexpected national cloud.", nationalCloud, "nationalCloud"));
        }
        string cloudAddress = $"{cloud}/{version}/";
        return new Uri(cloudAddress);

    }
}
