using System.Threading;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.Authentication;
using Microsoft.Graph.Cli.Core.Configuration;

namespace Microsoft.Graph.Cli.Core.IO;

public interface IAuthenticationCacheManager
{
    string GetAuthenticationCacheFilePath();

    Task SaveAuthenticationIdentifiersAsync(string? clientId, string? tenantId, string? certificateName, string? certificateThumbPrint, AuthenticationStrategy strategy, CancellationToken cancellationToken = default);

    Task<AuthenticationOptions> ReadAuthenticationIdentifiersAsync(CancellationToken cancellationToken = default);

    Task<AuthenticationRecord?> ReadAuthenticationRecordAsync(CancellationToken cancellationToken = default);

    Task ClearTokenCache(CancellationToken cancellationToken = default);
}
