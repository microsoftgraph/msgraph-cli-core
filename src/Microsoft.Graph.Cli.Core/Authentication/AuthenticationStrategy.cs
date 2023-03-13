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
    /// Environment strategy. Enables authentication using environment variables. Supports certificate file & client secret login
    /// </summary>
    Environment
}

public static class AuthenticationStrategyExtensions
{
    public static bool IsPrivateClient(this AuthenticationStrategy strategy)
    {
        return strategy switch
        {
            AuthenticationStrategy.DeviceCode or AuthenticationStrategy.InteractiveBrowser => false,
            AuthenticationStrategy.ClientCertificate or AuthenticationStrategy.Environment => true,
            _ => throw new System.ArgumentOutOfRangeException(nameof(strategy), strategy, "The authentication strategy is invalid")
        };
    }
}
