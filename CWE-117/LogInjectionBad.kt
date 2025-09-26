// {fact rule=log-injection@v1.0 defects=1}
package com.example.restservice

import org.slf4j.Logger
import org.slf4j.LoggerFactory
import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.RequestParam
import org.springframework.web.bind.annotation.RestController

@RestController
class LogInjectionBad {
    private val log: Logger = LoggerFactory.getLogger(LogInjection::class.java)

    // /bad?username=Guest'%0AUser:'Admin
    @GetMapping("/bad")
    fun bad(@RequestParam(value = "username", defaultValue = "name") username: String): String {
        log.warn("User:'{}'", username)
        // The logging call above would result in multiple log entries as shown below:
        // User:'Guest'
        // User:'Admin'
        return username
    }
}

class LogInjection {

}
// {/fact}
