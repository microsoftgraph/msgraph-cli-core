using Azure.Core;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Graph.Cli.Core.Authentication;

public class AppOnlyLoginService<T> : LoginServiceBase where T : TokenCredential
{
    private T credential;

    public AppOnlyLoginService(T credential, IPathUtility pathUtility) : base(pathUtility)
    {
        this.credential = credential;
    }

    protected override Task<AuthenticationRecord?> DoLoginAsync(string[] scopes, CancellationToken cancellationToken = default)
    {
        // TODO: Verify if auth is supported for app only.
        return Task.FromResult<AuthenticationRecord?>(null);
    }
}
