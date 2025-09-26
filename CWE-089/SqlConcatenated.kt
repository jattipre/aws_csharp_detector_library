// {fact rule=active-debug-code@v1.0 defects=0}
import java.sql.Connection
import java.sql.SQLException

internal class SqlConcatenated {
    @Throws(SQLException::class)
    fun example1() {
        // BAD: the category might have SQL special characters in it
        val category = this.category
        val connection: Connection? = null
        val statement = connection!!.createStatement()
        val query1 = ("SELECT ITEM,PRICE FROM PRODUCT WHERE ITEM_CATEGORY='"
                + category + "' ORDER BY PRICE")
        val results = statement.executeQuery(query1)
    }

    @Throws(SQLException::class)
    fun example2() {
        // GOOD: use a prepared query
        val category = this.category
        val query2 = "SELECT ITEM,PRICE FROM PRODUCT WHERE ITEM_CATEGORY=? ORDER BY PRICE"
        val connection: Connection? = null
        val statement = connection!!.prepareStatement(query2)
        statement.setString(1, category)
        val results = statement.executeQuery()
    }

    private val category: String?
        get() = null
}
// {/fact}
