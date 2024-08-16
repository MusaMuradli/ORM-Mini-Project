using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using ORM_Mini_Project.Models;

namespace ORM_Mini_Project.Configurations
{
    public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.OrderDate).HasDefaultValueSql("GETDATE()");
            builder.Property(o => o.TotalAmount).IsRequired(true).HasDefaultValue(0).HasColumnType("decimal(10,2)");
            //builder.HasOne<User>().WithMany().HasForeignKey(o => o.UserId);
            builder.Property(o => o.Status).IsRequired(true);
        }
    }
}
