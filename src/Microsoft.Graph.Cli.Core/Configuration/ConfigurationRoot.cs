namespace Microsoft.Graph.Cli.Core.Configuration;

/// <summary>
/// Application configuration
/// </summary>
public class ConfigurationRoot
{
    /// <summary>
    /// Authentication options
    /// </summary>
    public AuthenticationOptions AuthenticationOptions { get; set; } = new AuthenticationOptions();
}
