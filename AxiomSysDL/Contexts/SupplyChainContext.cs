using Microsoft.EntityFrameworkCore;

namespace AxiomSysDL.Contexts;

public class SupplyChainContext : DbContext
{
    public SupplyChainContext(DbContextOptions<SupplyChainContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
