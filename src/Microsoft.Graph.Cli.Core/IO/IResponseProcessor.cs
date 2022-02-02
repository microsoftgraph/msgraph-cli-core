using Microsoft.Kiota.Abstractions;

namespace Microsoft.Graph.Cli.Core.IO;

public interface IResponseProcessor
{
    Task<string?> ExtractStringResponseAsync(IResponseHandler responseHandler);

    Task<Stream?> ExtractStreamResponseAsync(IResponseHandler responseHandler);

    bool IsResponseSuccessful(IResponseHandler responseHandler);
}
