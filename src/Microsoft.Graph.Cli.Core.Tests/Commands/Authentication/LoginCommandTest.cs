using System.CommandLine.Parsing;
using System.Linq;
using System.Threading.Tasks;
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
        var pathUtilityMock = new Mock<IPathUtility>();
        var authServiceFactoryMock = new Mock<AuthenticationServiceFactory>(pathUtilityMock.Object, new AuthenticationOptions());
        var loginCommandBuilder = new LoginCommand(authServiceFactoryMock.Object);
        var command = loginCommandBuilder.Build();
        var parser = new Parser(command);

        // When
        var result = parser.Parse("login");
        var scopes = result.FindResultFor(command.Options[0])?.Tokens.Select(t => t.Value);

        // Then
        Assert.Null(scopes);
    }

    [Fact]
    public void Parses_Single_Scope()
    {
        // Given
        var pathUtilityMock = new Mock<IPathUtility>();
        var authServiceFactoryMock = new Mock<AuthenticationServiceFactory>(pathUtilityMock.Object, new AuthenticationOptions());
        var loginCommandBuilder = new LoginCommand(authServiceFactoryMock.Object);
        var command = loginCommandBuilder.Build();
        var parser = new Parser(command);

        // When
        var result = parser.Parse("login --scopes User.Read");
        var scopes = result.FindResultFor(command.Options[0])?.Tokens.Select(t => t.Value);

        // Then
        Assert.NotEmpty(scopes);
        Assert.Contains("User.Read", scopes);
    }

    [Theory]
    [InlineData("login --scopes User.Read --scopes Mail.Read")]
    [InlineData("login --scopes User.Read Mail.Read")]
    public void Parses_Multiple_Scopes(string commandString)
    {
        // Given
        var pathUtilityMock = new Mock<IPathUtility>();
        var authServiceFactoryMock = new Mock<AuthenticationServiceFactory>(pathUtilityMock.Object, new AuthenticationOptions());
        var loginCommandBuilder = new LoginCommand(authServiceFactoryMock.Object);
        var command = loginCommandBuilder.Build();
        var parser = new Parser(command);

        // When
        var result = parser.Parse(commandString);
        var scopes = result.FindResultFor(command.Options[0])?.Tokens.Select(t => t.Value);

        // Then
        Assert.NotEmpty(scopes);
        Assert.Contains("User.Read", scopes);
        Assert.Contains("Mail.Read", scopes);
    }
}
