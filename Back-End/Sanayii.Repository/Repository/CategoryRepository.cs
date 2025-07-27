using Sanayii.Core.Entities;
using Sanayii.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Repository.Repository
{
    public class CategoryRepository:GenericRepository<Category>
    {
        public CategoryRepository(SanayiiContext db) : base(db)
        {
        }
        public Category getByName(string name)
        {
            return db.Categories.FirstOrDefault(c => c.Name == name);
        }
    }
}
