using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Graph.Cli.Core.Authentication;
using Xunit;

namespace Microsoft.Graph.Cli.Core.Tests.Authentication;

public class ClientCertificateCredentialFactoryTests
{
    [Fact]
    public void ReturnsNullWhenStoreIsEmpty()
    {
        var store = new X509Certificate2Collection();

        var result = ClientCertificateCredentialFactory.FindLatestByValidity(store);

        Assert.Null(result);
    }

    [Fact]
    public void ReturnsLatestValidCertificate()
    {
        var store = new X509Certificate2Collection
        {
            GenerateSelfSignedCertificate("1", new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.Zero),
                new DateTimeOffset(2027, 1, 1, 0, 0, 0, TimeSpan.Zero)),
            GenerateSelfSignedCertificate("2", new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero),
                new DateTimeOffset(2027, 1, 1, 0, 0, 0, TimeSpan.Zero))
        };

        var result = ClientCertificateCredentialFactory.FindLatestByValidity(store);

        Assert.NotNull(result);
        Assert.Equal("CN=1", result.SubjectName.Name);
    }

    private static X509Certificate2 GenerateSelfSignedCertificate(string subjectName, DateTimeOffset notBefore,
        DateTimeOffset notAfter)
    {
        if (notAfter < notBefore)
        {
            throw new ArgumentException("notAfter must be after notBefore");
        }

        const string secp256R1Oid = "1.2.840.10045.3.1.7";
        var ecdsa = ECDsa.Create(ECCurve.CreateFromValue(secp256R1Oid));
        var certRequest = new CertificateRequest($"CN={subjectName}", ecdsa, HashAlgorithmName.SHA256);
        var generatedCert = certRequest.CreateSelfSigned(notBefore, notAfter);
        return generatedCert;
    }
}
