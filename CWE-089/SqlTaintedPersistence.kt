import jakarta.persistence.Entity
import jakarta.persistence.Id
import jakarta.persistence.NamedQuery

@Entity
open class Product(
    @Id
    val id: Int,
    var category: String,
    var price: Double
) {
    // No-argument constructor required by JPA
    protected constructor() : this(0, "", 0.0)
    // Stub implementation
}

// {fact rule=active-debug-code@v1.0 defects=0}
// GOOD: use a named query with a named parameter and set its value
@NamedQuery(name = "lookupByCategory", query = "SELECT p FROM Product p WHERE p.category LIKE :category ORDER BY p.price")
private class NQ {

}

// GOOD: use a named query with a positional parameter and set its value
@NamedQuery(name = "lookupByCategory", query = "SELECT p FROM Product p WHERE p.category LIKE ?1 ORDER BY p.price")
private class NA {

}
// {/fact}
