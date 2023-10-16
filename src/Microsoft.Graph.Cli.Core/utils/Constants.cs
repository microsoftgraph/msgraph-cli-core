using Microsoft.Graph.Cli.Core.Authentication;

namespace Microsoft.Graph.Cli.Core.Utils
{
    /// <summary>
    /// Graph CLI Core Constants
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Name of the CLI core directory in the APPDATA root.
        /// </summary>
        public const string ApplicationDataDirectory = ".mgc";

        /// <summary>
        /// Name of the auth record file.
        /// </summary>
        public const string AuthRecordPath = "authRecord";

        /// <summary>
        /// Name of the authentication cache file
        /// </summary>
        public const string AuthenticationIdCachePath = "authentication-id-cache.json";

        /// <summary>
        /// Name of the token cache.
        /// </summary>
        public const string TokenCacheName = "MicrosoftGraph";

        /// <summary>
        /// The default AppId.
        /// </summary>
        public const string DefaultAppId = "14d82eec-204b-4c2f-b7e8-296a70dab67e";

        /// <summary>
        /// Default tenant id.
        /// </summary>
        public const string DefaultTenant = "common";

        /// <summary>
        /// Default authority
        /// </summary>
        public const string DefaultAuthority = "https://login.microsoftonline.com";

        /// <summary>
        /// Environmrnt constants.
        /// </summary>
        public static class Environment
        {
            /// <summary>
            /// Tenant Id.
            /// </summary>
            public static string TenantId = "AZURE_TENANT_ID";

            /// <summary>
            /// Client Id.
            /// </summary>
            public static string ClientId = "AZURE_CLIENT_ID";

            /// <summary>
            /// Client secret
            /// </summary>
            public static string ClientSecret = "AZURE_CLIENT_SECRET";

            /// <summary>
            /// Client certificate path
            /// </summary>
            public static string ClientCertificatePath = "AZURE_CLIENT_CERTIFICATE_PATH";

            /// <summary>
            /// Client certificate password.
            /// </summary>
            public static string ClientCertificatePassword = "AZURE_CLIENT_CERTIFICATE_PASSWORD";

            /// <summary>
            /// Client send certificate chain.
            /// </summary>
            public static string ClientSendCertificateChain = "AZURE_CLIENT_SEND_CERTIFICATE_CHAIN";
        }

        /// <summary>
        /// The default auth strategy to use.
        /// </summary>
        public const AuthenticationStrategy defaultAuthStrategy = AuthenticationStrategy.DeviceCode;
    }
}
