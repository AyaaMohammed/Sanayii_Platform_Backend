using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.Entities
{
    public class CustomerDiscount
    {
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
        public DateTime DateGiven { get; set; }
    }
}
