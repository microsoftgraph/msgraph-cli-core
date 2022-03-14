using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph.Cli.Core.Configuration;

namespace Microsoft.Graph.Cli.Core.IO;

public interface IAuthenticationCacheUtility {
    string GetAuthenticationCacheFilePath();

    Task SaveAuthenticationIdentifiersAsync(string? clientId, string? tenantId, CancellationToken cancellationToken = default);

    Task<AuthenticationOptions> ReadAuthenticationIdentifiersAsync(CancellationToken cancellationToken = default);
}