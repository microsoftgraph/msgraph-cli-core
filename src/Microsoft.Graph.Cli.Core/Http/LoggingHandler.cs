using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Microsoft.Graph.Cli.Core.Http;

/// <summary>
/// Logs request and response messages.
/// </summary>
public partial class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler>? log;

    /// <summary>
    /// Creates a new LoggingHandler. If no logging handler is provided, no message will be printed.
    /// </summary>
    /// <param name="logger">The logger to use.</param>
    public LoggingHandler(ILogger<LoggingHandler>? logger)
    {
        log = logger;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
    {
        if (log is null || !log.IsEnabled(LogLevel.Debug)) return await base.SendAsync(request, cancellationToken);

        var requestContent = await ContentToStringAsync(request.Content, cancellationToken);

        LogRequest(log, request.Method, request.RequestUri, request.Version,
            HeadersToString(request.Headers, request.Content?.Headers),
            requestContent);

        var response = await base.SendAsync(request, cancellationToken);

        LogResponse(log, response.Version, (int)response.StatusCode, response.ReasonPhrase,
            HeadersToString(response.Headers, response.Content?.Headers),
            await ContentToStringAsync(response.Content, cancellationToken));
        return response;
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }

    private static string HeadersToString(in HttpHeaders headers, in HttpContentHeaders? contentHeaders)
    {
        if (!headers.Any() && contentHeaders?.Any() == false) return string.Empty;
        static string selector(KeyValuePair<string, IEnumerable<string>> h)
        {
            var value = string.Join(",", h.Value);
            if (h.Key.Contains("Authorization", StringComparison.OrdinalIgnoreCase))
            {
                value = "[PROTECTED]";
            }
            return string.Format("{0}: {1}\n", h.Key, value);
        };

        static string aggregator(string a, string b)
        {
            return string.Join(string.Empty, a, b);
        }

        var h = headers.Select(selector).Aggregate("", aggregator);
        if (contentHeaders != null)
        {
            h += contentHeaders.Select(selector).Aggregate("", aggregator);
        }

        return h;
    }

    /// <summary>
    /// Extract a string from a HttpContent.
    /// If the message is a stream, a message with the content length is
    /// printed to prevent writing non-string bytes. If the content is
    /// not a stream, a string representation of the content is printed.
    /// </summary>
    /// <param name="content">Http message content.</param>
    /// <param name="cancellationToken">The task cancellation token.</param>
    /// <returns>A string representation of the response content.</returns>
    private static async Task<string> ContentToStringAsync(HttpContent? content, CancellationToken cancellationToken = default)
    {
        if (content is null) return string.Empty;
        var isStream = content.Headers.ContentType?.MediaType?.Contains("stream", StringComparison.OrdinalIgnoreCase) == true;
        if (!isStream)
        {
            return await content.ReadAsStringAsync(cancellationToken);
        }
        else
        {
            if (content.Headers.ContentLength > 0)
            {
                return $"[...<{content.Headers.ContentLength} byte data stream>...]";
            }
            else
            {
                return "[...<data stream>...]";
            }
        }
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "\nRequest:\n\n{RequestMethod} {RequestUri} HTTP/{HttpVersion}\n{Headers}\n{RequestContent}\n")]
    static partial void LogRequest(ILogger logger, HttpMethod requestMethod, Uri? requestUri, Version httpVersion, string headers, string requestContent);

    [LoggerMessage(EventId = 2, Level = LogLevel.Debug, Message = "\nResponse:\n\nHTTP/{HttpVersion} {ResponseStatusCode} {ResponseStatusMessage}\n{Headers}\n{ResponseContent}\n")]
    static partial void LogResponse(ILogger logger, Version httpVersion, int responseStatusCode, string? responseStatusMessage, string headers, string responseContent);
}
