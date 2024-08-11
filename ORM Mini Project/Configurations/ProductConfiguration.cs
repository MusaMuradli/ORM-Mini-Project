using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ORM_Mini_Project.Models;

namespace ORM_Mini_Project.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Price).HasColumnType("decimal(10,2)");
            builder.Property(p => p.Stock).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.UpdatedDate).HasDefaultValueSql("GETDATE()");

            builder.HasCheckConstraint("CK_Price", "Price > 0");
            builder.HasCheckConstraint("CK_Stock", "Stock >= 0");
        }
    }
}
