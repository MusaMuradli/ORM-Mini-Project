using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ORM_Mini_Project.Models;

namespace ORM_Mini_Project.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasOne<Order>().WithMany().HasForeignKey(od => od.OrderId);
            builder.HasOne<Product>().WithMany().HasForeignKey(od => od.ProductId);
            builder.Property(od => od.Quantity).IsRequired();
            builder.Property(od => od.PricePerItem).HasColumnType("decimal(18,2)");
            throw new NotImplementedException();
        }
    }
}
