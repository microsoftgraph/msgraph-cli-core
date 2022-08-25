using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.Configuration;
using Microsoft.Graph.Cli.Core.IO;
using Microsoft.Graph.Cli.Core.Utils;

namespace Microsoft.Graph.Cli.Core.Authentication;

public class AuthenticationServiceFactory
{
    private readonly IPathUtility pathUtility;

    private readonly AuthenticationOptions? authenticationOptions;

    public AuthenticationServiceFactory(IPathUtility pathUtility, AuthenticationOptions? authOptions)
    {
        this.pathUtility = pathUtility;
        this.authenticationOptions = authOptions;
    }

    public virtual async Task<ILoginService> GetAuthenticationServiceAsync(AuthenticationStrategy strategy, string? tenantId, string? clientId, string? certificateName, string? certificateThumbPrint, CancellationToken cancellationToken = default)
    {
        switch (strategy)
        {
            case AuthenticationStrategy.DeviceCode:
                return await GetDeviceCodeLoginServiceAsync(tenantId, clientId, cancellationToken);
            case AuthenticationStrategy.InteractiveBrowser:
                return await GetInteractiveBrowserLoginServiceAsync(tenantId, clientId, cancellationToken);
            case AuthenticationStrategy.ClientCertificate:
                return GetClientCertificateLoginService(tenantId, clientId, certificateName, certificateThumbPrint);
            default:
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
            default:
                throw new InvalidOperationException($"The authentication strategy {strategy} is not supported");
        }
    }

    private async Task<DeviceCodeLoginService> GetDeviceCodeLoginServiceAsync(string? tenantId, string? clientId, CancellationToken cancellationToken = default)
    {
        var credential = await GetDeviceCodeCredentialAsync(tenantId, clientId, cancellationToken);
        return new(credential, pathUtility);
    }

    private async Task<InteractiveBrowserLoginService> GetInteractiveBrowserLoginServiceAsync(string? tenantId, string? clientId, CancellationToken cancellationToken = default)
    {
        var credential = await GetInteractiveBrowserCredentialAsync(tenantId, clientId, cancellationToken);
        return new(credential, pathUtility);
    }

    private ClientCertificateLoginService GetClientCertificateLoginService(string? tenantId, string? clientId, string? certificateName, string? certificateThumbPrint)
    {
        var credential = GetClientCertificateCredential(tenantId, clientId, certificateName, certificateThumbPrint);
        return new(credential, pathUtility);
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
        credOptions.AuthenticationRecord = await GetCachedAuthenticationRecordAsync(cancellationToken);

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
        credOptions.AuthenticationRecord = await GetCachedAuthenticationRecordAsync(cancellationToken);
        credOptions.LoginHint = credOptions.AuthenticationRecord?.Username;

        return new InteractiveBrowserCredential(credOptions);
    }

    private ClientCertificateCredential GetClientCertificateCredential(string? tenantId, string? clientId, string? certificateName, string? certificateThumbPrint)
    {
        return ClientCertificateCredentialFactory.GetClientCertificateCredential(tenantId ?? Constants.DefaultTenant, clientId ?? Constants.DefaultAppId, certificateName, certificateThumbPrint);
    }

    private async Task<AuthenticationRecord?> GetCachedAuthenticationRecordAsync(CancellationToken cancellationToken = default)
    {
        var recordPath = Path.Combine(pathUtility.GetApplicationDataDirectory(), Constants.AuthRecordPath);
        AuthenticationRecord? record = null;

        if (File.Exists(recordPath))
        {
            using var authRecordStream = new FileStream(recordPath, FileMode.Open, FileAccess.Read);
            var authRecord = await AuthenticationRecord.DeserializeAsync(authRecordStream, cancellationToken);
            record = authRecord;
        }

        return record;
    }
}
