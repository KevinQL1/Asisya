namespace Asisya.Entity;

public class OrderDetail
{
    public int OrderId { get; set; } // PK & FK
    public int ProductId { get; set; } // PK & FK
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; }
    public float Discount { get; set; }

    public virtual Order? Order { get; set; }
    public virtual Product? Product { get; set; }
}