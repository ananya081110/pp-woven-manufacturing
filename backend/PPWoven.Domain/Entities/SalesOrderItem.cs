namespace PPWoven.Domain.Entities;

public class SalesOrderItem
{
    public int Id { get; set; }

    public int SalesOrderId { get; set; }

    public int ItemId { get; set; }

    public decimal Quantity { get; set; }

    public string UOM { get; set; } = string.Empty;

    public decimal Rate { get; set; }

    public decimal TotalValue { get; set; }

    public DateTime? RequiredDeliveryDate { get; set; }

    public string? Notes { get; set; }
}