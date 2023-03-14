using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.IO;

namespace Microsoft.Graph.Cli.Core.Authentication;

public class InteractiveLoginService<T> : LoginServiceBase where T : TokenCredential
{
    private T credential;

    public InteractiveLoginService(T credential, IPathUtility pathUtility) : base(pathUtility)
    {
        if (credential is not DeviceCodeCredential && credential is not InteractiveBrowserCredential)
        {
            throw new ArgumentException($"The provided credential {credential.GetType().Name} does not support interactive login." +
            $"Supported types are:\n {nameof(DeviceCodeCredential)}\n {nameof(InteractiveBrowserCredential)}", nameof(credential));
        }

        this.credential = credential;
    }

    protected override async Task<AuthenticationRecord?> DoLoginAsync(string[] scopes, CancellationToken cancellationToken = default)
    {
        if (credential is DeviceCodeCredential deviceCodeCred)
        {
            return await deviceCodeCred.AuthenticateAsync(new TokenRequestContext(scopes), cancellationToken);
        }
        else if (credential is InteractiveBrowserCredential browserCred)
        {
            return await browserCred.AuthenticateAsync(new TokenRequestContext(scopes), cancellationToken);
        }

        // Due to the check in the constructor, this code shouldn't be reachable normally.
        throw new InvalidOperationException("The provided credential is not supported.");
    }
}
