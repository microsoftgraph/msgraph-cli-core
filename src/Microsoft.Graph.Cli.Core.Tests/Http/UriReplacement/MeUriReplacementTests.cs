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

    [Fact]
    public void Returns_Original_Uri_When_No_Match_Is_Found()
    {
        var uri = new Uri("http://example.com/test");
        var replacement = new MeUriReplacement();

        Assert.Equal(uri, replacement.Replace(uri));

        uri = new Uri("http://example.com/users/messages");
        Assert.Equal(uri, replacement.Replace(uri));

        uri = new Uri("http://example.com/v1.0/users/messages");
        Assert.Equal(uri, replacement.Replace(uri));

        uri = new Uri("http://example.com/users/test/me");
        Assert.Equal(uri, replacement.Replace(uri));

        uri = new Uri("http://example.com/a/b/users/test/me");
        Assert.Equal(uri, replacement.Replace(uri));
    }

    [Fact]
    public void Returns_A_New_Url_When_A_Match_Is_Found()
    {
        var replacement = new MeUriReplacement();

        var uri = new Uri("http://example.com/v1.0/users/me/messages");
        Assert.Equal("http://example.com/v1.0/me/messages", replacement.Replace(uri)!.ToString());

        uri = new Uri("http://example.com/v1.0/users/me");
        Assert.Equal("http://example.com/v1.0/me", replacement.Replace(uri)!.ToString());

        uri = new Uri("http://example.com/v1.0/users/me?a=b");
        Assert.Equal("http://example.com/v1.0/me?a=b", replacement.Replace(uri)!.ToString());
    }
}
