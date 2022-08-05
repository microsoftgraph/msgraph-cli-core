using System.Threading;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.IO;

namespace Microsoft.Graph.Cli.Core.Authentication;

public class ClientCertificateLoginService : LoginServiceBase
{
    private ClientCertificateCredential credential;

    public ClientCertificateLoginService(ClientCertificateCredential credential, IPathUtility pathUtility) : base(pathUtility)
    {
        this.credential = credential;
    }

    protected override Task<AuthenticationRecord?> DoLoginAsync(string[] scopes, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<AuthenticationRecord?>(null);
    }
}
