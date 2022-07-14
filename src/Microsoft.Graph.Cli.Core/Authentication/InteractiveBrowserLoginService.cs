using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.IO;

namespace Microsoft.Graph.Cli.Core.Authentication;

public class InteractiveBrowserLoginService : LoginServiceBase
{
    private InteractiveBrowserCredential credential;

    public InteractiveBrowserLoginService(InteractiveBrowserCredential credential, IPathUtility pathUtility) : base(pathUtility)
    {
        this.credential = credential;
    }

    protected override async Task<AuthenticationRecord> DoLoginAsync(string[] scopes, CancellationToken cancellationToken = default)
    {
        return await credential.AuthenticateAsync(new TokenRequestContext(scopes), cancellationToken);
    }
}
