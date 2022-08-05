using Microsoft.Graph.Cli.Core.Authentication;

namespace Microsoft.Graph.Cli.Core.Configuration;

public class AuthenticationOptions
{
    public string? TenantId { get; set; }

    public string? ClientId { get; set; }

    public string? ClientCertificateName { get; set; }

    public string? ClientCertificatePath { get; set; }

    public string? ClientCertificateThumbPrint { get; set; }

    public AuthenticationStrategy Strategy { get; set; } = AuthenticationStrategy.DeviceCode;
}
