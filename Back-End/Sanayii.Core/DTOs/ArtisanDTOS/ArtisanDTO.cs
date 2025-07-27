using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.DTOs.ArtisanDTOS
{
    public class ArtisanDTO
    {
        public string Id { get; set; }
        public int Rating { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Government { get; set; }
        public List<string> UserPhones { get; set; } = new List<string>();
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
