using Microsoft.EntityFrameworkCore;
using PPWoven.Domain.Entities;

namespace PPWoven.Infrastructure.Data;

public class PPWovenDbContext : DbContext
{
    public PPWovenDbContext(DbContextOptions<PPWovenDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Machine> Machines => Set<Machine>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Shift> Shifts => Set<Shift>();
    public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
    public DbSet<SalesOrderItem> SalesOrderItems => Set<SalesOrderItem>();

}