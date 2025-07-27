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
    public class ReviewConfigurations : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            // Configure foreign key constraints for Reviews table
            builder.HasOne(R => R.Customer)
                .WithMany(C => C.Reviews)
                .HasForeignKey(R => R.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.HasOne(R => R.Artisan)
               .WithMany(A => A.Reviews)
               .HasForeignKey(R => R.ArtisanId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(R => R.Service)
                .WithMany(S => S.Reviews)
                .HasForeignKey(R => R.ServiceId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
