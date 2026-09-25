using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPWoven.Domain.Entities;
using PPWoven.Infrastructure.Data;

namespace PPWoven.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductionOperationsController : ControllerBase
{
    private readonly PPWovenDbContext _context;

    public ProductionOperationsController(PPWovenDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductionOperation>>>
        GetProductionOperations()
    {
        return await _context.ProductionOperations.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductionOperation>>
        GetProductionOperation(int id)
    {
        var operation =
            await _context.ProductionOperations.FindAsync(id);

        if (operation == null)
            return NotFound();

        return operation;
    }

    [HttpPost]
    public async Task<ActionResult<ProductionOperation>>
        CreateProductionOperation(
            ProductionOperation operation)
    {
        var productionOrderExists =
            await _context.ProductionOrders
                .AnyAsync(p => p.Id == operation.ProductionOrderId);

        if (!productionOrderExists)
            return BadRequest("Production Order does not exist.");

        if (operation.OperationSequence <= 0)
            return BadRequest(
                "OperationSequence must be greater than zero.");

        if (string.IsNullOrWhiteSpace(operation.OperationName))
            return BadRequest("OperationName is required.");

        if (operation.PlannedQuantity < 0)
            return BadRequest(
                "PlannedQuantity cannot be negative.");

        if (operation.MachineId.HasValue)
        {
            var machineExists = await _context.Machines
                .AnyAsync(m => m.Id == operation.MachineId.Value);

            if (!machineExists)
                return BadRequest("Machine does not exist.");
        }

        if (operation.EmployeeId.HasValue)
        {
            var employeeExists = await _context.Employees
                .AnyAsync(e => e.Id == operation.EmployeeId.Value);

            if (!employeeExists)
                return BadRequest("Employee does not exist.");
        }

        if (operation.ShiftId.HasValue)
        {
            var shiftExists = await _context.Shifts
                .AnyAsync(s => s.Id == operation.ShiftId.Value);

            if (!shiftExists)
                return BadRequest("Shift does not exist.");
        }

        _context.ProductionOperations.Add(operation);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetProductionOperation),
            new { id = operation.Id },
            operation);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductionOperation(
        int id,
        ProductionOperation operation)
    {
        if (id != operation.Id)
            return BadRequest();

        var existingOperation =
            await _context.ProductionOperations.FindAsync(id);

        if (existingOperation == null)
            return NotFound();

        existingOperation.ProductionOrderId =
            operation.ProductionOrderId;

        existingOperation.OperationSequence =
            operation.OperationSequence;

        existingOperation.OperationName =
            operation.OperationName;

        existingOperation.MachineId =
            operation.MachineId;

        existingOperation.EmployeeId =
            operation.EmployeeId;

        existingOperation.ShiftId =
            operation.ShiftId;

        existingOperation.PlannedStartTime =
            operation.PlannedStartTime;

        existingOperation.PlannedEndTime =
            operation.PlannedEndTime;

        existingOperation.ActualStartTime =
            operation.ActualStartTime;

        existingOperation.ActualEndTime =
            operation.ActualEndTime;

        existingOperation.PlannedQuantity =
            operation.PlannedQuantity;

        existingOperation.GoodQuantity =
            operation.GoodQuantity;

        existingOperation.RejectedQuantity =
            operation.RejectedQuantity;

        existingOperation.WasteQuantity =
            operation.WasteQuantity;

        existingOperation.Status =
            operation.Status;

        existingOperation.Remarks =
            operation.Remarks;

        existingOperation.UpdatedAt =
            DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult>
        DeleteProductionOperation(int id)
    {
        var operation =
            await _context.ProductionOperations.FindAsync(id);

        if (operation == null)
            return NotFound();

        _context.ProductionOperations.Remove(operation);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}