using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Microsoft.Graph.Cli.Core.Http;

public class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler> log;

    public LoggingHandler(ILogger<LoggingHandler> logger)
    {
        log = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
    {
        if (!log.IsEnabled(LogLevel.Debug)) return await base.SendAsync(request, cancellationToken);

        var requestContent = await ContentToStringAsync(request.Content, cancellationToken);

        log.LogDebug("\nRequest:\n\n{0} {1} HTTP/{2}\n{3}\n{4}\n",
            request.Method, request.RequestUri, request.Version,
            HeadersToString(request.Headers, request.Content?.Headers),
            requestContent
        );

        var response = await base.SendAsync(request, cancellationToken);

        // If the response has a content length > 0 and is not a stream, get the content. Otherwise get the content length
        var responseContent = await ContentToStringAsync(response.Content, cancellationToken);

        log.LogDebug("\nResponse:\n\nHTTP/{0} {1} {2}\n{3}\n{4}\n",
            response.Version, (int)response.StatusCode, response.ReasonPhrase,
            HeadersToString(response.Headers, response.Content?.Headers),
            responseContent
        );
        return response;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }

    private string HeadersToString(in HttpHeaders headers, in HttpHeaders? contentHeaders)
    {
        if (!headers.Any() && contentHeaders?.Any() == false) return string.Empty;
        Func<KeyValuePair<string, IEnumerable<string>>, string> selector = (h) =>
        {
            var value = string.Join(",", h.Value);
            if (h.Key.Contains("Authorization", StringComparison.OrdinalIgnoreCase))
            {
                value = "[PROTECTED]";
            }
            return string.Format("{0}: {1}\n", h.Key, value);
        };

        Func<string, string, string> aggregator = (a, b) => string.Join(string.Empty, a, b);

        var h = headers.Select(selector).Aggregate("", aggregator);
        if (contentHeaders != null)
        {
            h += contentHeaders.Select(selector).Aggregate("", aggregator);
        }

        return h;
    }

    private async Task<string> ContentToStringAsync(HttpContent? content, CancellationToken cancellationToken = default) {
        if (content is null) return string.Empty;
        var responseContent = string.Empty;
        var isStream = content.Headers.ContentType?.MediaType?.Contains("stream") == true;
        if (!isStream)
        {
            responseContent = await content.ReadAsStringAsync(cancellationToken);
        }
        else if (isStream)
        {
            string size = string.Empty;
            if (content.Headers.ContentLength > 0)
            {
                size = $"{content.Headers.ContentLength} byte ";
            }

            responseContent = $"[...<{size}data stream>...]";
        }

        return responseContent;
    }
}
