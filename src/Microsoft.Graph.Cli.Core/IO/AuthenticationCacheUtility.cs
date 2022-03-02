using System.Text;

namespace Microsoft.Graph.Cli.Core.IO;

public class AuthenticationCacheUtility : IAuthenticationCacheUtility {
    private readonly IPathUtility pathUtility;

    private const string AUTHENTICATION_ID_FILE = "authentication-id";

    public AuthenticationCacheUtility(IPathUtility pathUtility)
    {
        this.pathUtility = pathUtility;
    }

    public async Task<(string, string)> ReadAuthenticationIdentifiersAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var path = this.GetAuthenticationIdFilePath();
        if (!File.Exists(path)) {
            throw new FileNotFoundException();
        }

        var text = await File.ReadAllTextAsync(path, cancellationToken);
        if (string.IsNullOrWhiteSpace(text)) throw new AuthenticationIdentifierException("The authentication identifier cache file is empty");
        var split = text.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (split.Length != 2) throw new AuthenticationIdentifierException("The authentication identifier cache file cannot be parsed");
        
        return (split[0], split[1]);
    }

    public async Task SaveAuthenticationIdentifiersAsync(string clientId, string tenantId, CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();
        var path = this.GetAuthenticationIdFilePath();
        var data = $"{clientId}:{tenantId}";
        await File.WriteAllTextAsync(path, data, cancellationToken);
    }

    private string GetAuthenticationIdFilePath() {
        return Path.Join(pathUtility.GetApplicationDataDirectory(), AUTHENTICATION_ID_FILE);
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