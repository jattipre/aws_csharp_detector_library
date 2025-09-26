//// {fact rule=cross-site-request-forgery@v1.0 defects=unknown}
//import org.springframework.context.annotation.Configuration
//
//@EnableWebSecurity
//@Configuration
//class WebSecurityConfig : WebSecurityConfigurerAdapter() {
//    @Throws(Exception::class)
//    protected fun configure(http: HttpSecurity) {
//        http
//                .csrf { csrf ->  // BAD - CSRF protection shouldn't be disabled
//                    csrf.disable()
//                }
//    }
//}
//// {/fact}
