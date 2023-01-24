using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.Authentication;
using Microsoft.Graph.Cli.Core.Configuration;
using Microsoft.Graph.Cli.Core.Utils;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;

namespace Microsoft.Graph.Cli.Core.IO;

public class AuthenticationCacheUtility : IAuthenticationCacheUtility
{
    private readonly IPathUtility pathUtility;

    public AuthenticationCacheUtility(IPathUtility pathUtility)
    {
        this.pathUtility = pathUtility;
    }

    public string GetAuthenticationCacheFilePath()
    {
        return Path.Join(pathUtility.GetApplicationDataDirectory(), Constants.AuthenticationIdCachePath);
    }

    public async Task<AuthenticationOptions> ReadAuthenticationIdentifiersAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var path = this.GetAuthenticationCacheFilePath();
        if (!File.Exists(path))
        {
            throw new FileNotFoundException();
        }

        var configRoot = await ReadConfigurationAsync(cancellationToken);
        if (configRoot?.AuthenticationOptions is null) throw new AuthenticationIdentifierException("Cannot find cached authentication identifiers.");

        return configRoot.AuthenticationOptions;
    }

    public async Task SaveAuthenticationIdentifiersAsync(string? clientId, string? tenantId, string? certificateName, string? certificateThumbPrint, AuthenticationStrategy strategy, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var path = this.GetAuthenticationCacheFilePath();
        ConfigurationRoot configuration = await ReadConfigurationAsync(cancellationToken) ?? new Configuration.ConfigurationRoot();
        var authOptions = configuration.AuthenticationOptions;
        var adAuthority = Constants.DefaultAuthority;
        clientId = clientId ?? Constants.DefaultAppId;
        tenantId = tenantId ?? Constants.DefaultTenant;

        // TODO: Verify the auth record matches the auth options supplied.

        // Only write auth configuration if the values have changed
        if (
                clientId != authOptions.ClientId || tenantId != authOptions.TenantId || certificateName != authOptions.ClientCertificateName ||
                certificateThumbPrint != authOptions.ClientCertificateThumbPrint || strategy != authOptions.Strategy ||
                adAuthority != authOptions.Authority
        )
        {
            configuration.AuthenticationOptions = new AuthenticationOptions
            {
                Authority = adAuthority,
                ClientId = clientId,
                TenantId = tenantId,
                ClientCertificateName = certificateName,
                ClientCertificateThumbPrint = certificateThumbPrint,
                Strategy = strategy,
            };

            await WriteConfigurationAsync(path, configuration, cancellationToken);
        }
    }

    public async Task<AuthenticationRecord?> ReadAuthenticationRecordAsync(CancellationToken cancellationToken = default)
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
    public async Task ClearTokenCache(CancellationToken cancellationToken = default)
    {
        // https://learn.microsoft.com/en-us/azure/active-directory/develop/msal-net-clear-token-cache
        // Work-around until https://github.com/Azure/azure-sdk-for-net/issues/32048 is resolved
        var options = await ReadAuthenticationIdentifiersAsync(cancellationToken);
        var record = await ReadAuthenticationRecordAsync(cancellationToken);
        var clientId = record?.ClientId ?? options?.ClientId;
        var tenantId = record?.TenantId ?? options?.TenantId;
        if (options == null || string.IsNullOrWhiteSpace(clientId))
        {
            return;
        }

        IClientApplicationBase app;
        // MSAL doesn't have an API to clear the token cache on confidential clients.
        // See https://learn.microsoft.com/en-us/azure/active-directory/develop/msal-net-clear-token-cache#web-api-and-daemon-apps
        if (!options.Strategy.IsPrivateClient())
        {
            var cacheHelper = await GetProtectedCacheHelperAsync(Constants.TokenCacheName, cancellationToken);
            var appBuilder = PublicClientApplicationBuilder.Create(clientId);
            app = appBuilder.Build();

            cacheHelper.RegisterCache(app.UserTokenCache);

            var accounts = await app.GetAccountsAsync();
            var accountsIter = accounts.GetEnumerator();
            while (accountsIter.MoveNext())
            {
                await app.RemoveAsync(accountsIter.Current);
            }

            cacheHelper.UnregisterCache(app.UserTokenCache);
        }

        DeleteAuthenticationIdentifiers();
        DeleteAuthenticationRecord();
    }

    private void DeleteAuthenticationIdentifiers()
    {
        var authIdentifierPath = GetAuthenticationCacheFilePath();
        if (File.Exists(authIdentifierPath))
        {
            File.Delete(authIdentifierPath);
        }
    }

    private void DeleteAuthenticationRecord()
    {
        var authRecordPath = Path.Combine(pathUtility.GetApplicationDataDirectory(), Constants.AuthRecordPath);
        if (File.Exists(authRecordPath))
        {
            File.Delete(authRecordPath);
        }
    }

    private async Task<ConfigurationRoot?> ReadConfigurationAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var path = this.GetAuthenticationCacheFilePath();
        if (!File.Exists(path))
        {
            return null;
        }

        using var fileStream = File.OpenRead(path);

        try
        {
            return await JsonSerializer.DeserializeAsync<Configuration.ConfigurationRoot>(fileStream, cancellationToken: cancellationToken);
        }
        catch (Exception)
        {
            // Don't fail on invalid JSON
            return null;
        }
    }

    private async Task WriteConfigurationAsync(string path, ConfigurationRoot configuration, CancellationToken cancellationToken = default, int retryCount = 0)
    {
        try
        {
            using FileStream fileStream = File.Open(path, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(fileStream, configuration, cancellationToken: cancellationToken);
        }
        catch (DirectoryNotFoundException)
        {
            Directory.CreateDirectory(path);
            if (retryCount < 1)
                await WriteConfigurationAsync(path, configuration, cancellationToken, retryCount + 1);
        }
    }

    private async Task<MsalCacheHelper> GetProtectedCacheHelperAsync(string name, CancellationToken cancellationToken = default)
    {
        // https://github.com/Azure/azure-sdk-for-net/blob/4b2579556b7271587d2fb122163e23090a043597/sdk/identity/Azure.Identity/src/Constants.cs
        string cacheDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ".IdentityService");
        const string keychainService = "Microsoft.Developer.IdentityService";
        const string keyringSchema = "msal.cache";
        const string keyringCollection = "default";
        KeyValuePair<string, string> keyringAttribute1 = new("MsalClientID", "Microsoft.Developer.IdentityService");
        KeyValuePair<string, string> keyringAttribute2 = new("Microsoft.Developer.IdentityService", "1.0.0.0");
        StorageCreationProperties storageProperties = new StorageCreationPropertiesBuilder(name, cacheDir)
            .WithMacKeyChain(keychainService, name)
            .WithLinuxKeyring(keyringSchema, keyringCollection, name, keyringAttribute1, keyringAttribute2)
            .Build();

        return await MsalCacheHelper.CreateAsync(storageProperties);
    }

    public class AuthenticationIdentifierException : Exception
    {
        public AuthenticationIdentifierException()
        {
        }

        public AuthenticationIdentifierException(string? message) : base(message)
        {
        }

        public AuthenticationIdentifierException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
