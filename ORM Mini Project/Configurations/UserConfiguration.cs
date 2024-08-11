using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ORM_Mini_Project.Models;
using System.Runtime.ConstrainedExecution;

namespace ORM_Mini_Project.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FullName).IsRequired(true).HasMaxLength(100);
            builder.Property(u => u.Email).IsRequired(true).HasMaxLength(100);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Password).IsRequired(true).HasMaxLength(50);
            builder.Property(u => u.Address).HasMaxLength(200);
        }
    }
}
