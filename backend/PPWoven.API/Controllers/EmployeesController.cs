using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPWoven.Domain.Entities;
using PPWoven.Infrastructure.Data;

namespace PPWoven.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly PPWovenDbContext _context;

    public EmployeesController(PPWovenDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
    {
        return await _context.Employees.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
            return NotFound();

        return employee;
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.EmployeeCode))
            return BadRequest("EmployeeCode is required.");

        if (string.IsNullOrWhiteSpace(employee.Name))
            return BadRequest("Name is required.");

        var exists = await _context.Employees
            .AnyAsync(e => e.EmployeeCode == employee.EmployeeCode);

        if (exists)
            return BadRequest("EmployeeCode already exists.");

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEmployee),
            new { id = employee.Id },
            employee
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(
        int id,
        Employee employee)
    {
        if (id != employee.Id)
            return BadRequest();

        var existingEmployee = await _context.Employees.FindAsync(id);

        if (existingEmployee == null)
            return NotFound();

        existingEmployee.EmployeeCode = employee.EmployeeCode;
        existingEmployee.Name = employee.Name;
        existingEmployee.Department = employee.Department;
        existingEmployee.Designation = employee.Designation;
        existingEmployee.Phone = employee.Phone;
        existingEmployee.Email = employee.Email;
        existingEmployee.IsActive = employee.IsActive;
        existingEmployee.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
            return NotFound();

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}