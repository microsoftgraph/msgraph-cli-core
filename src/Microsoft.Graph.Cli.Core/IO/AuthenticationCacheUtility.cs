using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph.Cli.Core.Authentication;
using Microsoft.Graph.Cli.Core.Configuration;
using Microsoft.Graph.Cli.Core.Utils;

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

    public async Task SaveAuthenticationIdentifiersAsync(string? clientId, string? tenantId, AuthenticationStrategy strategy, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var path = this.GetAuthenticationCacheFilePath();
        ConfigurationRoot configuration = await ReadConfigurationAsync(cancellationToken) ?? new Configuration.ConfigurationRoot();
        var authOptions = configuration.AuthenticationOptions;
        clientId = clientId ?? Constants.DefaultAppId;
        tenantId = tenantId ?? Constants.DefaultTenant;

        // Only write auth configuration if the values have changed
        if (clientId != authOptions.ClientId || tenantId != authOptions.TenantId || strategy != authOptions.Strategy)
        {
            configuration.AuthenticationOptions = new AuthenticationOptions
            {
                ClientId = clientId,
                TenantId = tenantId,
                Strategy = strategy,
            };

            await WriteConfigurationAsync(path, configuration, cancellationToken);
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
