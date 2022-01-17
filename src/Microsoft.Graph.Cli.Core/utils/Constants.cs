using Microsoft.Graph.Cli.Core.Authentication;

namespace Microsoft.Graph.Cli.Core.Utils
{
    class Constants
    {
        public const string ApplicationDataDirectory = ".mgc";

        public const string AuthRecordPath = "authRecord";

        public const string TokenCacheName = "MicrosoftGraph";

        public const string AuthenticationSection = "Authentication";

        public const AuthenticationStrategy defaultAuthStrategy = AuthenticationStrategy.DeviceCode;
    }
}