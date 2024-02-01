using System;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Cli.Core.Utils;


/// <summary>
/// The cloud environment to use
/// </summary>
public enum CloudEnvironment
{
    /// <summary>
    /// Global environment.
    /// </summary>
    Global,
    /// <summary>
    /// US Government cloud environment.
    /// </summary>
    USGov,
    /// <summary>
    /// US Government Department of Defense (DoD) cloud environment.
    /// </summary>
    USGovDoD,
    /// <summary>
    /// China cloud environment.
    /// </summary>
    China,
}

/// <summary>
/// Provides methods for the <see cref="CloudEnvironment"/> class.
/// </summary>
public static class CloudEnvironmentExtensions
{
    /// <summary>
    /// Gets the authority URL for the specified cloud environment.
    /// </summary>
    /// <param name="environment">The cloud environment.</param>
    /// <returns>The authority URL.</returns>
    /// <exception cref="ArgumentException">
    /// If the cloud environment is not one of the <see cref="CloudEnvironment"/> members.
    /// </exception>
    public static Uri Authority(this CloudEnvironment environment)
    {
        return environment switch
        {
            CloudEnvironment.Global => AzureAuthorityHosts.AzurePublicCloud,
            CloudEnvironment.USGov or CloudEnvironment.USGovDoD => AzureAuthorityHosts.AzureGovernment,
            CloudEnvironment.China => AzureAuthorityHosts.AzureChina,
            _ => throw new ArgumentException("Unknown cloud environment", nameof(environment))
        };
    }

    /// <summary>
    /// Gets the GraphClient Cloud identifier.
    /// </summary>
    /// <param name="environment">The cloud environment.</param>
    /// <returns>The cloud identifier to be used by the graph client.</returns>
    /// <exception cref="ArgumentException">
    /// If the cloud environment is not one of the <see cref="CloudEnvironment"/> members.
    /// </exception>
    public static string GraphClientCloud(this CloudEnvironment environment)
    {
        return environment switch
        {
            CloudEnvironment.Global => GraphClientFactory.Global_Cloud,
            CloudEnvironment.USGov => GraphClientFactory.USGOV_Cloud,
            CloudEnvironment.USGovDoD => GraphClientFactory.USGOV_DOD_Cloud,
            CloudEnvironment.China => GraphClientFactory.China_Cloud,
            _ => throw new ArgumentException("Unknown cloud environment", nameof(environment))
        };
    }
}
