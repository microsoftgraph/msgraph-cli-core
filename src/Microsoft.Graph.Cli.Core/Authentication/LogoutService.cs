using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph.Cli.Core.IO;

namespace Microsoft.Graph.Cli.Core.Authentication;

public class LogoutService
{
    private readonly IAuthenticationCacheUtility authCacheUtility;

    public LogoutService(IAuthenticationCacheUtility authCacheUtility)
    {
        this.authCacheUtility = authCacheUtility;
    }

    public async Task Logout(CancellationToken cancellationToken = default)
    {
        await authCacheUtility.ClearTokenCache(cancellationToken);
    }
}
