using Azure.Core;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.Configuration;
using Microsoft.Graph.Cli.Core.IO;
using Microsoft.Graph.Cli.Core.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

#if OS_WINDOWS
using System.Diagnostics;
using Azure.Identity.Broker;
using Microsoft.Graph.Cli.Core.utils;
#endif

namespace Microsoft.Graph.Cli.Core.Authentication;

/// <summary>
/// Creates authentication services based on a strategy.
/// </summary>
public class AuthenticationServiceFactory
{
    private readonly IPathUtility pathUtility;

    private readonly IAuthenticationCacheManager authenticationCacheManager;

    private readonly AuthenticationOptions? authenticationOptions;

    /// <summary>
    /// Creates a new authentication service factory instance
    /// </summary>
    /// <param name="pathUtility">Path utility</param>
    /// <param name="authenticationCacheManager">Cache manager.</param>
    /// <param name="authenticationOptions">Authentication options.</param>
    public AuthenticationServiceFactory(IPathUtility pathUtility, IAuthenticationCacheManager authenticationCacheManager, AuthenticationOptions? authenticationOptions)
    {
        this.pathUtility = pathUtility;
        this.authenticationOptions = authenticationOptions;
        this.authenticationCacheManager = authenticationCacheManager;
    }

    /// <summary>
    /// Returns a login service that satisfies the provided authentication strategy.
    /// </summary>
    /// <param name="strategy">Authentication strategy.</param>
    /// <param name="tenantId">Tenant Id</param>
    /// <param name="clientId">Client Id</param>
    /// <param name="certificateName">Certificate name</param>
    /// <param name="certificateThumbPrint">Certificate thumb-print</param>
    /// <param name="environment">The national cloud environment. Either 'Global', 'US_GOV', 'US_GOV_DOD' or 'China'</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Returns a login service instance.</returns>
    /// <exception cref="InvalidOperationException">When an unsupported authentication strategy is provided.</exception>
    public virtual async Task<LoginServiceBase> GetAuthenticationServiceAsync(AuthenticationStrategy strategy, string? tenantId, string? clientId, string? certificateName, string? certificateThumbPrint, CloudEnvironment environment, CancellationToken cancellationToken = default)
    {
        var credential = await GetTokenCredentialAsync(strategy, tenantId, clientId, certificateName, certificateThumbPrint, environment, cancellationToken);
        if (strategy == AuthenticationStrategy.DeviceCode && credential is DeviceCodeCredential deviceCred)
        {
            return new InteractiveLoginService<DeviceCodeCredential>(deviceCred, pathUtility);
        }
        else if (strategy == AuthenticationStrategy.InteractiveBrowser && credential is InteractiveBrowserCredential browserCred)
        {
            return new InteractiveLoginService<InteractiveBrowserCredential>(browserCred, pathUtility);
        }
        else if (strategy == AuthenticationStrategy.ClientCertificate && credential is ClientCertificateCredential)
        {
            return new AppOnlyLoginService<ClientCertificateCredential>(pathUtility);
        }
        else if (strategy == AuthenticationStrategy.ManagedIdentity && credential is ManagedIdentityCredential)
        {
            return new AppOnlyLoginService<ManagedIdentityCredential>(pathUtility);
        }
        else if (strategy == AuthenticationStrategy.Environment && credential is EnvironmentCredential)
        {
            return new AppOnlyLoginService<EnvironmentCredential>(pathUtility);
        }
        else
        {
            throw new InvalidOperationException($"The authentication strategy {strategy} is not supported");
        }
    }

