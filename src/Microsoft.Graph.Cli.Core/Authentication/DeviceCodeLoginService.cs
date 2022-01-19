using Azure.Core;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.IO;
using Microsoft.Graph.Cli.Core.Utils;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Graph.Cli.Core.Authentication;

public class DeviceCodeLoginService : LoginServiceBase {
    private DeviceCodeCredential credential;

    public DeviceCodeLoginService(DeviceCodeCredential credential, IPathUtility pathUtility) : base(pathUtility) {
        this.credential = credential;
    }

    protected override async Task<AuthenticationRecord> DoLoginAsync(string[] scopes) {
        return await credential.AuthenticateAsync(new TokenRequestContext(scopes));
    }
}