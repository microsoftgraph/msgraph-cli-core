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
    ClientCertificate
}
