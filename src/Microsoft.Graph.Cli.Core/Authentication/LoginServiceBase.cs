using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.IO;
using Microsoft.Graph.Cli.Core.Utils;

namespace Microsoft.Graph.Cli.Core.Authentication;

public abstract class LoginServiceBase : ILoginService {
    private readonly IPathUtility pathUtility;

    protected LoginServiceBase(IPathUtility pathUtility) {
        this.pathUtility = pathUtility;
    }

    public async Task LoginAsync(string[] scopes, CancellationToken cancellationToken = default(CancellationToken)) {
        var record = await this.DoLoginAsync(scopes, cancellationToken);
        await this.SaveSessionAsync(record, cancellationToken);
    }

    protected abstract Task<AuthenticationRecord> DoLoginAsync(string[] scopes, CancellationToken cancellationToken = default(CancellationToken));

    public async Task SaveSessionAsync(AuthenticationRecord record, CancellationToken cancellationToken = default(CancellationToken)) {
        var recordPath = Path.Combine(pathUtility.GetApplicationDataDirectory(), Constants.AuthRecordPath);
        using var authRecordStream = new FileStream(recordPath, FileMode.OpenOrCreate, FileAccess.Write);
        await record.SerializeAsync(authRecordStream, cancellationToken);
    }
}