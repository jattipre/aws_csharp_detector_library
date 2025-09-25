using System.Security.Cryptography;
class D{
public void method_D()
{
var b = new AesCryptoServiceProvider()
{
    //{fact rule=sensitive-information-leak@v1.0 defects=1}
    // BAD: explicit key assignment, hard-coded value
    Key = new byte[] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 }
    //{/fact}
};
}
}