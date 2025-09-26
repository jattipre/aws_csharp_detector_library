using System;
using System.Security.Cryptography;
					
public class Encryption_new
{
	public void EncryptWithAesEcb() {
		Aes key = Aes.Create();
		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_ecb_mode
		key.Mode = CipherMode.ECB; // DevSkim: ignore DS187371
		using var encryptor = key.CreateEncryptor();
		byte[] msg = new byte[32];
		var cipherText = encryptor.TransformFinalBlock(msg, 0, msg.Length);
	}
	//{/fact}
	
	public void EncryptWithAesEcb2() {
		Aes key = Aes.Create();
		byte[] msg = new byte[32];

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_ecb_mode
		var cipherText = key.EncryptEcb(msg, PaddingMode.PKCS7);
	}
	//{/fact}
	
	public void DecryptWithAesEcb(byte[] cipherText) {
		Aes key = Aes.Create();

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_ecb_mode
		key.Mode = CipherMode.ECB; // DevSkim: ignore DS187371
		using var decryptor = key.CreateDecryptor();
		var msg = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
	}
	//{/fact}
	public void DecryptWithAesEcb2(byte[] cipherText) {
		Aes key = Aes.Create();

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_ecb_mode
		var msgText = key.DecryptEcb(cipherText, PaddingMode.PKCS7);
	}
	//{/fact}
	
	public void EncryptWith3DESEcb() {
		TripleDES key = TripleDES.Create();

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_ecb_mode
		key.Mode = CipherMode.ECB; // DevSkim: ignore DS187371
		using var encryptor = key.CreateEncryptor();
		byte[] msg = new byte[32];
		var cipherText = encryptor.TransformFinalBlock(msg, 0, msg.Length);
	}
	//{/fact}
	
	public void EncryptWith3DESEcb2() {
		TripleDES key = TripleDES.Create();
		byte[] msg = new byte[32];

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_ecb_mode
		var cipherText = key.EncryptEcb(msg, PaddingMode.PKCS7);
	}
	//{/fact}
	
	public void DecryptWith3DESEcb(byte[] cipherText) {
		TripleDES key = TripleDES.Create();

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_ecb_mode
		key.Mode = CipherMode.ECB; // DevSkim: ignore DS187371
		using var decryptor = key.CreateDecryptor();
		var msg = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
	}
	//{/fact}
	
	public void DecryptWith3DESEcb2(byte[] cipherText) {
		TripleDES key = TripleDES.Create();

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_ecb_mode
		var msgText = key.DecryptEcb(cipherText, PaddingMode.PKCS7);
	}
	//{/fact}
	
	public void EncryptWithEcb(SymmetricAlgorithm key) {

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_ecb_mode
		key.Mode = CipherMode.ECB; // DevSkim: ignore DS187371
		using var encryptor = key.CreateEncryptor();
		byte[] msg = new byte[32];
		var cipherText = encryptor.TransformFinalBlock(msg, 0, msg.Length);
	}
	//{/fact}
	
	public void EncryptWithEcb2(SymmetricAlgorithm key) {
		byte[] msg = new byte[32];

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_ecb_mode
		var cipherText = key.EncryptEcb(msg, PaddingMode.PKCS7);
	}
	//{/fact}
	
	public void DecryptWithEcb(SymmetricAlgorithm key, byte[] cipherText) {

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_ecb_mode
		key.Mode = CipherMode.ECB; // DevSkim: ignore DS187371
		using var decryptor = key.CreateDecryptor();
		var msg = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
	}
	//{/fact}
	
	public void DecryptWithEcb2(SymmetricAlgorithm key, byte[] cipherText) {

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_ecb_mode
		var msgText = key.DecryptEcb(cipherText, PaddingMode.PKCS7);
	}
	//{/fact}
	
	public void EncryptWithAesCbc() {
		Aes key = Aes.Create();

		//{fact rule=insecure-cryptography@v1.0 defects=0}
		//ok: use_ecb_mode
		key.Mode = CipherMode.CBC;
		using var encryptor = key.CreateEncryptor();
		byte[] msg = new byte[32];
		var cipherText = encryptor.TransformFinalBlock(msg, 0, msg.Length);
	}
	//{/fact}
	
	public void EncryptWithAesCbc2() {
		Aes key = Aes.Create();
		byte[] msg = new byte[32];
		byte[] iv = new byte[16];

		//{fact rule=insecure-cryptography@v1.0 defects=0}
		//ok: use_ecb_mode
		var cipherText = key.EncryptCbc(msg, iv, PaddingMode.PKCS7);
	}
		//{/fact}
	
	public void DecryptWithAesCbc(byte[] cipherText) {
		Aes key = Aes.Create();

		//{fact rule=insecure-cryptography@v1.0 defects=0}
		//ok: use_ecb_mode		
		key.Mode = CipherMode.CBC;
		using var decryptor = key.CreateDecryptor();
		var msg = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
	}
	//{/fact}
	
	public void DecryptWithAesCbc2(byte[] cipherText, byte[] iv) {
		Aes key = Aes.Create();

		//{fact rule=insecure-cryptography@v1.0 defects=0}
		//ok: use_ecb_mode		
		var msgText = key.DecryptCbc(cipherText, iv, PaddingMode.PKCS7);
	}	

	//{/fact}
	public static void Main1()
	{
		Console.WriteLine("Hello World");
	}
}
