using Microsoft.Graph.Cli.Core.Authentication;

namespace Microsoft.Graph.Cli.Core.Configuration;

/// <summary>
/// Authentication options
/// </summary>
public class AuthenticationOptions
{
    /// <summary>
    /// Authority
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
}
