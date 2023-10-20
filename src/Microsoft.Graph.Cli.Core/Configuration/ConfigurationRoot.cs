using System.Text.Json.Serialization;

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

[JsonSerializable(typeof(ConfigurationRoot))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}
