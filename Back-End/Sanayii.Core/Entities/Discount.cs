using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.Entities
{
    public class Discount : BaseEntity
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public int? MinRequiredRequests { get; set; }
        public bool? IsFixedAmount { get; set; }
        public bool? IsPercentage { get; set; }
        public DateTime? ExpireDate { get; set; }
        public ICollection<CustomerDiscount> CustomerDiscounts { get; set; } = new HashSet<CustomerDiscount>();
    }
}
