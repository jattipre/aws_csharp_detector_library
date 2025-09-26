//// {ex-fact rule=sensitive-information-leak@v1.0 defects=1}
//import java.io.File
//import java.nio.file.Files
//
//class TempDirUsageVulnerable {
//    fun exampleVulnerable() {
//        val temp1 = File.createTempFile("random", ".txt") // BAD: File has permissions `-rw-r--r--`
//        val temp2 = File.createTempFile("random", "file", null) // BAD: File has permissions `-rw-r--r--`
//        val systemTempDir = File(System.getProperty("java.io.tmpdir"))
//        val temp3 = File.createTempFile("random", "file", systemTempDir) // BAD: File has permissions `-rw-r--r--`
//        val tempDir: File = com.google.common.io.Files.createTempDir() // BAD: CVE-2020-8908: Directory has permissions `drwxr-xr-x`
//        File(System.getProperty("java.io.tmpdir"), "/child").mkdir() // BAD: Directory has permissions `-rw-r--r--`
//        val tempDirChildFile = File(System.getProperty("java.io.tmpdir"), "/child-create-file.txt")
//        Files.createFile(tempDirChildFile.toPath()) // BAD: File has permissions `-rw-r--r--`
//        val tempDirChildDir = File(System.getProperty("java.io.tmpdir"), "/child-dir")
//        tempDirChildDir.mkdir() // BAD: Directory has permissions `drwxr-xr-x`
//        Files.createDirectory(tempDirChildDir.toPath()) // BAD: Directory has permissions `drwxr-xr-x`
//    }
//}
//// {/ex-fact}
