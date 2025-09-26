using System;
using System.Security.Cryptography;
					
public class ProgramNew
{	
	public void GenerateBadKey() {
		var rng = new System.Random(); // DevSkim: ignore DS148264
		byte[] key = new byte[16];
		rng.NextBytes(key);
		SymmetricAlgorithm cipher = Aes.Create();
		//{fact rule=weak-random-number-generation@v1.0 defects=1}
		// ruleid: use_weak_rng_for_keygeneration
		cipher.Key = key;
	}
	//{/fact}
	
	public void GenerateBadKeyGcm() {
		var rng = new System.Random(); // DevSkim: ignore DS148264
		byte[] key = new byte[16];
		rng.NextBytes(key);

		//{fact rule=weak-random-number-generation@v1.0 defects=1}
		// ruleid: use_weak_rng_for_keygeneration
		var cipher = new AesGcm(key);
	}
	//{/fact}
	
	public void GenerateGoodKey() {
		var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
		byte[] key = new byte[16];
		rng.GetBytes(key);
		var cipher = Aes.Create();

		//{fact rule=weak-random-number-generation@v1.0 defects=0}
		// ok: use_weak_rng_for_keygeneration
		cipher.Key = key;
	}
	//{/fact}

	public void GenerateGoodKeyGcm() {
		var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
		byte[] key = new byte[16];
		rng.GetBytes(key);

		//{fact rule=weak-random-number-generation@v1.0 defects=0}
		// ok: use_weak_rng_for_keygeneration
		var cipher = new AesGcm(key);
	}
	//{/fact}

	public void GenerateBadKeyCcm() {
		var rng = new System.Random(); // DevSkim: ignore DS148264
		byte[] key = new byte[16];
		rng.NextBytes(key);

		//{fact rule=weak-random-number-generation@v1.0 defects=1}
		// ruleid: use_weak_rng_for_keygeneration
		var cipher = new AesCcm(key);
	}
	//{/fact}

	public void GenerateGoodKeyCcm() {
		var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
		byte[] key = new byte[16];
		rng.GetBytes(key);

		//{fact rule=weak-random-number-generation@v1.0 defects=0}
		// ok: use_weak_rng_for_keygeneration
		var cipher = new AesCcm(key);
	}
	//{/fact}

	public void GenerateBadKeyChaCha20() {
		var rng = new System.Random(); // DevSkim: ignore DS148264
		byte[] key = new byte[16];
		rng.NextBytes(key);

		//{fact rule=weak-random-number-generation@v1.0 defects=1}
		// ruleid: use_weak_rng_for_keygeneration
		var cipher = new ChaCha20Poly1305(key);
	}
	//{/fact}

	public void GenerateGoodKeyChaCha20() {
		var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
		byte[] key = new byte[16];
		rng.GetBytes(key);

		//{fact rule=weak-random-number-generation@v1.0 defects=0}
		// ok: use_weak_rng_for_keygeneration
		var cipher = new ChaCha20Poly1305(key);
	}
}
		//{/fact}
