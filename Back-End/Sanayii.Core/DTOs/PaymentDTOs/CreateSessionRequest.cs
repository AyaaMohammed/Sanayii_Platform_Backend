using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.DTOs.PaymentDTOs
{
    public  class CreateSessionRequest
    {
        public int PaymentId { get; set; }
        public int Amount { get; set; }
        public string ProductName { get; set; }
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
        public string CustomerId { get; set; }
        public int ServiceId { get; set; }
    }
}
