using Microsoft.Kiota.Abstractions;

namespace Microsoft.Graph.Cli.Core.IO;

public class NativeResponseProcessor : IResponseProcessor
{
    public async Task<string?> ExtractStringResponseAsync(IResponseHandler responseHandler)
    {
        var response = this.GetHttpResponseMessage(responseHandler);
        if (response is not null)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return null;
    }

    public async Task<Stream?> ExtractStreamResponseAsync(IResponseHandler responseHandler)
    {
        var response = this.GetHttpResponseMessage(responseHandler);
        if (response is not null)
        {
            return await response.Content.ReadAsStreamAsync();
        }

        return null;
    }

    public bool IsResponseSuccessful(IResponseHandler responseHandler)
    {
        var response = this.GetHttpResponseMessage(responseHandler);
        if (response is not null)
        {
            return response.IsSuccessStatusCode;
        }

        return false;
    }

    private HttpResponseMessage? GetHttpResponseMessage(IResponseHandler responseHandler) {
        var handler = responseHandler as NativeResponseHandler;
        return handler?.Value as HttpResponseMessage;
    }
}
