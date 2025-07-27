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
    internal class AdminConfigurations : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            // ✅ Ensure Admin `Id` is also an FK to `AppUser`

            builder.ToTable("Admins")
                .HasOne<AppUser>()
                .WithOne()
                .HasForeignKey<Admin>(A => A.Id);
        }
    }
}
