using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPWoven.Domain.Entities;
using PPWoven.Infrastructure.Data;

namespace PPWoven.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly PPWovenDbContext _context;

    public ItemsController(PPWovenDbContext context)
    {
        _context = context;
    }

    // GET: api/items
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Item>>> GetItems()
    {
        var items = await _context.Items
            .OrderBy(i => i.Name)
            .ToListAsync();

        return Ok(items);
    }

    // GET: api/items/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Item>> GetItem(int id)
    {
        var item = await _context.Items.FindAsync(id);

        if (item == null)
        {
            return NotFound(new
            {
                message = $"Item with ID {id} was not found."
            });
        }

        return Ok(item);
    }

    // POST: api/items
    [HttpPost]
    public async Task<ActionResult<Item>> CreateItem(Item item)
    {
        if (string.IsNullOrWhiteSpace(item.ItemCode))
        {
            return BadRequest(new
            {
                message = "Item code is required."
            });
        }

        if (string.IsNullOrWhiteSpace(item.Name))
        {
            return BadRequest(new
            {
                message = "Item name is required."
            });
        }

        if (string.IsNullOrWhiteSpace(item.ItemType))
        {
            return BadRequest(new
            {
                message = "Item type is required."
            });
        }

        if (string.IsNullOrWhiteSpace(item.UOM))
        {
            return BadRequest(new
            {
                message = "UOM is required."
            });
        }

        var existingItem = await _context.Items
            .FirstOrDefaultAsync(i => i.ItemCode == item.ItemCode);

        if (existingItem != null)
        {
            return Conflict(new
            {
                message = $"Item code '{item.ItemCode}' already exists."
            });
        }

        item.Id = 0;
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;
        item.IsActive = true;

        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetItem),
            new { id = item.Id },
            item
        );
    }

    // PUT: api/items/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(
        int id,
        Item updatedItem)
    {
        var item = await _context.Items.FindAsync(id);

        if (item == null)
        {
            return NotFound(new
            {
                message = $"Item with ID {id} was not found."
            });
        }

        var duplicateCode = await _context.Items
            .AnyAsync(i =>
                i.ItemCode == updatedItem.ItemCode &&
                i.Id != id);

        if (duplicateCode)
        {
            return Conflict(new
            {
                message = $"Item code '{updatedItem.ItemCode}' already exists."
            });
        }

        item.ItemCode = updatedItem.ItemCode;
        item.Name = updatedItem.Name;
        item.ItemType = updatedItem.ItemType;
        item.UOM = updatedItem.UOM;
        item.Description = updatedItem.Description;
        item.IsActive = updatedItem.IsActive;
        item.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(item);
    }

    // DELETE: api/items/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var item = await _context.Items.FindAsync(id);

        if (item == null)
        {
            return NotFound(new
            {
                message = $"Item with ID {id} was not found."
            });
        }

        _context.Items.Remove(item);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}