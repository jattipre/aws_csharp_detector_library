//import jakarta.servlet.http.HttpServletRequest
//import jakarta.servlet.http.HttpServletResponse
//
//// {ex-fact rule=sensitive-information-leak@v1.0 defects=0}
//fun doGet(request: HttpServletRequest?, response: HttpServletResponse) {
//    try {
//        doSomeWork()
//    } catch (ex: NullPointerException) {
//        // BAD: printing a stack trace back to the response
//        ex.printStackTrace(response.getWriter())
//        return
//    }
//    try {
//        doSomeWork()
//    } catch (ex: NullPointerException) {
//        // GOOD: log the stack trace, and send back a non-revealing response
//        log("Exception occurred", ex)
//        response.sendError(
//                HttpServletResponse.SC_INTERNAL_SERVER_ERROR,
//                "Exception occurred")
//        return
//    }
//}
//
//fun doSomeWork() {
//    TODO("Not yet implemented")
//}
//// {/ex-fact}
