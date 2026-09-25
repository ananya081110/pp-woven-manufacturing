namespace PPWoven.Domain.Entities;

public class SalesOrder
{
    public int Id { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public int CustomerId { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public DateTime? RequiredDeliveryDate { get; set; }

    public string Priority { get; set; } = "Normal";

    public string Status { get; set; } = "Draft";

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}