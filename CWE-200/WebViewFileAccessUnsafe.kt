//// {ex-fact rule=sensitive-information-leak@v1.0 defects=unknown}
//internal class WebViewFileAccessSafe {
//    fun example1() {
//        val settings: WebSettings = view.getSettings()
//        settings.setAllowFileAccess(true)
//        settings.setAllowFileAccessFromURLs(true)
//        settings.setAllowUniversalAccessFromURLs(true)
//    }
//}
// {/ex-fact}
