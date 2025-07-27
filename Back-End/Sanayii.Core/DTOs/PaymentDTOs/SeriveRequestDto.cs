using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.DTOs.PaymentDTOs
{
    public class SeriveRequestDto
    {
        // category data
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        // service data 
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public DateTime RequestDate { get; set; }

        // payment data
        public int PaymentId { get; set; }
        public int PaymentMethod { get; set; }

        // customer data
        public string CustomerName { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerGovernment { get; set; }
        public string CustomerStreet { get; set; }
        public List<string> CustomerPhoneNumbers { get; set; }

        // artisan data
        public string artisanId { get; set; }
        public string ArtisanName { get; set; }
    }
}
