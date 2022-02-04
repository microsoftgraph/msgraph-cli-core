using System;
using System.CommandLine.Binding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph.Cli.Core.IO;

namespace Microsoft.Graph.Cli.Core.Binding {
    public class OutputFormatterFactoryBinder : BinderBase<IOutputFormatterFactory>
    {
        protected override IOutputFormatterFactory GetBoundValue(BindingContext bindingContext)
        {
            var host = bindingContext.GetRequiredService<IHost>();
            return host.Services.GetRequiredService<IOutputFormatterFactory>();
        }
    }
}

