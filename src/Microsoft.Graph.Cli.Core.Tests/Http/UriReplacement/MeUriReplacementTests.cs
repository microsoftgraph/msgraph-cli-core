using System;
using Microsoft.Graph.Cli.Core.Http.UriReplacement;
using Xunit;

namespace Microsoft.Graph.Cli.Core.Tests.Http.UriReplacement;

public class MeUriReplacementTests
{
    [Fact]
    public void Returns_Null_When_Given_A_Null_Url()
    {
        var replacement = new MeUriReplacement();

        Assert.Null(replacement.Replace(null));
    }

    [Theory]
    [InlineData("http://example.com/test")]
    [InlineData("http://example.com/users/messages")]
    [InlineData("http://example.com/v1.0/users/messages")]
    [InlineData("http://example.com/users/test/me")]
    [InlineData("http://example.com/a/b/users/test/me")]
    public void Returns_Original_Uri_When_No_Match_Is_Found(string inputUri)
    {
        var uri = new Uri(inputUri);
        var replacement = new MeUriReplacement();

        Assert.Equal(uri, replacement.Replace(uri));
    }

    [Theory]
    [InlineData("http://example.com/v1.0/users/me/messages", "http://example.com/v1.0/me/messages")]
    [InlineData("http://example.com/v1.0/users/me", "http://example.com/v1.0/me")]
    [InlineData("http://example.com/v1.0/users/me?a=b", "http://example.com/v1.0/me?a=b")]
    public void Returns_A_New_Url_When_A_Match_Is_Found(string inputUri, string expectedUri)
    {
        var replacement = new MeUriReplacement();

        var uri = new Uri(inputUri);
        Assert.Equal(expectedUri, replacement.Replace(uri)!.ToString());
    }
}
