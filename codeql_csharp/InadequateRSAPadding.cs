using System;
using System.Security.Cryptography;

namespace InadequateRSAPadding
{
    class Main
    {

        static public byte[]? EncryptWithRSAAndNoPadding(byte[] plaintext, RSAParameters key)
        {
             //{fact rule=insecure-cryptography@v1.0 defects=1}
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(key);
                return rsa.Encrypt(plaintext, false); // BAD
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
             //{/fact}
        }

        static public byte[]? EncryptWithRSAAndPadding(byte[] plaintext, RSAParameters key)
        {
             //{fact rule=insecure-cryptography@v1.0 defects=0}
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(key);
                return rsa.Encrypt(plaintext, true); // GOOD
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
             //{/fact}
        }

    }

}