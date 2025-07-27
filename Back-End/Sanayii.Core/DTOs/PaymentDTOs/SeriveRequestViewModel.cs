using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.DTOs.PaymentDTOs
{
    public class SeriveRequestViewModel
    {
        public int CategoryId { get; set; }
        public string CustomerId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public DateTime RequestDate { get; set; }
        public int Status { get; set; }
        public int PaymentMethod { get; set; }
    }
}
