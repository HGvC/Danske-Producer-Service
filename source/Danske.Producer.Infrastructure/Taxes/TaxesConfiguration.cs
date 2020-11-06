using Danske.Producer.Domain.Tax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Danske.Producer.Infrastructure.Taxes
{
    internal class TaxesConfiguration : IEntityTypeConfiguration<Tax>
    {
        private const int manucipalityLength = 50;

        public void Configure(EntityTypeBuilder<Tax> builder)
        {
            builder.Property(s => s.Municipality)
                .IsUnicode(false)
                .HasMaxLength(manucipalityLength);

            builder.Property(s => s.PeriodType);

            builder.Property(s => s.PeriodStart);

            builder.Property(s => s.PeriodEnd);

            builder.Property(s => s.Result)
                .HasColumnName("Tax")
                .IsRequired();

            builder.HasKey(s => new
            {
                s.Municipality,
                s.PeriodType,
                s.PeriodStart,
                s.PeriodEnd
            });
        }
    }
}