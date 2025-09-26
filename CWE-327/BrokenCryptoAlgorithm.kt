//// {fact rule=use-of-a-broken-or-risky-cryptographic-algorithm@v1.0 defects=0}
//import javax.crypto.Cipher
//
//class BrokenCryptoAlgorithm {
//    init {
//
//        // BAD: DES is a weak algorithm
//        val des = Cipher.getInstance("DES")
//        cipher.init(Cipher.ENCRYPT_MODE, secretKeySpec)
//        val encrypted: ByteArray = cipher.doFinal(input.getBytes("UTF-8"))
//
//        // ...
//
//        // GOOD: AES is a strong algorithm
//        val aes = Cipher.getInstance("AES")
//    } // ...
//}
//// {/fact}
