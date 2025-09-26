//// {ex-fact rule=sensitive-information-leak@v1.0 defects=unknown}
//internal class AssetLoaderExample {
//    fun example1() {
//        val loader: WebViewAssetLoader = Builder() // Replace the domain with a domain you control, or use the default
//                // appassets.androidplatform.com
//                .setDomain("appassets.example.com")
//                .addPathHandler("/resources", AssetsPathHandler(this))
//                .build()
//        webView.setWebViewClient(object : WebViewClientCompat() {
//            fun shouldInterceptRequest(view: WebView?, request: WebResourceRequest): WebResourceResponse {
//                return assetLoader.shouldInterceptRequest(request.getUrl())
//            }
//        })
//        webView.loadUrl("https://appassets.example.com/resources/www/index.html")
//    }
//}
// {/ex-fact}
