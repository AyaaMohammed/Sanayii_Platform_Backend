using Microsoft.EntityFrameworkCore;
using Sanayii.Core.Entities;
using Sanayii.Core.Entities.Identity;
using Sanayii.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Repository.Repository
{
    public class DiscountRepository:GenericRepository<Discount>
    {

        public DiscountRepository(SanayiiContext db) : base(db)
        {
        }
        public List<Discount> GetCustomerDiscounts(string customerid)
        {
            var discounts = (from cd in db.CustomerDiscounts
                             join d in db.Discount on cd.DiscountId equals d.Id
                             where cd.CustomerId == customerid
                             select d).ToList();
            return discounts;

        }
    }
}
