namespace Microsoft.Graph.Cli.Core.IO;

public interface IAuthenticationCacheUtility {
    Task SaveAuthenticationIdentifiersAsync(string clientId, string tenantId, CancellationToken cancellationToken = default(CancellationToken));

    Task<(string, string)> ReadAuthenticationIdentifiersAsync(CancellationToken cancellationToken = default(CancellationToken));
}