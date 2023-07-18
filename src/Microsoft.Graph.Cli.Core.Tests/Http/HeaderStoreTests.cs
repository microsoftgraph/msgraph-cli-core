using System.Linq;
using Microsoft.Graph.Cli.Core.Http;
using Xunit;

namespace Microsoft.Graph.Cli.Core.Tests.Http;

public class HeaderStoreTests
{
    [Fact]
    public void Stores_Single_Header()
    {
        var header = new [] {"sample=header"};
        HeadersStore.Instance.SetHeadersFromStrings(header);

        Assert.NotEmpty(HeadersStore.Instance.Headers);
        Assert.Equal(HeadersStore.Instance.Headers.Count(), 1);
        Assert.Equal(HeadersStore.Instance.Headers.First().Value.Count, 1);
    }

    [Fact]
    public void Stores_Multiple_Headers()
    {
        var header = new [] {"sample=header", "sample2=header2", };
        HeadersStore.Instance.SetHeadersFromStrings(header);

        Assert.NotEmpty(HeadersStore.Instance.Headers);
        Assert.Equal(HeadersStore.Instance.Headers.Count(), 2);
        Assert.Equal(HeadersStore.Instance.Headers.First().Value.Count, 1);
    }

    [Fact]
    public void Stores_Multiple_Headers_With_Matching_Key()
    {
        var header = new [] {"sample=header", "sample=header2", };
        HeadersStore.Instance.SetHeadersFromStrings(header);

        Assert.NotEmpty(HeadersStore.Instance.Headers);
        Assert.Equal(HeadersStore.Instance.Headers.Count(), 1);
        Assert.Equal(HeadersStore.Instance.Headers.First().Value.Count, 2);
    }
}
