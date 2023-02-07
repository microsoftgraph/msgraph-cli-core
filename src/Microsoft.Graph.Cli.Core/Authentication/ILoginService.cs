using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Graph.Cli.Core.Authentication;

public interface ILoginService
{
    Task LoginAsync(string[] scopes, CancellationToken cancellationToken = default);
}
