using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPWoven.Domain.Entities;
using PPWoven.Infrastructure.Data;

namespace PPWoven.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesOrderItemsController : ControllerBase
{
    private readonly PPWovenDbContext _context;

    public SalesOrderItemsController(PPWovenDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SalesOrderItem>>> GetSalesOrderItems()
    {
        return await _context.SalesOrderItems.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SalesOrderItem>> GetSalesOrderItem(int id)
    {
        var item = await _context.SalesOrderItems.FindAsync(id);

        if (item == null)
            return NotFound();

        return item;
    }

    [HttpPost]
    public async Task<ActionResult<SalesOrderItem>> CreateSalesOrderItem(
        SalesOrderItem salesOrderItem)
    {
        var orderExists = await _context.SalesOrders
            .AnyAsync(o => o.Id == salesOrderItem.SalesOrderId);

        if (!orderExists)
            return BadRequest("Sales Order does not exist.");

        var itemExists = await _context.Items
            .AnyAsync(i => i.Id == salesOrderItem.ItemId);

        if (!itemExists)
            return BadRequest("Item does not exist.");

        if (salesOrderItem.Quantity <= 0)
            return BadRequest("Quantity must be greater than zero.");

        if (salesOrderItem.Rate < 0)
            return BadRequest("Rate cannot be negative.");

        salesOrderItem.TotalValue =
            salesOrderItem.Quantity * salesOrderItem.Rate;

        _context.SalesOrderItems.Add(salesOrderItem);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetSalesOrderItem),
            new { id = salesOrderItem.Id },
            salesOrderItem
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesOrderItem(
        int id,
        SalesOrderItem salesOrderItem)
    {
        if (id != salesOrderItem.Id)
            return BadRequest();

        var existingItem =
            await _context.SalesOrderItems.FindAsync(id);

        if (existingItem == null)
            return NotFound();

        existingItem.SalesOrderId = salesOrderItem.SalesOrderId;
        existingItem.ItemId = salesOrderItem.ItemId;
        existingItem.Quantity = salesOrderItem.Quantity;
        existingItem.UOM = salesOrderItem.UOM;
        existingItem.Rate = salesOrderItem.Rate;
        existingItem.TotalValue =
            salesOrderItem.Quantity * salesOrderItem.Rate;
        existingItem.RequiredDeliveryDate =
            salesOrderItem.RequiredDeliveryDate;
        existingItem.Notes = salesOrderItem.Notes;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesOrderItem(int id)
    {
        var item = await _context.SalesOrderItems.FindAsync(id);

        if (item == null)
            return NotFound();

        _context.SalesOrderItems.Remove(item);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}