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

    /// <summary>
    /// Creates an instance of LogoutService.
    /// </summary>
    /// <param name="cacheManager">Cache manager</param>
    public LogoutService(IAuthenticationCacheManager cacheManager)
    {
        this.authenticationCacheManager = cacheManager;
    }

    /// <summary>
    /// Clears the token cache.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task Logout(CancellationToken cancellationToken = default)
    {
        await authenticationCacheManager.ClearTokenCache(cancellationToken);
    }
}
