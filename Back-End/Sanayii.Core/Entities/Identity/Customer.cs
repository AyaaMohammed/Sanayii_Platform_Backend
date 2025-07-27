using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.Entities.Identity
{
    public class Customer : AppUser
    {
        public Discount Discount { get; set; }
        public ICollection<ServiceRequestPayment> ServiceRequestPayments { get; set; }

        public ICollection<Review> Reviews { get; set; }
        public ICollection<CustomerDiscount> CustomerDiscounts { get; set; }
    }
}
