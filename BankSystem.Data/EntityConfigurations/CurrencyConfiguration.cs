using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Data.EntityConfigurations
{
    public class CurrencyConfiguration: IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("currency");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Type)
                   .IsRequired()
                   .HasMaxLength(3);

            builder.HasMany(c => c.Accounts)
                   .WithOne(a => a.Currency)
                   .HasForeignKey(a => a.CurrencyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
