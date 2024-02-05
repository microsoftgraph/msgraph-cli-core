using System.Threading;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.Authentication;
using Microsoft.Graph.Cli.Core.Configuration;

namespace Microsoft.Graph.Cli.Core.IO;

/// <summary>
/// Authentication cache manager interface.
/// </summary>
public interface IAuthenticationCacheManager
{
    /// <summary>
    /// Computes the path to the authentication cache.
    /// </summary>
    /// <returns>Returns a path to the authentication cache.</returns>
    string GetAuthenticationCacheFilePath();

    /// <summary>
    /// Save authentication options in the cache file.
    /// </summary>
    /// <param name="clientId">Client Id</param>
    /// <param name="tenantId">Tenant Id</param>
    /// <param name="certificateName">Certificate name</param>
    /// <param name="certificateThumbPrint">Certificate thumb-print</param>
    /// <param name="strategy">Authentication strategy.</param>
    /// <param name="environment">The cloud environment.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A void awaitable task.</returns>
    Task SaveAuthenticationIdentifiersAsync(string? clientId, string? tenantId, string? certificateName, string? certificateThumbPrint, AuthenticationStrategy strategy, CloudEnvironment environment = CloudEnvironment.Global, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reads authentication options from the authentication cache path.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication options from the cache.</returns>
    Task<AuthenticationOptions> ReadAuthenticationIdentifiersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Reads an authentication record from a serialized file.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task with the authentication record.</returns>
    Task<AuthenticationRecord?> ReadAuthenticationRecordAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears the token cache.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A void awaitable task.</returns>
    Task ClearTokenCache(CancellationToken cancellationToken = default);
}
