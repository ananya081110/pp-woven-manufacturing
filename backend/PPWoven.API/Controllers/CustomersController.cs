using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPWoven.Domain.Entities;
using PPWoven.Infrastructure.Data;

namespace PPWoven.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly PPWovenDbContext _context;

    public CustomersController(PPWovenDbContext context)
    {
        _context = context;
    }

    // GET: api/customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        var customers = await _context.Customers
            .OrderBy(c => c.Name)
            .ToListAsync();

        return Ok(customers);
    }

    // GET: api/customers/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(int id)
    {
        var customer = await _context.Customers
            .FindAsync(id);

        if (customer == null)
        {
            return NotFound(new
            {
                message = $"Customer with ID {id} was not found."
            });
        }

        return Ok(customer);
    }

    // POST: api/customers
    [HttpPost]
    public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.CustomerCode))
        {
            return BadRequest(new
            {
                message = "Customer code is required."
            });
        }

        if (string.IsNullOrWhiteSpace(customer.Name))
        {
            return BadRequest(new
            {
                message = "Customer name is required."
            });
        }

        var existingCustomer = await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerCode == customer.CustomerCode);

        if (existingCustomer != null)
        {
            return Conflict(new
            {
                message = $"Customer code '{customer.CustomerCode}' already exists."
            });
        }

        customer.Id = 0;
        customer.CreatedAt = DateTime.UtcNow;
        customer.UpdatedAt = DateTime.UtcNow;
        customer.IsActive = true;

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCustomer),
            new { id = customer.Id },
            customer
        );
    }

    // PUT: api/customers/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(
        int id,
        Customer updatedCustomer)
    {
        var customer = await _context.Customers
            .FindAsync(id);

        if (customer == null)
        {
            return NotFound(new
            {
                message = $"Customer with ID {id} was not found."
            });
        }

        var duplicateCode = await _context.Customers
            .AnyAsync(c =>
                c.CustomerCode == updatedCustomer.CustomerCode &&
                c.Id != id);

        if (duplicateCode)
        {
            return Conflict(new
            {
                message = $"Customer code '{updatedCustomer.CustomerCode}' already exists."
            });
        }

        customer.CustomerCode = updatedCustomer.CustomerCode;
        customer.Name = updatedCustomer.Name;
        customer.Address = updatedCustomer.Address;
        customer.Phone = updatedCustomer.Phone;
        customer.Email = updatedCustomer.Email;
        customer.TaxNumber = updatedCustomer.TaxNumber;
        customer.CreditLimit = updatedCustomer.CreditLimit;
        customer.IsActive = updatedCustomer.IsActive;
        customer.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(customer);
    }

    // DELETE: api/customers/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await _context.Customers
            .FindAsync(id);

        if (customer == null)
        {
            return NotFound(new
            {
                message = $"Customer with ID {id} was not found."
            });
        }

        _context.Customers.Remove(customer);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}