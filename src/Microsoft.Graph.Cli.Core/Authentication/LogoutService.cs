using System.IO;
using Microsoft.Graph.Cli.Core.IO;
using Microsoft.Graph.Cli.Core.Utils;

namespace Microsoft.Graph.Cli.Core.Authentication;

public class LogoutService
{
    private readonly IAuthenticationCacheUtility authCacheUtility;

    public LogoutService(IAuthenticationCacheUtility authCacheUtility)
    {
        this.authCacheUtility = authCacheUtility;
    }

    public void Logout()
    {
        authCacheUtility.DeleteAuthenticationIdentifiers();
        authCacheUtility.DeleteAuthenticationRecord();
    }
}
