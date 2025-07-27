using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sanayii.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Repository.Data
{
    public class SanayiiContext : IdentityDbContext<AppUser>
    {
        public SanayiiContext(DbContextOptions<SanayiiContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Configure Identity Table Names (optional)
            modelBuilder.Entity<AppUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            //// ✅ Use Table-Per-Type (TPT) to create separate tables
            //modelBuilder.Entity<AppUser>().UseTptMappingStrategy();


            // Apply configurations from the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Artisan> Artisans { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<CustomerDiscount> CustomerDiscounts { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<UserPhones> UserPhones { get; set; }

        public DbSet<Service> Service { get; set; }

        public virtual DbSet<ServiceRequestPayment> ServiceRequestPayments { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
    }
}
