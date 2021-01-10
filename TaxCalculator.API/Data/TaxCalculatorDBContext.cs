using Microsoft.EntityFrameworkCore;
using TaxCalculator.API.Data.Models;

namespace TaxCalculator.API.Data
{
    public partial class TaxCalculatorDBContext : DbContext
    {
        public TaxCalculatorDBContext()
        {
        }

        public TaxCalculatorDBContext(DbContextOptions<TaxCalculatorDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PostalCode> PostalCodes { get; set; }
        public virtual DbSet<Tax> Taxes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<PostalCode>(entity =>
            {
                entity.ToTable("PostalCode");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TaxCalculationType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tax>(entity =>
            {
                entity.ToTable("Tax");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.AnnualIncome).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CalculatedTaxValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PostalCodeId).HasColumnName("PostalCodeID");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.PostalCode)
                    .WithMany(p => p.Taxes)
                    .HasForeignKey(d => d.PostalCodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tax_PostalCode");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
