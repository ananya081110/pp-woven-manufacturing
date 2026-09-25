namespace PPWoven.Domain.Entities;

public class Item
{
    public int Id { get; set; }

    public string ItemCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string ItemType { get; set; } = string.Empty;

    public string UOM { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}