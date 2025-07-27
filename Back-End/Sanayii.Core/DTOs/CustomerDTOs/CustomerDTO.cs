using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.DTOs.CustomerDTOs
{
    public class CustomerDTO
    {
        public string Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string userName {  get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Government { get; set; }
        public string Email { get; set; }
        public List<string> phoneNumber { get; set; } = new List<string>();
    }
    
}
