using System.IO;
using Microsoft.Graph.Cli.Core.Utils;

namespace Microsoft.Graph.Cli.Core.Authentication;

public class LogoutService {
    public void Logout() {
        if (File.Exists(Constants.AuthRecordPath)) {
            File.Delete(Constants.AuthRecordPath);
        }
    }
}