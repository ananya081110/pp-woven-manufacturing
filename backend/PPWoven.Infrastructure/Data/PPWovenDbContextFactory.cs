using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PPWoven.Infrastructure.Data;

public class PPWovenDbContextFactory : IDesignTimeDbContextFactory<PPWovenDbContext>
{
    public PPWovenDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets("08d08bb7-9607-4995-b203-d8acb1bd9556")
            .Build();

        var connectionString =
            configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Database connection string not found in User Secrets."
            );
        }

        var optionsBuilder =
            new DbContextOptionsBuilder<PPWovenDbContext>();

        optionsBuilder.UseNpgsql(connectionString);

        return new PPWovenDbContext(optionsBuilder.Options);
    }
}