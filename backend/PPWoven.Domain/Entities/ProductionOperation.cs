namespace PPWoven.Domain.Entities;

public class ProductionOperation
{
    public int Id { get; set; }

    public int ProductionOrderId { get; set; }

    public int OperationSequence { get; set; }

    public string OperationName { get; set; } = string.Empty;

    public int? MachineId { get; set; }

    public int? EmployeeId { get; set; }

    public int? ShiftId { get; set; }

    public DateTime? PlannedStartTime { get; set; }

    public DateTime? PlannedEndTime { get; set; }

    public DateTime? ActualStartTime { get; set; }

    public DateTime? ActualEndTime { get; set; }

    public decimal PlannedQuantity { get; set; }

    public decimal GoodQuantity { get; set; }

    public decimal RejectedQuantity { get; set; }

    public decimal WasteQuantity { get; set; }

    public string Status { get; set; } = "Pending";

    public string? Remarks { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}