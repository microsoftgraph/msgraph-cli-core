using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.IO;

namespace Microsoft.Graph.Cli.Core.Authentication;

/// <summary>
/// App only login service.
/// </summary>
/// <typeparam name="T">An app-only token credential type</typeparam>
public class AppOnlyLoginService<T> : LoginServiceBase where T : TokenCredential
{
    /// <summary>
    /// Creates a new instance of an app-only login service.
    /// </summary>
    /// <param name="pathUtility">The path utility instance.</param>
    public AppOnlyLoginService(IPathUtility pathUtility) : base(pathUtility)
    {
    }

    /// <summary>
    /// No-op in app-only login scenarios.
    /// </summary>
    /// <param name="scopes">Scopes</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A completed task with no auth record.</returns>
    protected override Task<AuthenticationRecord?> DoLoginAsync(string[] scopes, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<AuthenticationRecord?>(null);
    }
}
