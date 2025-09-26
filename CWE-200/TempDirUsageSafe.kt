// {fact rule=sensitive-information-leak@v1.0 defects=0}
import java.io.File
import java.io.IOException
import java.io.UncheckedIOException
import java.nio.file.Files
import java.nio.file.Path
import java.nio.file.attribute.PosixFilePermission
import java.nio.file.attribute.PosixFilePermissions
import java.util.*

class TempDirUsageSafe {
    @Throws(IOException::class)
    fun exampleSafe() {
        val temp1 = Files.createTempFile("random", ".txt") // GOOD: File has permissions `-rw-------`
        val temp2 = Files.createTempDirectory("random-directory") // GOOD: File has permissions `drwx------`

        // Creating a temporary file with a non-randomly generated name
        val tempChildFile = File(System.getProperty("java.io.tmpdir"), "/child-create-file.txt")
        // Warning: This will fail on windows as it doesn't support PosixFilePermissions.
        // See `exampleSafeWithWindowsSupportFile` if your code needs to support windows and unix-like systems.
        Files.createFile(
                tempChildFile.toPath(),
                PosixFilePermissions.asFileAttribute(EnumSet.of(PosixFilePermission.OWNER_READ, PosixFilePermission.OWNER_WRITE))
        ) // GOOD: Good has permissions `-rw-------`
    }

    /*
     * An example of a safe use of createFile or createDirectory if your code must support windows and unix-like systems.
     */
    fun exampleSafeWithWindowsSupportFile() {
        // Creating a temporary file with a non-randomly generated name
        val tempChildFile = File(System.getProperty("java.io.tmpdir"), "/child-create-file.txt")
        createTempFile(tempChildFile.toPath()) // GOOD: Good has permissions `-rw-------`
    }

    fun exampleSafeWithWindowsSupportDirectory() {
        val tempDirChildDir = File(System.getProperty("java.io.tmpdir"), "/child-dir")
        createTempDirectories(tempDirChildDir.toPath()) // GOOD: Directory has permissions `drwx------`
    }

    companion object {
        fun createTempFile(tempDirChild: Path) {
            try {
                if (tempDirChild.fileSystem.supportedFileAttributeViews().contains("posix")) {
                    // Explicit permissions setting is only required on unix-like systems because
                    // the temporary directory is shared between all users.
                    // This is not necessary on Windows, each user has their own temp directory
                    val posixFilePermissions = EnumSet.of(
                            PosixFilePermission.OWNER_READ,
                            PosixFilePermission.OWNER_WRITE
                    )
                    if (!Files.exists(tempDirChild)) {
                        Files.createFile(
                                tempDirChild,
                                PosixFilePermissions.asFileAttribute(posixFilePermissions)
                        ) // GOOD: Directory has permissions `-rw-------`
                    } else {
                        Files.setPosixFilePermissions(
                                tempDirChild,
                                posixFilePermissions
                        ) // GOOD: Good has permissions `-rw-------`, or will throw an exception if this fails
                    }
                } else if (!Files.exists(tempDirChild)) {
                    // On Windows, we still need to create the directory, when it doesn't already exist.
                    Files.createDirectory(tempDirChild) // GOOD: Windows doesn't share the temp directory between users
                }
            } catch (exception: IOException) {
                throw UncheckedIOException("Failed to create temp file", exception)
            }
        }

        fun createTempDirectories(tempDirChild: Path) {
            try {
                if (tempDirChild.fileSystem.supportedFileAttributeViews().contains("posix")) {
                    // Explicit permissions setting is only required on unix-like systems because
                    // the temporary directory is shared between all users.
                    // This is not necessary on Windows, each user has their own temp directory
                    val posixFilePermissions = EnumSet.of(
                            PosixFilePermission.OWNER_READ,
                            PosixFilePermission.OWNER_WRITE,
                            PosixFilePermission.OWNER_EXECUTE
                    )
                    if (!Files.exists(tempDirChild)) {
                        Files.createDirectories(
                                tempDirChild,
                                PosixFilePermissions.asFileAttribute(posixFilePermissions)
                        ) // GOOD: Directory has permissions `drwx------`
                    } else {
                        Files.setPosixFilePermissions(
                                tempDirChild,
                                posixFilePermissions
                        ) // GOOD: Good has permissions `drwx------`, or will throw an exception if this fails
                    }
                } else if (!Files.exists(tempDirChild)) {
                    // On Windows, we still need to create the directory, when it doesn't already exist.
                    Files.createDirectories(tempDirChild) // GOOD: Windows doesn't share the temp directory between users
                }
            } catch (exception: IOException) {
                throw UncheckedIOException("Failed to create temp dir", exception)
            }
        }
    }
}
// {/fact}
