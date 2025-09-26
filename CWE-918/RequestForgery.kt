//// {fact rule=server-side-request-forgery@v1.0 defects=0}
//import java.net.http.HttpClient
//
//class SSRF : HttpServlet() {
//    private val client = HttpClient.newHttpClient()
//    @Throws(ServletException::class, IOException::class)
//    protected fun doGet(request: HttpServletRequest, response: HttpServletResponse?) {
//        val uri = URI(request.getParameter("uri"))
//        // BAD: a request parameter is incorporated without validation into a Http request
//        val r: HttpRequest = HttpRequest.newBuilder(uri).build()
//        client.send(r, null)
//
//        // GOOD: the request parameter is validated against a known fixed string
//        if (VALID_URI == request.getParameter("uri")) {
//            val r2: HttpRequest = HttpRequest.newBuilder(uri).build()
//            client.send(r2, null)
//        }
//    }
//
//    companion object {
//        private const val VALID_URI = "http://lgtm.com"
//    }
//}
//// {/fact}
