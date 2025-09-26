// {fact rule=insufficiently-protected-credentials@v1.0 defects=0}
import java.io.BufferedReader
import java.io.FileReader
import java.io.IOException
import java.io.InputStreamReader
import java.net.Socket

class TaintedPathGood {
    @Throws(IOException::class)
    fun sendUserFileGood(sock: Socket, user: String) {
        val filenameReader = BufferedReader(
                InputStreamReader(sock.getInputStream(), "UTF-8"))
        val filename = filenameReader.readLine()
        // GOOD: ensure that the file is in a designated folder in the user's home directory
        if (!filename.contains("..") && filename.startsWith("/home/$user/public/")) {
            val fileReader = BufferedReader(FileReader(filename))
            var fileLine = fileReader.readLine()
            while (fileLine != null) {
                sock.getOutputStream().write(fileLine.toByteArray())
                fileLine = fileReader.readLine()
            }
        }
    }
}
// {/fact}
