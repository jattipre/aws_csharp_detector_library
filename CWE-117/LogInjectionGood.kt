// {fact rule=log-injection@v1.0 defects=0}
package com.example.restservice

import org.slf4j.Logger
import org.slf4j.LoggerFactory
import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.RequestParam
import org.springframework.web.bind.annotation.RestController

@RestController
class LogInjectionGood {
    private val log: Logger = LoggerFactory.getLogger(LogInjectionGood::class.java)

    // /good?username=Guest'%0AUser:'Admin
    @GetMapping("/good")
    fun good(@RequestParam(value = "username", defaultValue = "name") username: String): String? {
        // The regex check here, allows only alphanumeric characters to pass.
        // Hence, does not result in log injection
        if (username.matches("\\w*".toRegex())) {
            log.warn("User:'{}'", username)
            return username
        } else {
            return null
        }
    }
}
// {/fact}
