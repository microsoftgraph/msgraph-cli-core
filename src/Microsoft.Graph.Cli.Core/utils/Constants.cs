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

        public const string DefaultCertificateScope = "https://graph.microsoft.com/.default";

        public const AuthenticationStrategy defaultAuthStrategy = AuthenticationStrategy.DeviceCode;
    }
}
