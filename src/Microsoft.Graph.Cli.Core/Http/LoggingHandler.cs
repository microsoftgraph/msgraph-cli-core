using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Microsoft.Graph.Cli.Core.Http;

public class LoggingHandler : DelegatingHandler
{
    private ILogger<LoggingHandler> logger;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
    {
        if (logger is null) return await base.SendAsync(request, cancellationToken);

        var headers = request.Headers.Select((h) =>
        {
            var value = string.Join(",", h.Value);
            if (h.Key == "Authorization")
            {
                value = "[PROTECTED]";
            }
            return string.Format("H:{0}={1}", h.Key, value);
        }).Aggregate((a, b) => string.Join("\n\t", a, b));
        logger?.LogDebug("Calling:\n{0} {1}\n\t{2}", request.Method, request.RequestUri, headers);
        var response = await base.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger?.LogDebug("{0}\t{1}\t{2}", request.RequestUri,
                (int)response.StatusCode, response.Headers.Date);
            logger?.LogDebug("{0}", await response.Content?.ReadAsStringAsync());
        }
        return response;
    }

    public void SetLogger(ILogger<LoggingHandler> logger)
    {
        this.logger = logger;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }
}
