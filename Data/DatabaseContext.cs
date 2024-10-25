using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VostokExchRateWebAPI.Models;

namespace VostokExchRateWebAPI.Data;

public class DatabaseContext : DbContext
{

    public DbSet<CurrencyModel> Currencies { get; set; }

    protected readonly IConfiguration Configuration;

    public DatabaseContext(IConfiguration configuration)
    {
        SQLitePCL.Batteries.Init();
        Configuration = configuration;
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(
            Configuration.GetConnectionString("DefaultConnection")
        );
    }
}
