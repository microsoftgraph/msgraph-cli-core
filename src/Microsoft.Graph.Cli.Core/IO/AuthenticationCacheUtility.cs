using System.Text;
using System.Text.Json;
using Microsoft.Graph.Cli.Core.Configuration;
using Microsoft.Graph.Cli.Core.Utils;

namespace Microsoft.Graph.Cli.Core.IO;

public class AuthenticationCacheUtility : IAuthenticationCacheUtility {
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
        if (!File.Exists(path)) {
            throw new FileNotFoundException();
        }

        using var fileStream = File.OpenRead(path);
        var configRoot = await JsonSerializer.DeserializeAsync<Configuration.ConfigurationRoot>(fileStream, cancellationToken: cancellationToken);
        if (configRoot?.AuthenticationOptions is null) throw new AuthenticationIdentifierException("Cannot find cached authentication identifiers.");
        
        return configRoot.AuthenticationOptions;
    }

    public async Task SaveAuthenticationIdentifiersAsync(string clientId, string tenantId, CancellationToken cancellationToken = default(CancellationToken)) {
        cancellationToken.ThrowIfCancellationRequested();
        var path = this.GetAuthenticationCacheFilePath();
        var configuration = new Configuration.ConfigurationRoot {
            AuthenticationOptions = new AuthenticationOptions {
                ClientId = clientId,
                TenantId = tenantId
            }
        };
        using FileStream fileStream = File.OpenWrite(path);
        await JsonSerializer.SerializeAsync(fileStream, configuration, cancellationToken: cancellationToken);
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