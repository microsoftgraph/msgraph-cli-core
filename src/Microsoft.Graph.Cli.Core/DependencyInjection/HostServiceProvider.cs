using System;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Graph.Cli.Core.IO;

public class HostServiceProvider : IServiceProvider
{
    private IHost? host;

    public HostServiceProvider(IHost? host)
    {
        this.host = host;
    }

    public object? GetService(Type serviceType)
    {
        return host?.Services.GetService(serviceType);
    }
}
