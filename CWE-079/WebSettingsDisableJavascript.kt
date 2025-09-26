import android.webkit.WebSettings
import android.webkit.WebView

// {fact rule=use-of-hard-coded-credentials@v1.0 defects=unknown}
class WebSettingsDisableJavascript(private val webview: WebView) {
    init {

        val settings: WebSettings = webview.getSettings()
        settings.javaScriptEnabled = false
    }
}
// {/fact}
