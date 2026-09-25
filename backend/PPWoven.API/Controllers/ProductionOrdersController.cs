using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPWoven.Domain.Entities;
using PPWoven.Infrastructure.Data;

namespace PPWoven.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductionOrdersController : ControllerBase
{
    private readonly PPWovenDbContext _context;

    public ProductionOrdersController(PPWovenDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductionOrder>>> GetProductionOrders()
    {
        return await _context.ProductionOrders.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductionOrder>> GetProductionOrder(int id)
    {
        var productionOrder =
            await _context.ProductionOrders.FindAsync(id);

        if (productionOrder == null)
            return NotFound();

        return productionOrder;
    }

    [HttpPost]
    public async Task<ActionResult<ProductionOrder>> CreateProductionOrder(
        ProductionOrder productionOrder)
    {
        if (string.IsNullOrWhiteSpace(
            productionOrder.ProductionOrderNumber))
        {
            return BadRequest(
                "ProductionOrderNumber is required.");
        }

        if (productionOrder.PlannedQuantity <= 0)
            return BadRequest(
                "PlannedQuantity must be greater than zero.");

        var itemExists = await _context.Items
            .AnyAsync(i => i.Id == productionOrder.ItemId);

        if (!itemExists)
            return BadRequest("Item does not exist.");

        if (productionOrder.SalesOrderId.HasValue)
        {
            var salesOrderExists = await _context.SalesOrders
                .AnyAsync(o =>
                    o.Id == productionOrder.SalesOrderId.Value);

            if (!salesOrderExists)
                return BadRequest(
                    "Sales Order does not exist.");
        }

        var orderExists = await _context.ProductionOrders
            .AnyAsync(o =>
                o.ProductionOrderNumber ==
                productionOrder.ProductionOrderNumber);

        if (orderExists)
            return BadRequest(
                "Production Order Number already exists.");

        _context.ProductionOrders.Add(productionOrder);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetProductionOrder),
            new { id = productionOrder.Id },
            productionOrder);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductionOrder(
        int id,
        ProductionOrder productionOrder)
    {
        if (id != productionOrder.Id)
            return BadRequest();

        var existingOrder =
            await _context.ProductionOrders.FindAsync(id);

        if (existingOrder == null)
            return NotFound();

        existingOrder.ProductionOrderNumber =
            productionOrder.ProductionOrderNumber;

        existingOrder.SalesOrderId =
            productionOrder.SalesOrderId;

        existingOrder.ItemId =
            productionOrder.ItemId;

        existingOrder.PlannedQuantity =
            productionOrder.PlannedQuantity;

        existingOrder.UOM =
            productionOrder.UOM;

        existingOrder.PlannedStartDate =
            productionOrder.PlannedStartDate;

        existingOrder.PlannedEndDate =
            productionOrder.PlannedEndDate;

        existingOrder.Priority =
            productionOrder.Priority;

        existingOrder.Status =
            productionOrder.Status;

        existingOrder.Notes =
            productionOrder.Notes;

        existingOrder.UpdatedAt =
            DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductionOrder(int id)
    {
        var productionOrder =
            await _context.ProductionOrders.FindAsync(id);

        if (productionOrder == null)
            return NotFound();

        _context.ProductionOrders.Remove(productionOrder);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}