using Microsoft.EntityFrameworkCore;
using Sanayii.Repository;
using Sanayii.Core.Entities;
using Sanayii.Repository.Data;

namespace Sanayii.Core.Repository
{
    public class AdminRepository : GenericRepository<Admin>
    {
        public AdminRepository(SanayiiContext db) : base(db)
        {
        }
        public List<Admin> GetAllAdmins()
        { 
           return db.Admins.Where(Admin=>Admin.IsDeleted == false).ToList();
        }
        public  void AddAdmin (Admin admin)
        {
            
            db.Admins.Add(admin);
        }

    }
}