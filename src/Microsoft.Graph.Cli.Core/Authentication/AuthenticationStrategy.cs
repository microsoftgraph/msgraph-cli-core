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
    InteractiveBrowser
}
