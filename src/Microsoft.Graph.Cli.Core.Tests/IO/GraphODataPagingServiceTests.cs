using System.IO;
using Microsoft.Graph.Cli.Core.IO;
using Microsoft.Kiota.Cli.Commons.IO;
using Xunit;

namespace Microsoft.Graph.Cli.Core.Tests.IO;

public class GraphODataPagingServiceTests
{
    [Fact]
    public void Sets_Top_If_Value_Is_Not_Set()
    {
        var pageLinkData = new PageLinkData(new(), Stream.Null);
        var pagingService = new GraphODataPagingService();

        var result = pagingService.OnBeforeGetPagedData(pageLinkData, true);

        Assert.True(result);
        Assert.Equal(999, pageLinkData.RequestInformation.QueryParameters["%24top"]);
    }

    [Theory]
    [InlineData(0)]
#pragma warning disable xUnit1012
    [InlineData(null)]
#pragma warning restore xUnit1012
    [InlineData("test")]
    public void Sets_Top_If_Value_Is_Invalid(object topValue)
    {
        var pageLinkData = new PageLinkData(new(), Stream.Null);
        pageLinkData.RequestInformation.QueryParameters.Add("%24top", topValue);
        var pagingService = new GraphODataPagingService();

        var result = pagingService.OnBeforeGetPagedData(pageLinkData, true);

        Assert.True(result);
        Assert.Equal(999, pageLinkData.RequestInformation.QueryParameters["%24top"]);
    }

    [Fact]
    public void Does_Not_Set_Top_If_Value_Is_Set()
    {
        var pageLinkData = new PageLinkData(new(), Stream.Null);
        pageLinkData.RequestInformation.QueryParameters.Add("%24top", 20);
        var pagingService = new GraphODataPagingService();

        var result = pagingService.OnBeforeGetPagedData(pageLinkData, true);

        Assert.True(result);
        Assert.Equal(20, pageLinkData.RequestInformation.QueryParameters["%24top"]);
    }
}
