using Sanayii.Core.Entities;
using Sanayii.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Repository.Repository
{
    public class PaymentRepository : GenericRepository<Payment>
    {

        public PaymentRepository(SanayiiContext db) : base(db)
        {
        }
        public List<Payment> GetPaymentsByCustomerId(string customerId)
        {
            var payments = (from p in db.Payments
                            join srp in db.ServiceRequestPayments on p.Id equals srp.PaymentId
                            where srp.CustomerId == customerId
                            select p).ToList();
            return payments;
        }
    }
}
