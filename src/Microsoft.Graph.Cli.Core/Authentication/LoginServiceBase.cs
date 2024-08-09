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

    /// <summary>
    /// Creates a new instance of the login service base abstract class.
    /// </summary>
    /// <param name="pathUtility">The path utility</param>
    protected LoginServiceBase(IPathUtility pathUtility)
    {
        this.pathUtility = pathUtility;
    }

    /// <summary>
    /// Perform login.
    /// </summary>
    /// <param name="scopes">Login scopes.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A void task.</returns>
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
        var authRecordStream = new FileStream(recordPath, FileMode.Create, FileAccess.Write);
        await using (authRecordStream.ConfigureAwait(false))
        {
            await record.SerializeAsync(authRecordStream, cancellationToken);
        }
    }
}
