// {fact rule=active-debug-code@v1.0 defects=0}
import java.sql.Connection
import java.sql.PreparedStatement
import java.sql.ResultSet
import java.sql.Statement

internal class SqlTainted {
    init {
        // BAD: the category might have SQL special characters in it
        val category = System.getenv("ITEM_CATEGORY")
        val connection: Connection? = null
        val statement: Statement = connection!!.createStatement()
        val query1 = ("SELECT ITEM,PRICE FROM PRODUCT WHERE ITEM_CATEGORY='"
                + category + "' ORDER BY PRICE")
        val results: ResultSet = statement.executeQuery(query1)
    }

    init {
        // GOOD: use a prepared query
        val category = System.getenv("ITEM_CATEGORY")
        val query2 = "SELECT ITEM,PRICE FROM PRODUCT WHERE ITEM_CATEGORY=? ORDER BY PRICE"
        val connection: Connection? = null
        val statement: PreparedStatement = connection!!.prepareStatement(query2)
        statement.setString(1, category)
        val results: ResultSet = statement.executeQuery()
    }
}
// {/fact}
