using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Microsoft.Graph.Cli.Core.Tests.Http;

internal class TestLogger<T> : ILogger<T>
{
    private readonly List<string> messages = new();
    private LogLevel level = LogLevel.Trace;

    public IList<string> Messages => messages;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= level;
    }

    public void SetLevel(LogLevel logLevel) {
        level = logLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        messages.Add(formatter(state, exception));
    }
}
