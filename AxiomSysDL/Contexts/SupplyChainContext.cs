using AxiomSysDL.Models.SupplyChain;
using Microsoft.EntityFrameworkCore;

namespace AxiomSysDL.Contexts;

public class SupplyChainContext : DbContext
{
    public SupplyChainContext(DbContextOptions<SupplyChainContext> options) : base(options)
    {
    }

    public virtual DbSet<SaDeliveryTerm> SaDeliveryTerms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SaDeliveryTerm>(entity =>
        {
            entity.ToTable("SA_DELIVERY_TERMS");
            entity.HasKey(e => e.Code).HasName("SA_DELIVERY_TERMS_PK");

            entity.Property(e => e.Code).HasColumnName("CODE").HasColumnType("NUMBER(3)").ValueGeneratedNever();
            entity.Property(e => e.SName).HasColumnName("S_NAME").HasMaxLength(60).IsUnicode(false).IsRequired();
            entity.Property(e => e.BName).HasColumnName("B_NAME").HasMaxLength(60).IsUnicode(false).IsRequired();
            entity.Property(e => e.Days).HasColumnName("DAYS").HasColumnType("NUMBER(4)");
            entity.Property(e => e.ActiveFlag).HasColumnName("ACTIVE_FLAG").HasDefaultValueSql("1");
            entity.Property(e => e.EntryUser).HasColumnName("ENTRY_USER").HasColumnType("NUMBER(10)");
            entity.Property(e => e.EntryDate).HasColumnName("ENTRY_DATE").HasColumnType("DATE");
            entity.Property(e => e.ChangeUser).HasColumnName("CHANGE_USER").HasColumnType("NUMBER(10)");
            entity.Property(e => e.ChangeDate).HasColumnName("CHANGE_DATE").HasColumnType("DATE");
        });
    }
}
