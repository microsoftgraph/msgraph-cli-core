using System.CommandLine.Binding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Graph.Cli.Core.Binding;

class HostBinder : BinderBase<IHost>
{
    protected override IHost GetBoundValue(BindingContext bindingContext)
    {
        return bindingContext.GetRequiredService<IHost>();
    }
}
