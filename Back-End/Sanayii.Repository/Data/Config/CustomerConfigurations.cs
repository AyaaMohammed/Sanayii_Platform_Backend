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
    internal class CustomerConfigurations : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // ✅ Ensure Customer `Id` is also an FK to `AppUser`

            builder.ToTable("Customer")
                .HasOne<AppUser>()
                .WithOne()
                .HasForeignKey<Customer>(C => C.Id);
        }
    }
}
