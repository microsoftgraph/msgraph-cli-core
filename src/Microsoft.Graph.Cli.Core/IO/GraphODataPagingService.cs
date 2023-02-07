using Microsoft.Kiota.Cli.Commons.IO;

namespace Microsoft.Graph.Cli.Core.IO;

/// <summary>
/// The graph OData paging service
/// </summary>
public class GraphODataPagingService : ODataPagingService
{
    /// <inheritdoc />
    public override bool OnBeforeGetPagedData(PageLinkData pageLinkData, bool fetchAllPages = false)
    {
        // Set the page size to 999 if the user asked to fetch all pages and top either isn't specified or is invalid
        if (fetchAllPages && (!pageLinkData.RequestInformation.QueryParameters.TryGetValue("%24top", out var topVal) || (topVal as int?) == null || (topVal as int?) < 1))
        {
            pageLinkData.RequestInformation.QueryParameters["%24top"] = 999;
        }
        return true;
    }
}
