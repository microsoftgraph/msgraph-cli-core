using System;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Azure.Identity;
using Microsoft.Graph.Cli.Core.Utils;

namespace Microsoft.Graph.Cli.Core.Authentication;

/// <summary>
/// Client certificate credential factory.
/// </summary>
public static class ClientCertificateCredentialFactory
{
    /// <summary>
    /// Creates a ClientCertificateCredential
    /// </summary>
    /// <param name="tenantId">TenantId</param>
    /// <param name="clientId">ClientId</param>
    /// <param name="certificateName">Subject name of the certificate.</param>
    /// <param name="certificateThumbPrint">Thumb print of the certificate.</param>
    /// <param name="authorityHost">The entra authentication endpoint (to use with national clouds)</param>
    /// <returns>A ClientCertificateCredential</returns>
    /// <exception cref="ArgumentNullException">When a null url is provided for the authority host.</exception>
    public static ClientCertificateCredential GetClientCertificateCredential(string? tenantId, string? clientId, string? certificateName, string? certificateThumbPrint, Uri authorityHost)
    {
        if (string.IsNullOrWhiteSpace(certificateName) && string.IsNullOrWhiteSpace(certificateThumbPrint))
        {
            throw new ArgumentException("Either a certificate name or a certificate thumb print must be provided.");
        }

        ClientCertificateCredentialOptions credOptions = new() { AuthorityHost = authorityHost };

        // // TODO: Enable token caching
        // // Fix error:
        // //      MsalCacheHelperSingleton Error: 0 : [MSAL.Extension][2022-08-05T12:28:16.5646582Z] An exception was encountered while deserializing the MsalCacheHelper : System.NotSupportedException: Specified method is not supported.
        // //         at Microsoft.Identity.Client.PlatformsCommon.Shared.InMemoryPartitionedAppTokenCacheAccessor.SaveRefreshToken(MsalRefreshTokenCacheItem item)
        // //         at Microsoft.Identity.Client.Cache.TokenCacheJsonSerializer.Deserialize(Byte[] bytes, Boolean clearExistingCacheData)
        // //         at Microsoft.Identity.Client.TokenCache.Microsoft.Identity.Client.ITokenCacheSerializer.DeserializeMsalV3(Byte[] msalV3State, Boolean shouldClearExistingCache)
        // //         at Microsoft.Identity.Client.Extensions.Msal.MsalCacheHelper.BeforeAccessNotification(TokenCacheNotificationArgs args)
        // //      MsalCacheHelperSingleton Error: 0 : [MSAL.Extension][2022 - 08 - 05T12: 28:16.5681763Z] No data found in the store, clearing the cache in memory.
        // TODO: Investigate how to clear confidential client cache. What accountId is used with the GetAccountAsync & RemoveAsync functions?
        // TokenCachePersistenceOptions tokenCacheOptions = new() { Name = Microsoft.Graph.Cli.Core.Utils.Constants.TokenCacheName };
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

        throw new ArgumentException("Could not find a valid certificate.");
    }

    /// <summary>
    /// Gets unexpired certificate from the current user store by a subject name or thumb print.
    /// </summary>
    /// <param name="certificateNameOrThumbPrint">Subject name or thumb print of the certificate to get.</param>
    /// <param name="isThumbPrint">If true, try to find the certificate by the thumb print.</param>
    /// <param name="certificate">A matching unexpired certificate from the store.</param>
    /// <returns>Returns true if the certificate was fetched successfully.</returns>
    internal static bool TryGetCertificateFromStore(string certificateNameOrThumbPrint, bool isThumbPrint, out X509Certificate2? certificate)
    {
        bool result = false;
        certificate = null;
        // Get the certificate store for the current user.
        X509Store store = new X509Store(StoreLocation.CurrentUser);
        try
        {
            store.Open(OpenFlags.ReadOnly);

            // If using a certificate with a trusted root you do not need to FindByTimeValid, instead:
            // currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, true);
            X509Certificate2Collection signingCerts = store.Certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, false)
                .Find(isThumbPrint ? X509FindType.FindByThumbprint : X509FindType.FindBySubjectDistinguishedName, certificateNameOrThumbPrint, false);
            if (signingCerts.Count == 0)
            {
                result = false;
            }
            else
            {
                // Return the first certificate in the collection, has the right name and is current.
                certificate = signingCerts.OrderByDescending(static c => c.NotBefore).FirstOrDefault();
                result = true;
            }
        }
        catch (CryptographicException)
        {
            // The store cannot be opened as requested.
            // findType (X509Certificate2Collection.Find) is invalid.
            result = false;
        }
        catch (SecurityException)
        {
            // Isufficient permissions to read the store
            result = false;
        }
        catch (ArgumentException)
        {
            // Store contains invalid values.
            result = false;
        }
        finally
        {
            if (!result && certificate is not null)
            {
                certificate.Dispose();
                certificate = null;
            }
            store.Close();
        }

        return result;
    }

    /// <summary>
    /// Opens a certificate file.
    /// </summary>
    /// <param name="path">The path to a certificate file.</param>
    /// <param name="password">The optional password for the certificate file.</param>
    /// <param name="certificate">The certificate. Will be null if the certificate failed to open or if the certificate has no pricate key. A certificate with no private key cannot be used to authenticate.</param>
    /// <returns>Returns true if the certificate was fetched successfully.</returns>
    internal static bool TryGetCertificateFromFile(string path, string? password, out X509Certificate2? certificate)
    {
        bool result = false;
        certificate = null;
        try
        {
            certificate = new X509Certificate2(path, password);
            result = certificate?.HasPrivateKey == true;
        }
        catch (CryptographicException)
        {
            result = false;
        }
        finally
        {
            if (!result && certificate is not null)
            {
                certificate.Dispose();
                certificate = null;
            }
        }

        return result;
    }
}
