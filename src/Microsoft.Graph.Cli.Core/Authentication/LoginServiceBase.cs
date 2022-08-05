using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.IO;
using Microsoft.Graph.Cli.Core.Utils;

namespace Microsoft.Graph.Cli.Core.Authentication;

public abstract class LoginServiceBase : ILoginService
{
    private readonly IPathUtility pathUtility;

    protected LoginServiceBase(IPathUtility pathUtility)
    {
        this.pathUtility = pathUtility;
    }

    public async Task LoginAsync(string[] scopes, CancellationToken cancellationToken = default)
    {
        var record = await this.DoLoginAsync(scopes, cancellationToken);
        await this.SaveSessionAsync(record, cancellationToken);
    }

    protected abstract Task<AuthenticationRecord?> DoLoginAsync(string[] scopes, CancellationToken cancellationToken = default);

    public async Task SaveSessionAsync(AuthenticationRecord? record = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (record is null) return;
        var recordPath = Path.Combine(pathUtility.GetApplicationDataDirectory(), Constants.AuthRecordPath);
        using var authRecordStream = new FileStream(recordPath, FileMode.Create, FileAccess.Write);
        await record.SerializeAsync(authRecordStream, cancellationToken);
    }
}
