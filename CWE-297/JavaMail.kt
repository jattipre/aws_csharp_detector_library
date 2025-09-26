//import org.apache.velocity.texen.util.PropertiesUtil
//import java.net.Authenticator
//import java.util.*
//
//// {ex-fact rule=unverified-hostname@v1.0 defects=0}
//internal object JavaMail {
//    @JvmStatic
//    fun main(args: Array<String>) {
//        // BAD: Don't have server certificate check
//        run {
//            val properties: Properties = PropertiesUtil.getSystemProperties()
//            properties.put("mail.transport.protocol", "protocol")
//            properties.put("mail.smtp.host", "hostname")
//            properties.put("mail.smtp.socketFactory.class", "classname")
//            val authenticator: Authenticator = buildAuthenticator("username", "password")
//            if (null != authenticator) {
//                properties.put("mail.smtp.auth", "true")
//            }
//            val session: Session = Session.getInstance(properties, authenticator)
//        }
//
//        // GOOD: Have server certificate check
//        run {
//            val properties: Properties = PropertiesUtil.getSystemProperties()
//            properties.put("mail.transport.protocol", "protocol")
//            properties.put("mail.smtp.host", "hostname")
//            properties.put("mail.smtp.socketFactory.class", "classname")
//            val authenticator: Authenticator = buildAuthenticator("username", "password")
//            if (null != authenticator) {
//                properties.put("mail.smtp.auth", "true")
//                properties.put("mail.smtp.ssl.checkserveridentity", "true")
//            }
//            val session: Session = Session.getInstance(properties, authenticator)
//        }
//    }
//
//    private fun buildAuthenticator(s: String, s1: String): Authenticator {
//        TODO("Not yet implemented")
//    }
//}
// {/ex-fact}
