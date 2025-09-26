// {fact rule=use-of-hard-coded-credentials@v1.0 defects=unknown}
import android.content.Context
import android.database.Cursor
import android.database.sqlite.SQLiteDatabase
import android.database.sqlite.SQLiteOpenHelper


internal abstract class ExposedObject(context: Context, name: String, factory: SQLiteDatabase.CursorFactory?, version: Int) : SQLiteOpenHelper(context, name, factory, version) {


    fun studentEmail(studentName: String): String {
        val db = readableDatabase
        val query = "SELECT email FROM students WHERE studentname = ?"
        val cursor: Cursor = db.rawQuery(query, arrayOf(studentName))
        return if (cursor.moveToFirst()) {
            val email = cursor.getString(cursor.getColumnIndexOrThrow("email"))
            cursor.close()
            email
        } else {
            cursor.close()
            ""
        }
    }

    override abstract fun onCreate(db: SQLiteDatabase)
    override abstract fun onUpgrade(db: SQLiteDatabase, oldVersion: Int, newVersion: Int)
}
