using Danske.Producer.Domain.Tax;
using Microsoft.EntityFrameworkCore;

namespace Danske.Producer.Infrastructure
{
    public class TaxesDbContext : DbContext
    {
        public DbSet<Tax> Taxes { get; set; }

        public TaxesDbContext(DbContextOptions<TaxesDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}