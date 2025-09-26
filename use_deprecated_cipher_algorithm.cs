using System;
using System.Security.Cryptography;
					
public class Encryption
{
	public void CreateAes1() {
		//{fact rule=insecure-cryptography@v1.0 defects=0}
		//ok: use_deprecated_cipher_algorithm
		var key = Aes.Create();
	}
	//{/fact}
	
	public void CreateAes2() {

		//{fact rule=insecure-cryptography@v1.0 defects=0}
		//ok: use_deprecated_cipher_algorithm
		var key = Aes.Create("ImplementationName");
	}
	//{/fact}

	public void CreateRijndael1() {

		//{fact rule=insecure-cryptography@v1.0 defects=0}
		//ok: use_deprecated_cipher_algorithm
		var key = Rijndael.Create();
	}
	//{/fact}
	
	public void CreateRijndael2() {

		//{fact rule=insecure-cryptography@v1.0 defects=0}
		//ok: use_deprecated_cipher_algorithm
		var key = Rijndael.Create("ImplementationName");
	}
	//{/fact}

	public void CreateDES1() {

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_deprecated_cipher_algorithm
		var key = DES.Create(); // DevSkim: ignore DS106863
	}
	//{/fact}
	
	public void CreateDES2() {

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_deprecated_cipher_algorithm
		var key = DES.Create("ImplementationName"); // DevSkim: ignore DS106863 until 2023-09-17
	}
	//{/fact}

	public void CreateTripleDES1() {

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ok: use_deprecated_cipher_algorithm
		var key = TripleDES.Create();
	}
	//{/fact}
	
	public void CreateTripleDES2() {

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ok: use_deprecated_cipher_algorithm
		var key = TripleDES.Create("ImplementationName");
	}
	//{/fact}

	public void CreateRC21() {

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_deprecated_cipher_algorithm
		var key = RC2.Create();
	}
	//{/fact}
	
	public void CreateRC22() {

		//{fact rule=insecure-cryptography@v1.0 defects=1}
		//ruleid: use_deprecated_cipher_algorithm
		var key = RC2.Create("ImplementationName");
	}
}		//{/fact}
