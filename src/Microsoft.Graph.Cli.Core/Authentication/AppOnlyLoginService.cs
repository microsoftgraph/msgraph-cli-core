using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.IO;

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
        return Task.FromResult<AuthenticationRecord?>(null);
    }
}
