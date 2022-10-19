using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Microsoft.Graph.Cli.Core.Http;

public class LoggingHandler : DelegatingHandler
{
    public ILogger<LoggingHandler>? Logger { private get; set; }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
    {
        if (Logger is null) return await base.SendAsync(request, cancellationToken);

        Logger?.LogDebug("\nRequest:\n\n{0} {1} HTTP/{2}\n{3}\n{4}\n",
            request.Method, request.RequestUri, request.Version,
            HeadersToString(request.Headers),
            await (request.Content?.ReadAsStringAsync() ?? Task.FromResult<string>(string.Empty))
        );

        var response = await base.SendAsync(request, cancellationToken);

        // If the response has a content length > 0 and is not a stream, get the content. Otherwise get the content length
        var responseContent = response.Content.Headers.ContentLength > 0 && response.Content.Headers.ContentType?.MediaType?.Contains("stream") == true ?
                    (response.Content.Headers.ContentLength ?? 0).ToString() :
                    await response.Content.ReadAsStringAsync();

        Logger?.LogDebug("\nResponse:\n\nHTTP/{0} {1} {2}\n{3}\n{4}\n",
            response.Version, (int)response.StatusCode, response.ReasonPhrase,
            HeadersToString(request.Headers),
            responseContent
        );
        return response;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }

    private string HeadersToString(in HttpHeaders headers)
    {
        return headers.Select((h) =>
        {
            var value = string.Join(",", h.Value);
            if (h.Key.Contains("Authorization", StringComparison.OrdinalIgnoreCase))
            {
                value = "[PROTECTED]";
            }
            return string.Format("{0}: {1}\n", h.Key, value);
        }).Aggregate((a, b) => string.Join(string.Empty, a, b));
    }
}
