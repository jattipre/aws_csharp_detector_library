//// {ex-fact rule=placeholder defects=0}
//import org.apache.commons.mail.DefaultAuthenticator
//
//internal object SimpleMail {
//    @Throws(EmailException::class)
//    @JvmStatic
//    fun main(args: Array<String>) {
//        // BAD: Don't have setSSLCheckServerIdentity set or set as false
//        run {
//            val email: Email = SimpleEmail()
//            email.setHostName("hostName")
//            email.setSmtpPort(25)
//            email.setAuthenticator(DefaultAuthenticator("username", "password"))
//            email.setSSLOnConnect(true)
//
//            //email.setSSLCheckServerIdentity(false);
//            email.setFrom("fromAddress")
//            email.setSubject("subject")
//            email.setMsg("body")
//            email.addTo("toAddress")
//            email.send()
//        }
//
//        // GOOD: Have setSSLCheckServerIdentity set to true
//        run {
//            val email: Email = SimpleEmail()
//            email.setHostName("hostName")
//            email.setSmtpPort(25)
//            email.setAuthenticator(DefaultAuthenticator("username", "password"))
//            email.setSSLOnConnect(true)
//            email.setSSLCheckServerIdentity(true)
//            email.setFrom("fromAddress")
//            email.setSubject("subject")
//            email.setMsg("body")
//            email.addTo("toAddress")
//            email.send()
//        }
//    }
//}
// {/ex-fact}
