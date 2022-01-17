using System.Threading.Tasks;

namespace Microsoft.Graph.Cli.Core.Authentication;

public interface ILoginService {
    Task LoginAsync(string[] scopes);
}