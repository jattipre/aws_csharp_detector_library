import jakarta.servlet.ServletException
import jakarta.servlet.http.HttpServlet
import jakarta.servlet.http.HttpServletRequest
import jakarta.servlet.http.HttpServletResponse
import java.io.IOException

// {fact rule=use-of-hard-coded-credentials@v1.0 defects=1}
class XSS : HttpServlet() {
    @Throws(ServletException::class, IOException::class)
    protected override fun doGet(request: HttpServletRequest, response: HttpServletResponse) {
        // BAD: a request parameter is written directly to the Servlet response stream
        response.writer.print(
                "The page \"" + request.getParameter("page") + "\" was not found.")
    }
}
// {/fact}
