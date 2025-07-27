using Sanayii.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.DTOs
{
    public class ReviewDTO
    {
        public DateTime ReviewDate { get; set; }
        public int Rating { get; set; }
        public string CustomerId { get; set; }
        public string ArtisanId { get; set; }
        public int ServiceId { get; set; }
        public string Comment { get; set; }
    }
}
