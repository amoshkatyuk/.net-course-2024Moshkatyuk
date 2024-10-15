using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data.EntityConfigurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("employee");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Surname).IsRequired().HasMaxLength(100);
            builder.Property(e => e.PassportData).IsRequired().HasMaxLength(11);
            builder.Property(e => e.BirthDate).IsRequired();
            builder.Property(e => e.Salary).IsRequired();
            builder.Property(e => e.Position).IsRequired();
            builder.Property(e => e.Contract).IsRequired();
        }
    }
}
