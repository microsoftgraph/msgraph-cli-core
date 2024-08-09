using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Microsoft.Graph.Cli.Core.utils;

[SupportedOSPlatform("Windows")]
internal partial class WindowUtils
{
    enum GetAncestorFlags
    {
        GetParent = 1,
        GetRoot = 2,
        /// <summary>
        /// Retrieves the owned root window by walking the chain of parent and owner windows returned by GetParent.
        /// </summary>
        GetRootOwner = 3
    }

    /// <summary>
    /// Retrieves the handle to the ancestor of the specified window.
    /// </summary>
    /// <param name="hwnd">A handle to the window whose ancestor is to be retrieved.
    /// If this parameter is the desktop window, the function returns NULL. </param>
    /// <param name="flags">The ancestor to be retrieved.</param>
    /// <returns>The return value is the handle to the ancestor window.</returns>
    [LibraryImport("user32.dll")]
    [SupportedOSPlatform("Windows")]
    private static partial IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags flags);

    [LibraryImport("kernel32.dll")]
    [SupportedOSPlatform("Windows")]
    private static partial IntPtr GetConsoleWindow();

    // https://learn.microsoft.com/en-us/entra/msal/dotnet/acquiring-tokens/desktop-mobile/wam#parent-window-handles
    internal static IntPtr GetConsoleOrTerminalWindow()
    {
        var consoleHandle = GetConsoleWindow();
        var handle = GetAncestor(consoleHandle, GetAncestorFlags.GetRootOwner );
        return handle;
    }
}
