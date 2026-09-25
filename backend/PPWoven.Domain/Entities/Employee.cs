namespace PPWoven.Domain.Entities;

public class Employee
{
    public int Id { get; set; }

    public string EmployeeCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Department { get; set; }

    public string? Designation { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}