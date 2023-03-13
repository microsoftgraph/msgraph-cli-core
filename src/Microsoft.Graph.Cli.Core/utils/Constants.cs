using Microsoft.Graph.Cli.Core.Authentication;

namespace Microsoft.Graph.Cli.Core.Utils
{
    public class Constants
    {
        public const string ApplicationDataDirectory = ".mgc";

        public const string AuthRecordPath = "authRecord";

        public const string AuthenticationIdCachePath = "authentication-id-cache.json";

        public const string TokenCacheName = "MicrosoftGraph";

        public const string DefaultAppId = "14d82eec-204b-4c2f-b7e8-296a70dab67e";

        public const string DefaultTenant = "common";

        public const string DefaultAuthority = "https://login.microsoftonline.com";

        public static class Environment
        {
            public static string TenantId = "AZURE_TENANT_ID";
            public static string ClientId = "AZURE_CLIENT_ID";
            public static string ClientSecret = "AZURE_CLIENT_SECRET";
            public static string ClientCertificatePath = "AZURE_CLIENT_CERTIFICATE_PATH";
            public static string ClientCertificatePassword = "AZURE_CLIENT_CERTIFICATE_PASSWORD";
            public static string ClientSendCertificateChain = "AZURE_CLIENT_SEND_CERTIFICATE_CHAIN";
        }

        public const AuthenticationStrategy defaultAuthStrategy = AuthenticationStrategy.DeviceCode;
    }
}