    /// <summary>
    /// Returns a credential object instance that satisfies the provided authentication strategy
    /// </summary>
    /// <param name="strategy">Authentication strategy.</param>
    /// <param name="tenantId">Tenant Id</param>
    /// <param name="clientId">Client Id</param>
    /// <param name="certificateName">Certificate name</param>
    /// <param name="certificateThumbPrint">Certificate thumb-print</param>
    /// <param name="environment">The cloud environment. <see cref="CloudEnvironment"/></param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A token credential instance.</returns>
    /// <exception cref="InvalidOperationException">When an unsupported authentication strategy is provided.</exception>
    /// <exception cref="ArgumentNullException">When a null url is provided for the authority host.</exception>
    public virtual async Task<TokenCredential> GetTokenCredentialAsync(AuthenticationStrategy strategy, string? tenantId, string? clientId, string? certificateName, string? certificateThumbPrint, CloudEnvironment environment, CancellationToken cancellationToken = default)
    {
        var authorityHost = environment.Authority();
        return strategy switch
        {
            AuthenticationStrategy.DeviceCode => await GetDeviceCodeCredentialAsync(tenantId, clientId, authorityHost, cancellationToken),
            AuthenticationStrategy.InteractiveBrowser => await GetInteractiveBrowserCredentialAsync(tenantId, clientId, authorityHost, cancellationToken),
            AuthenticationStrategy.ClientCertificate => GetClientCertificateCredential(tenantId, clientId, certificateName, certificateThumbPrint, authorityHost),
            AuthenticationStrategy.Environment => new EnvironmentCredential(tenantId, clientId, new TokenCredentialOptions { AuthorityHost = authorityHost }),
            AuthenticationStrategy.ManagedIdentity => new ManagedIdentityCredential(clientId, new TokenCredentialOptions { AuthorityHost = authorityHost }),
            _ => throw new InvalidOperationException($"The authentication strategy {strategy} is not supported"),
        };
    }

    private async Task<DeviceCodeCredential> GetDeviceCodeCredentialAsync(string? tenantId, string? clientId, Uri authorityHost, CancellationToken cancellationToken = default)
    {
        DeviceCodeCredentialOptions credOptions = new()
        {
            ClientId = clientId ?? Constants.DefaultAppId,
            TenantId = tenantId ?? Constants.DefaultTenant,
            DisableAutomaticAuthentication = true,
            AuthorityHost = authorityHost
        };

        TokenCachePersistenceOptions tokenCacheOptions = new() { Name = Constants.TokenCacheName };
        credOptions.TokenCachePersistenceOptions = tokenCacheOptions;
        credOptions.AuthenticationRecord = await authenticationCacheManager.ReadAuthenticationRecordAsync(cancellationToken);

        return new DeviceCodeCredential(credOptions);
    }

    private async Task<InteractiveBrowserCredential> GetInteractiveBrowserCredentialAsync(string? tenantId, string? clientId, Uri authorityHost, CancellationToken cancellationToken = default)
    {
#if OS_WINDOWS
        Debug.Assert(OperatingSystem.IsWindows());
        InteractiveBrowserCredentialBrokerOptions credOptions = new(WindowUtils.GetConsoleOrTerminalWindow());
#else
        InteractiveBrowserCredentialOptions credOptions = new();
#endif

        credOptions.ClientId = clientId ?? Constants.DefaultAppId;
        credOptions.TenantId = tenantId ?? Constants.DefaultTenant;
        credOptions.DisableAutomaticAuthentication = true;
        credOptions.AuthorityHost = authorityHost;

        TokenCachePersistenceOptions tokenCacheOptions = new() { Name = Constants.TokenCacheName };
        credOptions.TokenCachePersistenceOptions = tokenCacheOptions;
        credOptions.AuthenticationRecord = await authenticationCacheManager.ReadAuthenticationRecordAsync(cancellationToken);

        return new InteractiveBrowserCredential(credOptions);
    }

    private ClientCertificateCredential GetClientCertificateCredential(string? tenantId, string? clientId, string? certificateName, string? certificateThumbPrint, Uri authorityHost)
    {
        return ClientCertificateCredentialFactory.GetClientCertificateCredential(tenantId ?? Constants.DefaultTenant, clientId ?? Constants.DefaultAppId, certificateName, certificateThumbPrint, authorityHost);
    }
}
