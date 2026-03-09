namespace Asisya.Entity;

public class Product
{
    public int ProductId { get; set; } // PK
    public string ProductName { get; set; } = string.Empty;
    public int? SupplierId { get; set; } // FK
    public int CategoryId { get; set; } // FK
    public string? QuantityPerUnit { get; set; }
    public decimal UnitPrice { get; set; }
    public short UnitsInStock { get; set; }
    public short UnitsOnOrder { get; set; }
    public short ReorderLevel { get; set; }
    public bool Discontinued { get; set; }
    
    public virtual Category? Category { get; set; }
    public virtual Supplier? Supplier { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}