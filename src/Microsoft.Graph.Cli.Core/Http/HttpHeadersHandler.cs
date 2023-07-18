using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Graph.Cli.Core.Http;

public class HttpHeadersHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        foreach (var headerItem in HeadersStore.Instance.Headers)
        {
            request.Headers.Add(headerItem.Key, headerItem.Value);
        }
        return base.SendAsync(request, cancellationToken);
    }
}
public sealed class HeadersStore
{
    private readonly Dictionary<string, List<string>> headers = new();
    private static readonly Lazy<HeadersStore> lazy =
        new Lazy<HeadersStore>(() => new HeadersStore());

    public static HeadersStore Instance { get { return lazy.Value; } }

    public IEnumerable<KeyValuePair<string, List<string>>> Headers { get { return headers; } }

    private HeadersStore()
    {
    }

    public void SetHeadersFromStrings(string[] headers)
    {
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
