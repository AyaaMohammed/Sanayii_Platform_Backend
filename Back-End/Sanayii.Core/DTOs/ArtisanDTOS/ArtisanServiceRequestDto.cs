using Sanayii.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.DTOs.ArtisanDTOS
{
    public class ArtisanServiceRequestDto
    {
        public DateTime CreatedAt { get; set; }
        public int ExecutionTime { get; set; }
        public string Status { get; set; }

        public string ServiceName { get; set; }

        public string CustomerName { get; set; } 

        public int rating { get; set; }
    }
}
