using System.CommandLine.Parsing;
using System.Linq;
using Microsoft.Graph.Cli.Core.Authentication;
using Microsoft.Graph.Cli.Core.Commands.Authentication;
using Microsoft.Graph.Cli.Core.Configuration;
using Microsoft.Graph.Cli.Core.IO;
using Moq;
using Xunit;

namespace Microsoft.Graph.Cli.Core.Tests.Commands.Authentication;

public class LoginCommandTest
{
    [Fact]
    public void Parses_No_Scopes()
    {
        // Given
        var command = new LoginCommand();
        var parser = new Parser(command);

        // When
        var result = parser.Parse("login");
        var scopes = result.FindResultFor(command.Options[0])?.Tokens.Select(t => t.Value);

        // Then
        Assert.NotNull(scopes);
        Assert.Empty(scopes);
    }

    [Fact]
    public void Parses_Single_Scope()
    {
        // Given
        var command = new LoginCommand();
        var parser = new Parser(command);

        // When
        var result = parser.Parse("login --scopes User.Read");
        var scopes = result.FindResultFor(command.Options[0])?.Tokens.Select(t => t.Value);

        // Then
        Assert.NotNull(scopes);
        Assert.NotEmpty(scopes);
        Assert.Contains("User.Read", scopes);
    }

    [Theory]
    [InlineData("login --scopes User.Read --scopes Mail.Read")]
    [InlineData("login --scopes User.Read Mail.Read")]
    public void Parses_Multiple_Scopes(string commandString)
    {
        // Given
        var command = new LoginCommand();
        var parser = new Parser(command);

        // When
        var result = parser.Parse(commandString);
        var scopes = result.FindResultFor(command.Options[0])?.Tokens.Select(t => t.Value);

        // Then
        Assert.NotNull(scopes);
        Assert.NotEmpty(scopes);
        Assert.Contains("User.Read", scopes);
        Assert.Contains("Mail.Read", scopes);
    }
}
