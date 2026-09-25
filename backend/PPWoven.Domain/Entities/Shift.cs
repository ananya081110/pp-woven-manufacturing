namespace PPWoven.Domain.Entities;

public class Shift
{
    public int Id { get; set; }

    public string ShiftCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}