using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

class CertSelect
{
    static void Main()
    {
        X509Store store = new X509Store("MY",StoreLocation.CurrentUser);
        store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

        X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
        X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid,DateTime.Now,false);
        X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(fcollection, "Test Certificate Select","Select a certificate from the following list to get information on that certificate",X509SelectionFlag.MultiSelection);

        foreach (X509Certificate2 x509 in scollection)
        {
            try
            {
                //{fact rule=aws-kms-reencryption@v1.0 defects=1}
                // ruleid: X509Certificate2-privkey
                Console.WriteLine(x509.PrivateKey);
            }
            catch (CryptographicException)
            {
                Console.WriteLine("Information could not be written out for this certificate.");
            }
        }  //{/fact}
        store.Close();

        X509Certificate2 cert = new X509Certificate2();

        //{fact rule=aws-kms-reencryption@v1.0 defects=1}
        // ruleid: X509Certificate2-privkey
        var privkey = cert.PrivateKey;
    }
}

internal class X509SelectionFlag
{
    public static object MultiSelection { get; internal set; }
}

internal class X509Certificate2UI
{
    internal static X509Certificate2Collection SelectFromCollection(X509Certificate2Collection fcollection, string v1, string v2, object multiSelection)
    {
        throw new NotImplementedException();
    }
}
//{/fact}
