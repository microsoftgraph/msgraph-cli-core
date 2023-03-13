using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.Configuration;
using Microsoft.Graph.Cli.Core.IO;
using Microsoft.Graph.Cli.Core.Utils;

namespace Microsoft.Graph.Cli.Core.Authentication;

/// <summary>
/// Creates authentication services based on a strategy.
/// </summary>
public class AuthenticationServiceFactory
{
    private readonly IPathUtility pathUtility;

    private readonly IAuthenticationCacheManager authenticationCacheManager;

    private readonly AuthenticationOptions? authenticationOptions;

    public AuthenticationServiceFactory(IPathUtility pathUtility, IAuthenticationCacheManager authenticationCacheManager, AuthenticationOptions? authOptions)
    {
        this.pathUtility = pathUtility;
        this.authenticationOptions = authOptions;
        this.authenticationCacheManager = authenticationCacheManager;
    }

    public virtual async Task<LoginServiceBase> GetAuthenticationServiceAsync(AuthenticationStrategy strategy, string? tenantId, string? clientId, string? certificateName, string? certificateThumbPrint, CancellationToken cancellationToken = default)
    {
        var credential = await GetTokenCredentialAsync(strategy, tenantId, clientId, certificateName, certificateThumbPrint, cancellationToken);
        if (strategy == AuthenticationStrategy.DeviceCode && credential is DeviceCodeCredential deviceCred)
        {
            return new InteractiveLoginService<DeviceCodeCredential>(deviceCred, pathUtility);
        }
        else if (strategy == AuthenticationStrategy.InteractiveBrowser && credential is InteractiveBrowserCredential browserCred)
        {
            return new InteractiveLoginService<InteractiveBrowserCredential>(browserCred, pathUtility);
        }
        else if (strategy == AuthenticationStrategy.ClientCertificate && credential is ClientCertificateCredential certCred)
        {
            return new AppOnlyLoginService<ClientCertificateCredential>(GetClientCertificateCredential(tenantId, clientId, certificateName, certificateThumbPrint), pathUtility);
        }
        else if (strategy == AuthenticationStrategy.Environment && credential is EnvironmentCredential envCred)
        {
            return new AppOnlyLoginService<EnvironmentCredential>(envCred, pathUtility);
        }
        else
        {
            throw new InvalidOperationException($"The authentication strategy {strategy} is not supported");
        }
    }

    public virtual async Task<TokenCredential> GetTokenCredentialAsync(AuthenticationStrategy strategy, string? tenantId, string? clientId, string? certificateName, string? certificateThumbPrint, CancellationToken cancellationToken = default)
    {
        switch (strategy)
        {
            case AuthenticationStrategy.DeviceCode:
                return await GetDeviceCodeCredentialAsync(tenantId, clientId, cancellationToken);
            case AuthenticationStrategy.InteractiveBrowser:
                return await GetInteractiveBrowserCredentialAsync(tenantId, clientId, cancellationToken);
            case AuthenticationStrategy.ClientCertificate:
                return GetClientCertificateCredential(tenantId, clientId, certificateName, certificateThumbPrint);
            case AuthenticationStrategy.Environment:
                return new EnvironmentCredential(tenantId, clientId);
            default:
                throw new InvalidOperationException($"The authentication strategy {strategy} is not supported");
        }
    }

    private async Task<DeviceCodeCredential> GetDeviceCodeCredentialAsync(string? tenantId, string? clientId, CancellationToken cancellationToken = default)
    {
        DeviceCodeCredentialOptions credOptions = new()
        {
            ClientId = clientId ?? Constants.DefaultAppId,
            TenantId = tenantId ?? Constants.DefaultTenant,
            DisableAutomaticAuthentication = true,
        };

        TokenCachePersistenceOptions tokenCacheOptions = new() { Name = Constants.TokenCacheName };
        credOptions.TokenCachePersistenceOptions = tokenCacheOptions;
        credOptions.AuthenticationRecord = await authenticationCacheManager.ReadAuthenticationRecordAsync(cancellationToken);

        return new DeviceCodeCredential(credOptions);
    }

    private async Task<InteractiveBrowserCredential> GetInteractiveBrowserCredentialAsync(string? tenantId, string? clientId, CancellationToken cancellationToken = default)
    {
        InteractiveBrowserCredentialOptions credOptions = new()
        {
            ClientId = clientId ?? Constants.DefaultAppId,
            TenantId = tenantId ?? Constants.DefaultTenant,
            DisableAutomaticAuthentication = true,
        };

        TokenCachePersistenceOptions tokenCacheOptions = new() { Name = Constants.TokenCacheName };
        credOptions.TokenCachePersistenceOptions = tokenCacheOptions;
        credOptions.AuthenticationRecord = await authenticationCacheManager.ReadAuthenticationRecordAsync(cancellationToken);
        credOptions.LoginHint = credOptions.AuthenticationRecord?.Username;

        return new InteractiveBrowserCredential(credOptions);
    }

    private ClientCertificateCredential GetClientCertificateCredential(string? tenantId, string? clientId, string? certificateName, string? certificateThumbPrint)
    {
        return ClientCertificateCredentialFactory.GetClientCertificateCredential(tenantId ?? Constants.DefaultTenant, clientId ?? Constants.DefaultAppId, certificateName, certificateThumbPrint);
    }
}
