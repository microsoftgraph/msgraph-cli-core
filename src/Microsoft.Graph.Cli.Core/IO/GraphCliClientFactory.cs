using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Microsoft.Kiota.Http.HttpClientLibrary.Middleware;
using Microsoft.Kiota.Http.HttpClientLibrary.Middleware.Options;

namespace Microsoft.Graph.Cli.Core.IO;

public class GraphCliClientFactory
{
    const string SdkVersionHeaderValueFormatString = "graph-cli/{0}.{1}.{2}";

    public IEnumerable<DelegatingHandler> GetDefaultMiddlewaresWithOptions(GraphClientOptions options, IAuthenticationProvider authenticationProvider)
    {
        var result = new List<DelegatingHandler>();
        var kiotaMiddlewares = KiotaClientFactory.CreateDefaultHandlers();
        Func<HttpRequestMessage, HttpRequestMessage> telemetryCfg = (message) =>
        {
            if (Version.TryParse(options.GraphCliVersion, out var version))
            {
                var sdkVersionHeaderValue = string.Format(SdkVersionHeaderValueFormatString, version.Major, version.Minor, version.Build);
                message.Headers.Add("SdkVersion", sdkVersionHeaderValue);
            }
            return message;
        };
        var telemetryHandlerOption = new TelemetryHandlerOption
        {
            TelemetryConfigurator = telemetryCfg
        };
        var additionalMiddlewares = new DelegatingHandler[] {
            new TelemetryHandler(telemetryHandlerOption)
        };
        var graphMiddlewares = GraphClientFactory.CreateDefaultHandlers(authenticationProvider);
        result.AddRange(kiotaMiddlewares);
        result.AddRange(graphMiddlewares);
        result.AddRange(additionalMiddlewares);

        return result;
    }

    public HttpClient GetDefaultClient(GraphClientOptions options, IAuthenticationProvider authenticationProvider, string nationalCloud = GraphClientFactory.Global_Cloud, params DelegatingHandler[] middlewares)
    {
        IEnumerable<DelegatingHandler> m = middlewares;
        if (middlewares.Length == 0)
        {
            m = GetDefaultMiddlewaresWithOptions(options, authenticationProvider);
        }
        var finalHandler = KiotaClientFactory.ChainHandlersCollectionAndGetFirstLink(KiotaClientFactory.GetDefaultHttpMessageHandler(), middlewares);
        return GraphClientFactory.Create(authenticationProvider, version: options.GraphServiceVersion, nationalCloud: nationalCloud, finalHandler: finalHandler);
    }
}