using Sanayii.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.DTOs.CustomerDTOs
{
    public class CustomerServiceRequestDto
    {
        public DateTime CreatedAt { get; set; }
        public int ExecutionTime { get; set; }
        public int Status { get; set; }
        public int PaymentMethod { get; set; }      // NEW
        public int PaymentAmount { get; set; }      // NEW
        public string ServiceName { get; set; }
        public string ArtisanName { get; set; }

        public int ServiceId { get; set; }
        public string artisanId { get; set; }

        

    }
}
