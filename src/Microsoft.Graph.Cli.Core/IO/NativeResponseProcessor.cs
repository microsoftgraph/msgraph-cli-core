using Microsoft.Kiota.Abstractions;

namespace Microsoft.Graph.Cli.Core.IO;

public class NativeResponseProcessor : IResponseProcessor
{
    public async Task<string?> ExtractStringResponseAsync(IResponseHandler responseHandler)
    {
        var handler = responseHandler as NativeResponseHandler;
        var response = handler?.Value as HttpResponseMessage;
        if (response is not null)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return null;
    }

    public async Task<Stream?> ExtractStreamResponseAsync(IResponseHandler responseHandler)
    {
        var handler = responseHandler as NativeResponseHandler;
        var response = handler?.Value as HttpResponseMessage;
        if (response is not null)
        {
            return await response.Content.ReadAsStreamAsync();
        }

        return null;
    }

    public bool IsResponseSuccessful(IResponseHandler responseHandler)
    {
        var handler = responseHandler as NativeResponseHandler;
        var response = handler?.Value as HttpResponseMessage;
        if (response is not null)
        {
            return response.IsSuccessStatusCode;
        }

        return false;
    }
}
