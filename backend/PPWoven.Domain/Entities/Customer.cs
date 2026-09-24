namespace PPWoven.Domain.Entities;

public class Customer
{
    public int Id { get; set; }

    public string CustomerCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? TaxNumber { get; set; }

    public decimal? CreditLimit { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}