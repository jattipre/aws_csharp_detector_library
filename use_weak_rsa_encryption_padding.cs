using System;
using System.Security.Cryptography;
					
public class RSAEncryption
{
	public static void EncryptWithBadPadding1()
	{
		RSA key = RSA.Create();
		byte[] msg = new byte[16];
		Type t = typeof(byte[]);
		RSAPKCS1KeyExchangeFormatter formatter = new RSAPKCS1KeyExchangeFormatter(key);
		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_weak_rsa_encryption_padding
		byte[] cipherText = formatter.CreateKeyExchange(msg, t);
	}
	//{/fact}
	
	public static void DecryptWithBadPadding()
	{
		RSA key = RSA.Create();
		byte[] ciphertext = new byte[16];
		var deformatter = new RSAPKCS1KeyExchangeDeformatter(key);

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_weak_rsa_encryption_padding
		var plaintext = deformatter.DecryptKeyExchange(ciphertext);
	}
	//{/fact}

	public static void EncryptWithBadPadding2()
	{
		RSA key = RSA.Create();
		byte[] msg = new byte[16];
		var formatter = new RSAPKCS1KeyExchangeFormatter(key);

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_weak_rsa_encryption_padding
		byte[] cipherText = formatter.CreateKeyExchange(msg);
	}
	//{/fact}

	public static void EncryptWithGoodPadding1()
	{
		RSA key = RSA.Create();
		byte[] msg = new byte[16];
		Type t = typeof(byte[]);
		AsymmetricKeyExchangeFormatter formatter = new RSAOAEPKeyExchangeFormatter(key);

		//{fact rule=insecure-cryptography@v1.0 defects=0}
		//ok: use_weak_rsa_encryption_padding
		byte[] cipherText = formatter.CreateKeyExchange(msg, t);
	}
	//{/fact}
	
	public static void EncryptWithGoodPadding2()
	{
		RSA key = RSA.Create();
		byte[] msg = new byte[16];
		AsymmetricKeyExchangeFormatter formatter = new RSAOAEPKeyExchangeFormatter(key);

		//{fact rule=insecure-cryptography@v1.0 defects=0}
		//ok: use_weak_rsa_encryption_padding
		byte[] cipherText = formatter.CreateKeyExchange(msg);
	}
	//{/fact}

	public static void DecryptWithGoodPadding()
	{
		RSA key = RSA.Create();
		byte[] ciphertext = new byte[16];
		var deformatter = new RSAOAEPKeyExchangeDeformatter(key);

		//{fact rule=insecure-cryptography@v1.0 defects=0}
		//ok: use_weak_rsa_encryption_padding
		var plaintext = deformatter.DecryptKeyExchange(ciphertext);
	}
	//{/fact}

	
	public static void Main1(string[] args){
	}
}

