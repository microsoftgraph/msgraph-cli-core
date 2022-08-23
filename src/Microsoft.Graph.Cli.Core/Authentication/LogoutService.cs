using System.IO;
using Microsoft.Graph.Cli.Core.IO;
using Microsoft.Graph.Cli.Core.Utils;

namespace Microsoft.Graph.Cli.Core.Authentication;

public class LogoutService
{
    private readonly IAuthenticationCacheUtility authCacheUtility;

    private readonly IPathUtility pathUtility;

    public LogoutService(IAuthenticationCacheUtility authCacheUtility, IPathUtility pathUtility)
    {
        this.authCacheUtility = authCacheUtility;
        this.pathUtility = pathUtility;
    }

    public void Logout()
    {
        var authRecordPath = Path.Combine(pathUtility.GetApplicationDataDirectory(), Constants.AuthRecordPath);
        if (File.Exists(authRecordPath))
        {
            File.Delete(authRecordPath);
        }

        var authIdentifierPath = authCacheUtility.GetAuthenticationCacheFilePath();
        if (File.Exists(authIdentifierPath))
        {
            File.Delete(authIdentifierPath);
        }
    }
}
