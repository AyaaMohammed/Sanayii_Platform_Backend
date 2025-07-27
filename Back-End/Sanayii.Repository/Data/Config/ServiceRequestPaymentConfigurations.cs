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
    internal class ServiceRequestPaymentConfigurations : IEntityTypeConfiguration<ServiceRequestPayment>
    {
        public void Configure(EntityTypeBuilder<ServiceRequestPayment> builder)
        {
            // Composite PK for ServiceRequestPayment
            builder.ToTable("ServiceRequestPayment");
            builder.HasKey(SRP => new
            {
                SRP.CustomerId,
                SRP.PaymentId,
                SRP.ServiceId
            });
            builder.Property(SRP => SRP.Status)
             .HasConversion<int>();
        }
    }
}
