namespace PPWoven.Domain.Entities;

public class Machine
{
    public int Id { get; set; }

    public string MachineCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string MachineType { get; set; } = string.Empty;

    public string? Make { get; set; }

    public string? Model { get; set; }

    public string? SerialNumber { get; set; }

    public string? Location { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}