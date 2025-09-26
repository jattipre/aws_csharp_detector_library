//// {ex-fact rule=sensitive-information-leak@v1.0 defects=unknown}
//internal class ContentAccessDisabled {
//    fun example1() {
//        val settings: WebSettings = webview.getSettings()
//        settings.setAllowContentAccess(true)
//    }
//}
// {/ex-fact}
