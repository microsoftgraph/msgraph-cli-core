namespace Microsoft.Graph.Cli.Core.Authentication;

/// <summary>
/// The authentication strategy to use
/// </summary>
public enum AuthenticationStrategy
{
    /// <summary>
    /// Device code strategy
    /// </summary>
    DeviceCode,
    /// <summary>
    /// Interactive browser strategy. Opens a user's browser.
    /// </summary>
    InteractiveBrowser,
    /// <summary>
    /// Client certificate strategy. Enables authentication of a service principal in to Azure Active Directory using a X509 certificate
    /// that is assigned to it's App Registration
    /// </summary>
    ClientCertificate,
    /// <summary>
    /// Managed Identity strategy. Enables authentication using a managed identity.
    /// </summary>
    ManagedIdentity,
    /// <summary>
    /// Environment strategy. Enables authentication using environment variables. Supports certificate file &amp; client secret login
    /// </summary>
    Environment
}

/// <summary>
/// Authentication strategy extensions.
/// </summary>
public static class AuthenticationStrategyExtensions
{
    /// <summary>
    /// Returns a boolean indicating if the authentication strategy uses a private client.
    /// </summary>
    /// <param name="strategy">The authentication strategy</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentOutOfRangeException">When an unsupported authentication strategy is used.</exception>
    public static bool IsPrivateClient(this AuthenticationStrategy strategy)
    {
        return strategy switch
        {
            AuthenticationStrategy.DeviceCode or AuthenticationStrategy.InteractiveBrowser => false,
            AuthenticationStrategy.ClientCertificate or AuthenticationStrategy.Environment or AuthenticationStrategy.ManagedIdentity => true,
            _ => throw new System.ArgumentOutOfRangeException(nameof(strategy), strategy, "The authentication strategy is invalid")
        };
    }
}
