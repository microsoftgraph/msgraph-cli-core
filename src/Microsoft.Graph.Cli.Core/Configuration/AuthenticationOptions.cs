using Microsoft.Graph.Cli.Core.Authentication;

namespace Microsoft.Graph.Cli.Core.Configuration;

/// <summary>
/// Authentication options
/// </summary>
public class AuthenticationOptions
{
    /// <summary>
    /// Entra authority. Corresponds to the <see cref="CloudEnvironment"/> via the
    /// <see cref="CloudEnvironmentExtensions.Authority"/> convenience extension.
    /// </summary>
    public string? Authority { get; set; }

    /// <summary>
    /// Tenant (App) Id
    /// </summary>
    public string? TenantId { get; set; }

    /// <summary>
    /// Client Id
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Client certificate name.
    /// </summary>
    public string? ClientCertificateName { get; set; }

    /// <summary>
    /// Client certificate thumb-print
    /// </summary>
    public string? ClientCertificateThumbPrint { get; set; }

    /// <summary>
    /// Authentication strategy
    /// </summary>
    public AuthenticationStrategy Strategy { get; set; } = AuthenticationStrategy.DeviceCode;

    /// <summary>
    /// Cloud environment for authentication. This is stored separately from
    /// Authority in case some environments use the same authority.
    /// </summary>
    public CloudEnvironment Environment { get; set; } = CloudEnvironment.Global;
}
