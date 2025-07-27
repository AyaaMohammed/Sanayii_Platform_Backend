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
    internal class ArtisanConfigurations : IEntityTypeConfiguration<Artisan>
    {
        public void Configure(EntityTypeBuilder<Artisan> builder)
        {
            // ✅ Ensure Artisan `Id` is also an FK to `AppUser`

            builder.ToTable("Artisans")
                .HasOne<AppUser>()
                .WithOne()
                .HasForeignKey<Artisan>(A => A.Id);
        }
    }
}
