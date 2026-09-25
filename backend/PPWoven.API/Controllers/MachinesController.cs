using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPWoven.Domain.Entities;
using PPWoven.Infrastructure.Data;

namespace PPWoven.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MachinesController : ControllerBase
{
    private readonly PPWovenDbContext _context;

    public MachinesController(PPWovenDbContext context)
    {
        _context = context;
    }

    // GET: api/machines
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Machine>>> GetMachines()
    {
        var machines = await _context.Machines
            .OrderBy(m => m.Name)
            .ToListAsync();

        return Ok(machines);
    }

    // GET: api/machines/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Machine>> GetMachine(int id)
    {
        var machine = await _context.Machines.FindAsync(id);

        if (machine == null)
        {
            return NotFound(new
            {
                message = $"Machine with ID {id} was not found."
            });
        }

        return Ok(machine);
    }

    // POST: api/machines
    [HttpPost]
    public async Task<ActionResult<Machine>> CreateMachine(Machine machine)
    {
        if (string.IsNullOrWhiteSpace(machine.MachineCode))
        {
            return BadRequest(new
            {
                message = "Machine code is required."
            });
        }

        if (string.IsNullOrWhiteSpace(machine.Name))
        {
            return BadRequest(new
            {
                message = "Machine name is required."
            });
        }

        if (string.IsNullOrWhiteSpace(machine.MachineType))
        {
            return BadRequest(new
            {
                message = "Machine type is required."
            });
        }

        var existingMachine = await _context.Machines
            .FirstOrDefaultAsync(m => m.MachineCode == machine.MachineCode);

        if (existingMachine != null)
        {
            return Conflict(new
            {
                message = $"Machine code '{machine.MachineCode}' already exists."
            });
        }

        machine.Id = 0;
        machine.CreatedAt = DateTime.UtcNow;
        machine.UpdatedAt = DateTime.UtcNow;
        machine.IsActive = true;

        _context.Machines.Add(machine);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetMachine),
            new { id = machine.Id },
            machine
        );
    }

    // PUT: api/machines/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMachine(
        int id,
        Machine updatedMachine)
    {
        var machine = await _context.Machines.FindAsync(id);

        if (machine == null)
        {
            return NotFound(new
            {
                message = $"Machine with ID {id} was not found."
            });
        }

        var duplicateCode = await _context.Machines
            .AnyAsync(m =>
                m.MachineCode == updatedMachine.MachineCode &&
                m.Id != id);

        if (duplicateCode)
        {
            return Conflict(new
            {
                message = $"Machine code '{updatedMachine.MachineCode}' already exists."
            });
        }

        machine.MachineCode = updatedMachine.MachineCode;
        machine.Name = updatedMachine.Name;
        machine.MachineType = updatedMachine.MachineType;
        machine.Make = updatedMachine.Make;
        machine.Model = updatedMachine.Model;
        machine.SerialNumber = updatedMachine.SerialNumber;
        machine.Location = updatedMachine.Location;
        machine.IsActive = updatedMachine.IsActive;
        machine.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(machine);
    }

    // DELETE: api/machines/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMachine(int id)
    {
        var machine = await _context.Machines.FindAsync(id);

        if (machine == null)
        {
            return NotFound(new
            {
                message = $"Machine with ID {id} was not found."
            });
        }

        _context.Machines.Remove(machine);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}