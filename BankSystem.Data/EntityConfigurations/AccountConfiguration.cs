using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.EntityConfigurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("account");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Amount).IsRequired();

            builder.HasOne(a => a.Currency)
                   .WithMany()
                   .HasForeignKey(a => a.CurrencyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Client)
                   .WithMany(c => c.Accounts)
                   .HasForeignKey(a => a.ClientId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
