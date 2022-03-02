using Microsoft.Graph.Cli.Core.Authentication;

namespace Microsoft.Graph.Cli.Core.Utils
{
    public class Constants
    {
        public const string ApplicationDataDirectory = ".mgc";

        public const string AuthRecordPath = "authRecord";

        public const string AuthenticationIdCachePath = "authentication-id-cache.json";

        public const string TokenCacheName = "MicrosoftGraph";

        public const AuthenticationStrategy defaultAuthStrategy = AuthenticationStrategy.DeviceCode;
    }
}