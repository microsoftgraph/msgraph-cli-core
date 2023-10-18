using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.IO;

namespace Microsoft.Graph.Cli.Core.Authentication;

/// <summary>
/// Interactive login service.
/// </summary>
/// <typeparam name="T">An interactive token credential type</typeparam>
public class InteractiveLoginService<T> : LoginServiceBase where T : TokenCredential
{
    private readonly T credential;

    /// <summary>
    /// Creates a new interactive login service instance.
    /// </summary>
    /// <param name="credential">The interactive login credential.</param>
    /// <param name="pathUtility">The path utility instance</param>
    /// <exception cref="ArgumentException">If the provided credential does not support interactive login scenarios.</exception>
    public InteractiveLoginService(T credential, IPathUtility pathUtility) : base(pathUtility)
    {
        if (credential is not DeviceCodeCredential && credential is not InteractiveBrowserCredential)
        {
            throw new ArgumentException($"The provided credential {credential.GetType().Name} does not support interactive login." +
            $"Supported types are:\n {nameof(DeviceCodeCredential)}\n {nameof(InteractiveBrowserCredential)}", nameof(credential));
        }

        this.credential = credential;
    }

    /// <summary>
    /// Begin the interactive login process.
    /// </summary>
    /// <param name="scopes">Login scopes.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An authentication record.</returns>
    /// <exception cref="InvalidOperationException">When the credential is not supported.</exception>
    protected override async Task<AuthenticationRecord?> DoLoginAsync(string[] scopes, CancellationToken cancellationToken = default)
    {
        var requestContext = new TokenRequestContext(scopes);
        var record = credential switch
        {
            DeviceCodeCredential deviceCodeCred => await deviceCodeCred.AuthenticateAsync(requestContext, cancellationToken),
            InteractiveBrowserCredential browserCred => await browserCred.AuthenticateAsync(requestContext, cancellationToken),
            // Due to the check in the constructor, this code shouldn't be reachable normally.
            _ => throw new InvalidOperationException("The provided credential is not supported."),
        };

        // Request a new token to update the cache allowing incremental consent.
        var _ = await credential.GetTokenAsync(requestContext, cancellationToken);

        return record;
    }
}
