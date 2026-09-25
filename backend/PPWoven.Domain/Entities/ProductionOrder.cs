namespace PPWoven.Domain.Entities;

public class ProductionOrder
{
    public int Id { get; set; }

    public string ProductionOrderNumber { get; set; } = string.Empty;

    public int? SalesOrderId { get; set; }

    public int ItemId { get; set; }

    public decimal PlannedQuantity { get; set; }

    public string UOM { get; set; } = string.Empty;

    public DateTime PlannedStartDate { get; set; }

    public DateTime? PlannedEndDate { get; set; }

    public string Priority { get; set; } = "Normal";

    public string Status { get; set; } = "Planned";

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}