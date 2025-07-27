using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.Entities
{
    public class UserPhones
    {
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }

        // Navigation property:the relationship between User and UserPhones is one-to-many
        public AppUser User { get; set; }
    }
}
