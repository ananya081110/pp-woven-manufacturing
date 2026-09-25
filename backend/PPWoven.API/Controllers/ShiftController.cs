using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPWoven.Domain.Entities;
using PPWoven.Infrastructure.Data;

namespace PPWoven.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShiftsController : ControllerBase
{
    private readonly PPWovenDbContext _context;

    public ShiftsController(PPWovenDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Shift>>> GetShifts()
    {
        return await _context.Shifts.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Shift>> GetShift(int id)
    {
        var shift = await _context.Shifts.FindAsync(id);

        if (shift == null)
            return NotFound();

        return shift;
    }

    [HttpPost]
    public async Task<ActionResult<Shift>> CreateShift(Shift shift)
    {
        if (string.IsNullOrWhiteSpace(shift.ShiftCode))
            return BadRequest("ShiftCode is required.");

        if (string.IsNullOrWhiteSpace(shift.Name))
            return BadRequest("Name is required.");

        var exists = await _context.Shifts
            .AnyAsync(s => s.ShiftCode == shift.ShiftCode);

        if (exists)
            return BadRequest("ShiftCode already exists.");

        _context.Shifts.Add(shift);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetShift),
            new { id = shift.Id },
            shift
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShift(
        int id,
        Shift shift)
    {
        if (id != shift.Id)
            return BadRequest();

        var existingShift = await _context.Shifts.FindAsync(id);

        if (existingShift == null)
            return NotFound();

        existingShift.ShiftCode = shift.ShiftCode;
        existingShift.Name = shift.Name;
        existingShift.StartTime = shift.StartTime;
        existingShift.EndTime = shift.EndTime;
        existingShift.IsActive = shift.IsActive;
        existingShift.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShift(int id)
    {
        var shift = await _context.Shifts.FindAsync(id);

        if (shift == null)
            return NotFound();

        _context.Shifts.Remove(shift);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}