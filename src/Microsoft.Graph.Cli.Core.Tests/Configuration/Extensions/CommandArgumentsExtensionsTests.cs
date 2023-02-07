using System.Collections.Generic;
using Microsoft.Graph.Cli.Core.Configuration.Extensions;
using Xunit;

namespace Microsoft.Graph.Cli.Core.Tests.Configuration.Extensions;

public class CommandArgumentsExtensionsTests
{
    [Fact]
    public void ReturnsEmptyArrayWhenInputIsEmpty()
    {
        // Given
        var args = new string[] { };

        // When
        var result = args.ExpandFlagsForConfiguration();

        // Then
        Assert.Empty(result);
    }

    [Fact]
    public void ReturnsIdenticalArrayWhenInputFlagsHaveValues()
    {
        // Given
        var args = new string[] { "--test", "value", "--test2", "false" };

        // When
        var result = args.ExpandFlagsForConfiguration();

        // Then
        Assert.Equal(args, result);
    }

    public static IEnumerable<object[]> GetArgsData()
    {
        yield return new[] {
            new string[] { "--test", "value", "--test2", "--test3", "value3" },
            new string[] { "--test", "value", "--test2", "true", "--test3", "value3" }
        };
        yield return new[] {
            new string[] { "--test", "value", "--test2" },
            new string[] { "--test", "value", "--test2", "true" }
        };
        yield return new[] {
            new string[] { "--test" },
            new string[] { "--test", "true" }
        };
        yield return new[] {
            new string[] { "--test", "--test2" },
            new string[] { "--test", "true", "--test2", "true" }
        };
        yield return new[] {
            new string[] { "--test", "--test2", "value2" },
            new string[] { "--test", "true", "--test2", "value2" }
        };
        yield return new[] {
            new string[] { "-t", "-t2" },
            new string[] { "-t", "true", "-t2", "true" }
        };
    }

    [Theory]
    [MemberData(nameof(GetArgsData))]
    public void ReturnsUpdatedArrayWhenInputFlagsHaveNoValues(string[] input, string[] expectedOutput)
    {
        // Given
        var args = input;

        // When
        var result = args.ExpandFlagsForConfiguration();

        // Then
        Assert.Equal(expectedOutput, result);
    }
}
