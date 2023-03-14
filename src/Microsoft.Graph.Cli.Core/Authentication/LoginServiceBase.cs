using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.IO;
using Microsoft.Graph.Cli.Core.Utils;

namespace Microsoft.Graph.Cli.Core.Authentication;

/// <summary>
/// Used by the login command to perform interactive login if necessary.
/// </summary>
public abstract class LoginServiceBase
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

    /// <summary>
    /// Subclasses should implement this method and perform interactive login using it. If the login strategy isn't interactive,
    /// subclasses can return a null authentication record.
    /// </summary>
    protected abstract Task<AuthenticationRecord?> DoLoginAsync(string[] scopes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stores an authentication record on disk for use later. If the authentication record is null, this function is a noop.
    /// </summary>
    public async Task SaveSessionAsync(AuthenticationRecord? record = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (record is null) return;
        var recordPath = Path.Combine(pathUtility.GetApplicationDataDirectory(), Constants.AuthRecordPath);
        using var authRecordStream = new FileStream(recordPath, FileMode.Create, FileAccess.Write);
        await record.SerializeAsync(authRecordStream, cancellationToken);
    }
}
