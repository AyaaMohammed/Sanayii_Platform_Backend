using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.Entities
{
    public class Service : BaseEntity
    {
        public string ServiceName { get; set; }
        public double AdditionalPrice { get; set; }
        public string Description { get; set; }
        public double BasePrice { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public ICollection<ServiceRequestPayment> ServiceRequestPayments { get; set; } = new HashSet<ServiceRequestPayment>();
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    }
}
