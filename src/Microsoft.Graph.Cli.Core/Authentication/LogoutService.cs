using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph.Cli.Core.IO;

namespace Microsoft.Graph.Cli.Core.Authentication;

/// <summary>
/// Used by the logout command to clear session data. It currently clears the token cache using the <see ref="AuthenticationCacheManager"/>.
/// </summary>
public class LogoutService
{
    private readonly IAuthenticationCacheManager authenticationCacheManager;

    public LogoutService(IAuthenticationCacheManager cacheManager)
    {
        this.authenticationCacheManager = cacheManager;
    }

    public async Task Logout(CancellationToken cancellationToken = default)
    {
        await authenticationCacheManager.ClearTokenCache(cancellationToken);
    }
}
