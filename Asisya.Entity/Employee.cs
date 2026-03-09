namespace Asisya.Entity;

public class Employee
{
    public int EmployeeId { get; set; } // PK
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty; 

    public string? Title { get; set; }
    public string? TitleOfCourtesy { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? HireDate { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? HomePhone { get; set; }
    public string? Extension { get; set; }
    public byte[]? Photo { get; set; }
    public string? Notes { get; set; }
    public int? ReportsTo { get; set; } // FK Recursiva

    public virtual Employee? Manager { get; set; }
    public virtual ICollection<Employee> Subordinates { get; set; } = new List<Employee>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}