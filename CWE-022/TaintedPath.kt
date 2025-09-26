// {fact rule=insufficiently-protected-credentials@v1.0 defects=1}
import java.io.BufferedReader
import java.io.FileReader
import java.io.IOException
import java.io.InputStreamReader
import java.net.Socket

class TaintedPath {
    @Throws(IOException::class)
    fun sendUserFile(sock: Socket, user: String?) {
        val filenameReader = BufferedReader(
                InputStreamReader(sock.getInputStream(), "UTF-8"))
        val filename = filenameReader.readLine()
        // BAD: read from a file without checking its path
        val fileReader = BufferedReader(FileReader(filename))
        var fileLine = fileReader.readLine()
        while (fileLine != null) {
            sock.getOutputStream().write(fileLine.toByteArray())
            fileLine = fileReader.readLine()
        }
    }
}
// {/fact}
