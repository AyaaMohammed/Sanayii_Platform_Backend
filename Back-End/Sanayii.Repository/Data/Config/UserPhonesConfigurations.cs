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
    public class UserPhonesConfigurations : IEntityTypeConfiguration<UserPhones>
    {
        public void Configure(EntityTypeBuilder<UserPhones> builder)
        {
            // Composite PK for UserPhones
            builder.HasKey(UP => new
            {
                UP.UserId,
                UP.PhoneNumber
            });
        }
    }
}
