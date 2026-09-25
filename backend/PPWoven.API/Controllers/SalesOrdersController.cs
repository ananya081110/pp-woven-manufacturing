using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPWoven.Domain.Entities;
using PPWoven.Infrastructure.Data;

namespace PPWoven.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesOrdersController : ControllerBase
{
    private readonly PPWovenDbContext _context;

    public SalesOrdersController(PPWovenDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SalesOrder>>> GetSalesOrders()
    {
        return await _context.SalesOrders.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SalesOrder>> GetSalesOrder(int id)
    {
        var salesOrder = await _context.SalesOrders.FindAsync(id);

        if (salesOrder == null)
            return NotFound();

        return salesOrder;
    }

    [HttpPost]
    public async Task<ActionResult<SalesOrder>> CreateSalesOrder(
        SalesOrder salesOrder)
    {
        if (string.IsNullOrWhiteSpace(salesOrder.OrderNumber))
            return BadRequest("OrderNumber is required.");

        var customerExists = await _context.Customers
            .AnyAsync(c => c.Id == salesOrder.CustomerId);

        if (!customerExists)
            return BadRequest("Customer does not exist.");

        var orderExists = await _context.SalesOrders
            .AnyAsync(o => o.OrderNumber == salesOrder.OrderNumber);

        if (orderExists)
            return BadRequest("OrderNumber already exists.");

        _context.SalesOrders.Add(salesOrder);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetSalesOrder),
            new { id = salesOrder.Id },
            salesOrder
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesOrder(
        int id,
        SalesOrder salesOrder)
    {
        if (id != salesOrder.Id)
            return BadRequest();

        var existingOrder = await _context.SalesOrders.FindAsync(id);

        if (existingOrder == null)
            return NotFound();

        existingOrder.OrderNumber = salesOrder.OrderNumber;
        existingOrder.CustomerId = salesOrder.CustomerId;
        existingOrder.OrderDate = salesOrder.OrderDate;
        existingOrder.RequiredDeliveryDate =
            salesOrder.RequiredDeliveryDate;
        existingOrder.Priority = salesOrder.Priority;
        existingOrder.Status = salesOrder.Status;
        existingOrder.Notes = salesOrder.Notes;
        existingOrder.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesOrder(int id)
    {
        var salesOrder = await _context.SalesOrders.FindAsync(id);

        if (salesOrder == null)
            return NotFound();

        _context.SalesOrders.Remove(salesOrder);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}