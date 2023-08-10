using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Microsoft.Graph.Cli.Core.Http;

public class HttpHeadersHandler : DelegatingHandler
{
    private readonly ILogger<HttpHeadersHandler> log;

    public HttpHeadersHandler(ILogger<HttpHeadersHandler> logger)
    {
        log = logger;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        foreach (var headerItem in HeadersStore.Instance.Headers)
        {
            try {
                request.Headers.Add(headerItem.Key, headerItem.Value);
            } catch (Exception ex) when (ex is InvalidOperationException || ex is FormatException) {
                log.LogWarning(ex, "Could not add the header {}", headerItem.Key);
            }
        }

        HeadersStore.Instance.ClearHeaders();
        return base.SendAsync(request, cancellationToken);
    }
}
public sealed class HeadersStore
{
    private readonly Dictionary<string, List<string>> headers = new();
    private static readonly Lazy<HeadersStore> storeLazyInstance =
        new(() => new HeadersStore());

    public static HeadersStore Instance { get { return storeLazyInstance.Value; } }

    public IEnumerable<KeyValuePair<string, List<string>>> Headers { get { return headers; } }

    private HeadersStore()
    {
    }

    public void ClearHeaders() {
        this.headers.Clear();
    }

    public void SetHeadersFromStrings(string[] headers)
    {
        if (headers.Length < 1)
        {
            return;
        }

        this.headers.Clear();

        var mapped = headers
            .Select<string, (string, string)?>(static h =>
            {
                var split = h.Split('=', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (split.Length < 1)
                {
                    return null;
                }

                var k = split[0];
                var v = string.Empty;
                if (split.Length > 1)
                {
                    v = split[1];
                }

                return (k, v);
            });
        foreach (var kv in mapped)
        {
            if (kv is { } nonNull)
            {
                if (!this.headers.TryAdd(nonNull.Item1, new List<string> { nonNull.Item2 }))
                {
                    this.headers[nonNull.Item1].Add(nonNull.Item2);
                }
            }
        }
    }
}
