using System.Threading.Tasks;

namespace Microsoft.Graph.Cli.Core.Authentication;

interface ILoginService {
    Task LoginAsync(string[] scopes);
}