using System;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.Utils;

namespace Microsoft.Graph.Cli.Core.Authentication;

public static class ClientCertificateCredentialFactory
{
    /// <summary>
    /// Creates a ClientCertificateCredential
    /// </summary>
    /// <param name="tenantId">TenantId</param>
    /// <param name="clientId">ClientId</param>
    /// <param name="certificateName">Subject name of the certificate.</param>
    /// <param name="certificateThumbPrint">Thumb print of the certificate.</param>
    /// <returns>A ClientCertificateCredential</returns>
    public static ClientCertificateCredential GetClientCertificateCredential(string? tenantId, string? clientId, string? certificateName, string? certificateThumbPrint)
    {
        if (string.IsNullOrWhiteSpace(certificateName) && string.IsNullOrWhiteSpace(certificateThumbPrint))
        {
            throw new ArgumentException("Either a certificate name or a certificate thumb print must be provided.");
        }

        ClientCertificateCredentialOptions credOptions = new();

        // // TODO: Enable token caching
        // // Fix error:
        // //      MsalCacheHelperSingleton Error: 0 : [MSAL.Extension][2022-08-05T12:28:16.5646582Z] An exception was encountered while deserializing the MsalCacheHelper : System.NotSupportedException: Specified method is not supported.
        // //         at Microsoft.Identity.Client.PlatformsCommon.Shared.InMemoryPartitionedAppTokenCacheAccessor.SaveRefreshToken(MsalRefreshTokenCacheItem item)
        // //         at Microsoft.Identity.Client.Cache.TokenCacheJsonSerializer.Deserialize(Byte[] bytes, Boolean clearExistingCacheData)
        // //         at Microsoft.Identity.Client.TokenCache.Microsoft.Identity.Client.ITokenCacheSerializer.DeserializeMsalV3(Byte[] msalV3State, Boolean shouldClearExistingCache)
        // //         at Microsoft.Identity.Client.Extensions.Msal.MsalCacheHelper.BeforeAccessNotification(TokenCacheNotificationArgs args)
        // //      MsalCacheHelperSingleton Error: 0 : [MSAL.Extension][2022 - 08 - 05T12: 28:16.5681763Z] No data found in the store, clearing the cache in memory.
        // TokenCachePersistenceOptions tokenCacheOptions = new() { Name = Constants.TokenCacheName };
        // credOptions.TokenCachePersistenceOptions = tokenCacheOptions;

        X509Certificate2? certificate;

        if (!string.IsNullOrWhiteSpace(certificateName) && TryGetCertificateFromStore(certificateName, isThumbPrint: false, out certificate))
        {
            return new ClientCertificateCredential(tenantId, clientId, certificate, credOptions);
        }
        else if (!string.IsNullOrWhiteSpace(certificateThumbPrint) && TryGetCertificateFromStore(certificateThumbPrint, isThumbPrint: true, out certificate))
        {
            return new ClientCertificateCredential(tenantId, clientId, certificate, credOptions);
        }

        throw new ArgumentException("Could not find valid certificate.");
    }

    /// <summary>
    /// Gets unexpired certificate from the current user in My store by a subject name or thumb print.
    /// </summary>
    /// <param name="certificateNameOrThumbPrint">Subject name or thumb print of the certificate to get.</param>
    /// <param name="isThumbPrint">If true, try to find the certificate by the thumb print.</param>
    /// <returns>A matching unexpired certificate.</returns>
    private static bool TryGetCertificateFromStore(string certNameOrThumbPrint, bool isThumbPrint, out X509Certificate2? certificate)
    {
        certificate = null;
        // Get the certificate store for the current user.
        X509Store store = new X509Store(StoreLocation.CurrentUser);
        try
        {
            store.Open(OpenFlags.ReadOnly);

            // If using a certificate with a trusted root you do not need to FindByTimeValid, instead:
            // currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, true);
            X509Certificate2Collection signingCerts = store.Certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, false)
                .Find(isThumbPrint ? X509FindType.FindByThumbprint : X509FindType.FindBySubjectDistinguishedName, certNameOrThumbPrint, false);
            if (signingCerts.Count == 0)
            {
                return false;
            }
            // Return the first certificate in the collection, has the right name and is current.
            certificate = signingCerts.OrderByDescending(c => c.NotBefore).FirstOrDefault();
            return true;
        }
        catch (CryptographicException)
        {
            // The store cannot be opened as requested.
            // findType (X509Certificate2Collection.Find) is invalid.
            return false;
        }
        catch (SecurityException)
        {
            // Isufficient permissions to read the store
            return false;
        }
        catch (ArgumentException)
        {
            // Store contains invalid values.
            return false;
        }
        finally
        {
            store.Close();
        }
    }
}
