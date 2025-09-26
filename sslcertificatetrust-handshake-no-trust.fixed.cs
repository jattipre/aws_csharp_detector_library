using System.Net.Security;

public class Fo
{
    private void SomeFunction(string arg1, System.Security.Cryptography.X509Certificates.X509Certificate2Collection certCollection)
    {
        //{fact rule=sensitive-information-leak@v1.0 defects=0}
        //ruleid: correctness-sslcertificatetrust-handshake-no-trust
        var collection = SslCertificateTrust.CreateForX509Collection(certCollection,false);
    }
    //{/fact}

    private void SomeFunction2(string arg1, System.Security.Cryptography.X509Certificates.X509Certificate2Collection certCollection)
    {

        //{fact rule=sensitive-information-leak@v1.0 defects=0}
        //ruleid: correctness-sslcertificatetrust-handshake-no-trust
        var collection = SslCertificateTrust.CreateForX509Collection(certCollection,false);
    }
    //{/fact}

    private void SomeFunction3(string arg1, System.Security.Cryptography.X509Certificates.X509Certificate2Collection certCollection)
    {

        //{fact rule=sensitive-information-leak@v1.0 defects=0}
        //ok: correctness-sslcertificatetrust-handshake-no-trust
        var collection = SslCertificateTrust.CreateForX509Collection(certCollection);
    }
    //{/fact}

    private void SomeFunction4(string arg1, System.Security.Cryptography.X509Certificates.X509Store certCollection)
    {

        //{fact rule=sensitive-information-leak@v1.0 defects=0}
        //ruleid: correctness-sslcertificatetrust-handshake-no-trust
        var collection = SslCertificateTrust.CreateForX509Store(certCollection,false);
    }
    //{/fact}

    private void SomeFunction5(string arg1, System.Security.Cryptography.X509Certificates.X509Store certCollection)
    {

        //{fact rule=sensitive-information-leak@v1.0 defects=0}
        //ruleid: correctness-sslcertificatetrust-handshake-no-trust
        var collection = SslCertificateTrust.CreateForX509Store(certCollection,false);
    }
    //{/fact}

    private void SomeFunction6(string arg1, System.Security.Cryptography.X509Certificates.X509Store certCollection)
    {

        //{fact rule=sensitive-information-leak@v1.0 defects=0}
        //ok: correctness-sslcertificatetrust-handshake-no-trust
        var collection = SslCertificateTrust.CreateForX509Store(certCollection);
    }
}
        //{/fact}
