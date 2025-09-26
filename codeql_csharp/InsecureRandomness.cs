using System.Security.Cryptography;

string GeneratePassword()
{
    //{fact rule=weak-random-number-generation@v1.0 defects=1}
    // BAD: Password is generated using a cryptographically insecure RNG
    Random gen = new Random();
    string password = "mypassword" + gen.Next();
    //{/fact}

    //{fact rule=weak-random-number-generation@v1.0 defects=0}
    // GOOD: Password is generated using a cryptographically secure RNG
    using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
    {
        byte[] randomBytes = new byte[sizeof(int)];
        crypto.GetBytes(randomBytes);
        password = "mypassword" + BitConverter.ToInt32(randomBytes);
    }
    //{/fact}

    //{fact rule=weak-random-number-generation@v1.0 defects=1}
    // BAD: Membership.GeneratePassword generates a password with a bias
    password = Membership.GeneratePassword(12, 3);

    return password;
    //{/fact}
}

internal class Membership
{
    internal static string GeneratePassword(int v1, int v2)
    {
        throw new NotImplementedException();
    }
}
