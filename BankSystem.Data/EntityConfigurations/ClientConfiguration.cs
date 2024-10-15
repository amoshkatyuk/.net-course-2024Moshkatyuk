using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Data.EntityConfigurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("client");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Surname).IsRequired().HasMaxLength(100);
            builder.Property(c => c.PassportData).IsRequired().HasMaxLength(11);
            builder.Property(c => c.BirthDate).IsRequired();
            builder.Property(c => c.TelephoneNumber).IsRequired().HasMaxLength(11);

            builder.HasMany(c => c.Accounts)
                   .WithOne(a => a.Client)
                   .HasForeignKey(a => a.ClientId);
        }
    }
}
