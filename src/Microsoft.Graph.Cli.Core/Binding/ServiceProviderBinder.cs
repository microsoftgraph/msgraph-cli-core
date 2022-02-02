using System.CommandLine.Binding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph.Cli.Core.IO;

namespace Microsoft.Graph.Cli.Core.Binding;

public class ServiceProviderBinder : BinderBase<IServiceProvider>
{
    protected override IServiceProvider GetBoundValue(BindingContext bindingContext)
    {
        var host = bindingContext.GetService<IHost>();
        return new HostServiceProvider(host);
    }
}

