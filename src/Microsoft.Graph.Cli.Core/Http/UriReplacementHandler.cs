using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Graph.Cli.Core.Http;

/// <summary>
/// Interface for making URI replacements.
/// </summary>
public interface IUriReplacement
{
    /// <summary>
    /// Accepts a URI and returns a new URI with all replacements applied.
    /// </summary>
    /// <param name="original">The URI to apply replacements to</param>
    /// <returns>A new URI with all replacements applied.</returns>
    Uri? Replace(Uri? original);
}

/// <summary>
/// Specialized replacement for /[version]/users/me with /[version]/me
/// </summary>
public struct MeUriReplacement : IUriReplacement
{
    /// <summary>
    /// If a URI path starts with /[version]/users/me, replace it with /[version]/me
    /// </summary>
    /// <param name="original">The original URI</param>
    /// <returns>A URI with /[version]/users/me replaced with /[version]/me</returns>
    /// <remarks>This method assumes that the first segment after the root is a version segment to match Microsoft Graph API's format.</remarks>
    public readonly Uri? Replace(Uri? original)
    {
        if (original is null)
        {
            return null;
        }

        if (original.Segments.Length < 4)
        {
            // Must have at least segments "/", "[version]/", "users/", "me"
            return original;
        }

        Span<char> toMatch = stackalloc char[9];
        toMatch[0] = '/';
        toMatch[1] = 'u';
        toMatch[2] = 's';
        toMatch[3] = 'e';
        toMatch[4] = 'r';
        toMatch[5] = 's';
        toMatch[6] = '/';
        toMatch[7] = 'm';
        toMatch[8] = 'e';
        var separator = toMatch[..1];
        var matchUsers = toMatch[1..6];
        var matchMe = toMatch[7..];

        var maybeUsersSegment = original.Segments[2].AsSpan();
        if (!maybeUsersSegment[..^1].SequenceEqual(matchUsers))
        {
            return original;
        }

        var maybeMeSegment = original.Segments[3].AsSpan();
        if (!maybeMeSegment[..(maybeMeSegment.EndsWith(separator) ? maybeMeSegment.Length - 1 : maybeMeSegment.Length)].SequenceEqual(matchMe))
        {
            return original;
        }

        var newUrl = new UriBuilder(original);
        var versionSegment = original.Segments[1].AsSpan();
        const int usersMeLength = 9;
        var length = versionSegment.Length + usersMeLength;
        if (newUrl.Path.Length == length)
        {
            // Matched /[version]/users/me
            newUrl.Path = string.Concat(separator, versionSegment, matchMe);
        }
        else
        {
            // Maybe matched /[version]/users/me...
            // Logic to make sure we don't match paths like /users/messages
            var span = newUrl.Path.AsSpan(length);
            if (span[0] == '/')
            {
                newUrl.Path = string.Concat(separator, versionSegment, matchMe, span);
            }
        }

        return newUrl.Uri;
    }
}

/// <summary>
/// Replaces a portion of the URL.
/// </summary>
public class UriReplacementHandler<TUriReplacement> : DelegatingHandler where TUriReplacement: IUriReplacement
{
    private readonly TUriReplacement urlReplacement;

    /// <summary>
    /// Creates a new UriReplacementHandler.
    /// </summary>
    public UriReplacementHandler(TUriReplacement urlReplacement)
    {
        this.urlReplacement = urlReplacement;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
    {
        request.RequestUri = urlReplacement.Replace(request.RequestUri);
        return await base.SendAsync(request, cancellationToken);
    }
}
