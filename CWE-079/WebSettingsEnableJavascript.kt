import android.webkit.WebSettings
import android.webkit.WebView

// {fact rule=use-of-hard-coded-credentials@v1.0 defects=unknown}
class WebSettingsEnableJavascript(private val webview: WebView) {
    init {
        val settings: WebSettings = webview.settings
        settings.javaScriptEnabled = true
    }
}
// {/fact}
