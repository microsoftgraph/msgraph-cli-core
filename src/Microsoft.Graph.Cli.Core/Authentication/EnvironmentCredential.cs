using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;

namespace Microsoft.Graph.Cli.Core.Authentication
{
    /// <summary>
    /// Enables authentication to Azure Active Directory using a client secret or certificate.
    /// <para>
    /// Configuration is attempted in this order, using these environment variables:
    /// </para>
    ///
    /// <b>Service principal with secret:</b>
    /// <list type="table">
    /// <listheader><term>Variable</term><description>Description</description></listheader>
    /// <item><term>AZURE_TENANT_ID</term><description>The Azure Active Directory tenant (directory) ID.</description></item>
    /// <item><term>AZURE_CLIENT_ID</term><description>The client (application) ID of an App Registration in the tenant.</description></item>
    /// <item><term>AZURE_CLIENT_SECRET</term><description>A client secret that was generated for the App Registration.</description></item>
    /// </list>
    ///
    /// <b>Service principal with certificate:</b>
    /// <list type="table">
    /// <listheader><term>Variable</term><description>Description</description></listheader>
    /// <item><term>AZURE_TENANT_ID</term><description>The Azure Active Directory tenant (directory) ID.</description></item>
    /// <item><term>AZURE_CLIENT_ID</term><description>The client (application) ID of an App Registration in the tenant.</description></item>
    /// <item><term>AZURE_CLIENT_CERTIFICATE_PATH</term><description>A path to certificate and private key pair in PEM or PFX format, which can authenticate the App Registration.</description></item>
    /// <item><term>AZURE_CLIENT_CERTIFICATE_PASSWORD</term><description>(Optional) The password protecting the certificate file (currently only supported for PFX (PKCS12) certificates).</description></item>
    /// <item><term>AZURE_CLIENT_SEND_CERTIFICATE_CHAIN</term><description>(Optional) Specifies whether an authentication request will include an x5c header to support subject name / issuer based authentication. When set to `true` or `1`, authentication requests include the x5c header.</description></item>
    /// </list>
    ///
    /// This credential ultimately uses a <see cref="ClientSecretCredential"/>, <see cref="ClientCertificateCredential"/> to
    /// perform the authentication using these details. Please consult the
    /// documentation of that class for more details.
    /// </summary>
    public class EnvironmentCredential : TokenCredential
    {
        private const string UnavailableErrorMessage = "EnvironmentCredential authentication unavailable. Environment variables are not fully configured. See the troubleshooting guide for more information. https://aka.ms/azsdk/net/identity/environmentcredential/troubleshoot";
        private readonly TokenCredentialOptions _options;

        internal TokenCredential? Credential { get; }

        /// <summary>
        /// Creates a new environment credential.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="clientId">Client Id</param>
        /// <param name="options">Credential options</param>
        public EnvironmentCredential(string? tenantId, string? clientId, TokenCredentialOptions? options = null)
        {
            _options = options ?? new TokenCredentialOptions();

            tenantId = Environment.GetEnvironmentVariable(Utils.Constants.Environment.TenantId) ?? tenantId;
            clientId = Environment.GetEnvironmentVariable(Utils.Constants.Environment.ClientId) ?? clientId;
            string? clientSecret = Environment.GetEnvironmentVariable(Utils.Constants.Environment.ClientSecret);
            string? clientCertificatePath = Environment.GetEnvironmentVariable(Utils.Constants.Environment.ClientCertificatePath);
            string? clientCertificatePassword = Environment.GetEnvironmentVariable(Utils.Constants.Environment.ClientCertificatePassword);
            string? clientSendCertificateChain = Environment.GetEnvironmentVariable(Utils.Constants.Environment.ClientSendCertificateChain);

            if (!string.IsNullOrEmpty(tenantId) && !string.IsNullOrEmpty(clientId))
            {
                if (!string.IsNullOrEmpty(clientSecret))
                {
                    Credential = new ClientSecretCredential(tenantId, clientId, clientSecret, _options);
                }
                else if (!string.IsNullOrEmpty(clientCertificatePath))
                {
                    bool sendCertificateChain = !string.IsNullOrEmpty(clientSendCertificateChain) &&
                        (clientSendCertificateChain == "1" || clientSendCertificateChain == "true");

                    ClientCertificateCredentialOptions clientCertificateCredentialOptions = new ClientCertificateCredentialOptions
                    {
                        AuthorityHost = _options.AuthorityHost,
                        Transport = _options.Transport,
                        SendCertificateChain = sendCertificateChain
                    };
                    // Use reflection to set internal properties.
                    X509Certificate2? cert;
                    if (ClientCertificateCredentialFactory.TryGetCertificateFromFile(clientCertificatePath, clientCertificatePassword, out cert))
                    {
                        Credential = new ClientCertificateCredential(tenantId, clientId, cert, clientCertificateCredentialOptions);
                    }
                }
            }
        }

        /// <summary>
        /// Obtains a token from the Azure Active Directory service, using the specified client details specified in the environment variables
        /// AZURE_TENANT_ID, AZURE_CLIENT_ID, and AZURE_CLIENT_SECRET or AZURE_USERNAME and AZURE_PASSWORD to authenticate.
        /// Acquired tokens are cached by the credential instance. Token lifetime and refreshing is handled automatically. Where possible,
        /// reuse credential instances to optimize cache effectiveness.
        /// </summary>
        /// <remarks>
        /// If the environment variables AZURE_TENANT_ID, AZURE_CLIENT_ID, and AZURE_CLIENT_SECRET are not specified, the default <see cref="AccessToken"/>
        /// </remarks>
        /// <param name="requestContext">The details of the authentication request.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> controlling the request lifetime.</param>
        /// <returns>An <see cref="AccessToken"/> which can be used to authenticate service client calls.</returns>
        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken = default)
        {
            return GetCredentialOrFail().GetToken(requestContext, cancellationToken);
        }

        /// <summary>
        /// Obtains a token from the Azure Active Directory service, using the specified client details specified in the environment variables
        /// AZURE_TENANT_ID, AZURE_CLIENT_ID, and AZURE_CLIENT_SECRET or AZURE_USERNAME and AZURE_PASSWORD to authenticate.
        /// Acquired tokens are cached by the credential instance. Token lifetime and refreshing is handled automatically. Where possible,
        /// reuse credential instances to optimize cache effectiveness.
        /// </summary>
        /// <remarks>
        /// If the environment variables AZURE_TENANT_ID, AZURE_CLIENT_ID, and AZURE_CLIENT_SECRET are not specified, the default <see cref="AccessToken"/>
        /// </remarks>
        /// <param name="requestContext">The details of the authentication request.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> controlling the request lifetime.</param>
        /// <returns>An <see cref="AccessToken"/> which can be used to authenticate service client calls, or a default <see cref="AccessToken"/>.</returns>
        public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken = default)
        {
            return await GetCredentialOrFail().GetTokenAsync(requestContext, cancellationToken).ConfigureAwait(false);
        }

        private TokenCredential GetCredentialOrFail()
        {
            return Credential ?? throw new CredentialUnavailableException(UnavailableErrorMessage);
        }
    }
}
