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
    public class AuditLogConfigurations : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            // Audit Log Configuration

            // Set Primary Key
            builder.HasKey(A => A.AuditId);

            // Make TableName Required and Limit Length
            builder.Property(A => A.TableName)
                  .IsRequired()
                  .HasMaxLength(100);

            // Make Type Required and Limit Length
            builder.Property(A => A.Type)
                  .IsRequired()
                  .HasMaxLength(50);

            // Make UserId Optional but with a Max Length
            builder.Property(A => A.UserId)
                  .HasMaxLength(450);

            // Ensure PrimaryKey is Stored as JSON
            builder.Property(A => A.PrimaryKey)
                  .HasColumnType("NVARCHAR(MAX)");

            // Store OldValues and NewValues as JSON (optional)
            builder.Property(A => A.OldValues)
                  .HasColumnType("NVARCHAR(MAX)");

            builder.Property(A => A.NewValues)
                  .HasColumnType("NVARCHAR(MAX)");

            // Store AffectedColumns as JSON (optional)
            builder.Property(A => A.AffectedColumns)
                  .HasColumnType("NVARCHAR(MAX)");

            // Index for Faster Queries on TableName and Timestamp
            builder.HasIndex(A => A.TableName);
            builder.HasIndex(A => A.Timestamp);

        }
    }
}
