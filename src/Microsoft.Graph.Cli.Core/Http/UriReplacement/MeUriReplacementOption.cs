using System;
using Microsoft.Kiota.Http.HttpClientLibrary.Middleware.Options;

namespace Microsoft.Graph.Cli.Core.Http.UriReplacement;

/// <summary>
/// Specialized replacement for /[version]/users/me with /[version]/me
/// </summary>
public readonly struct MeUriReplacementOption : IUriReplacementHandlerOption
{
    private readonly bool isEnabled;

    /// <summary>
    /// Create new MeUriReplacement
    /// </summary>
    /// <param name="isEnabled"></param>
    public MeUriReplacementOption(bool isEnabled = true)
    {
        this.isEnabled = isEnabled;
    }

    /// <inheritdoc/>
    public readonly bool IsEnabled()
    {
        return isEnabled;
    }

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

        if (!isEnabled || original.Segments.Length < 4)
        {
            // Must have at least segments "/", "[version]/", "users/", "me"
            return original;
        }

        Span<char> toMatch = stackalloc char[] { '/', 'u', 's', 'e', 'r', 's', '/', 'm', 'e' };
        var separator = toMatch[..1];
        var matchUsers = toMatch[1..6];
        var matchMe = toMatch[7..];

        var maybeUsersSegment = original.Segments[2].AsSpan();
        if (!maybeUsersSegment[..^1].SequenceEqual(matchUsers))
        {
            return original;
        }

        var maybeMeSegment = original.Segments[3].AsSpan();
        if (!maybeMeSegment[..(maybeMeSegment.EndsWith(separator) ? ^1 : ^0)].SequenceEqual(matchMe))
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
