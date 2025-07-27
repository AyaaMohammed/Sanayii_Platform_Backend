using Sanayii.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.Entities
{
    public class ServiceRequestPayment
    {
        public DateTime CreatedAt { get; set; }
        public int ExecutionTime { get; set; }
        public ServiceStatus Status { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public string CustomerId { get; set; }

        [ForeignKey("PaymentId")]
        public Payment Payment { get; set; }
        public int PaymentId { get; set; }

        [ForeignKey("ServiceId")]
        public Service Service { get; set; }
        public int ServiceId { get; set; }

        [ForeignKey("ArtisanId")]
        public Artisan Artisan { get; set; }
        public string ArtisanId { get; set; }
    }
}
