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
    public class ViolationConfigurations : IEntityTypeConfiguration<Violation>
    {
        public void Configure(EntityTypeBuilder<Violation> builder)
        {
            // Configure Violations Table
            builder.HasKey(V => V.Id);

            builder.Property(V => V.Id)
                  .ValueGeneratedOnAdd();


            builder.HasOne(v => v.Contract)
                  .WithMany(c => c.Violations)
                  .HasForeignKey(v => v.ContractId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
