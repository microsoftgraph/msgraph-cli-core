using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.IO;
using Microsoft.Graph.Cli.Core.Utils;

namespace Microsoft.Graph.Cli.Core.Authentication;

public class AuthenticationServiceFactory
{
    private readonly IPathUtility pathUtility;

    public AuthenticationServiceFactory(IPathUtility pathUtility)
    {
        this.pathUtility = pathUtility;
    }

    public virtual async Task<ILoginService> GetAuthenticationServiceAsync(AuthenticationStrategy strategy, string? tenantId, string? clientId, CancellationToken cancellationToken = default)
    {
        switch (strategy)
        {
            case AuthenticationStrategy.DeviceCode:
                return await GetDeviceCodeLoginServiceAsync(tenantId, clientId, cancellationToken);
            default:
                throw new InvalidOperationException($"The authentication strategy {strategy} is not supported");
        }

    }

    public virtual async Task<TokenCredential> GetTokenCredentialAsync(AuthenticationStrategy strategy, string? tenantId, string? clientId, CancellationToken cancellationToken = default)
    {
        switch (strategy)
        {
            case AuthenticationStrategy.DeviceCode:
                return await GetDeviceCodeCredentialAsync(tenantId, clientId, cancellationToken);
            default:
                throw new InvalidOperationException($"The authentication strategy {strategy} is not supported");
        }
    }

    private async Task<DeviceCodeLoginService> GetDeviceCodeLoginServiceAsync(string? tenantId, string? clientId, CancellationToken cancellationToken = default)
    {
        var credential = await GetDeviceCodeCredentialAsync(tenantId, clientId, cancellationToken);
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
        var recordPath = Path.Combine(pathUtility.GetApplicationDataDirectory(), Constants.AuthRecordPath);

        if (File.Exists(recordPath))
        {
            using var authRecordStream = new FileStream(recordPath, FileMode.Open, FileAccess.Read);
            var authRecord = await AuthenticationRecord.DeserializeAsync(authRecordStream, cancellationToken);
            credOptions.AuthenticationRecord = authRecord;
        }

        return new DeviceCodeCredential(credOptions);
    }
}
