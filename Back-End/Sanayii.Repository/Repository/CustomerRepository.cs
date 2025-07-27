using Microsoft.EntityFrameworkCore;
using Sanayii.Core.Entities;
using Sanayii.Repository;
using Sanayii.Repository.Data;
namespace Sanayii.Core.Repository
{
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository(SanayiiContext db) : base(db)
        {
        }

        public IQueryable<Customer> GetAllCustomers()
        {
            return db.Customers.Where(c => c.IsDeleted == false);
        }
        public Customer GetCustomerById(string id)
        {
            return db.Customers
                .Include(c=>c.UserPhones)
                .FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
        }
        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }

}