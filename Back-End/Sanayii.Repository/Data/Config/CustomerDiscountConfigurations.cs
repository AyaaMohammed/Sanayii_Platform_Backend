using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sanayii.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Repository.Data.Config
{
    public class CustomerDiscountConfigurations : IEntityTypeConfiguration<CustomerDiscount>
    {
        public void Configure(EntityTypeBuilder<CustomerDiscount> builder)
        {
            // Configure the many-to-many relationship with the extra field in the join table
            builder.HasKey(CD => new { CD.CustomerId, CD.DiscountId });

            builder.HasOne(CD => CD.Customer)
                .WithMany(C => C.CustomerDiscounts)
                .HasForeignKey(CD => CD.CustomerId);

            builder.HasOne(CD => CD.Discount)
                .WithMany(D => D.CustomerDiscounts)
                .HasForeignKey(CD => CD.DiscountId);

            // Optionally: You can specify the table name if you want to use a custom table name
            builder.ToTable("CustomerDiscounts");
        }
    }
}
