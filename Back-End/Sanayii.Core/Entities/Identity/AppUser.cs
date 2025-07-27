using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Government { get; set; }

        public bool IsDeleted { get; set; } = false;//default value is false

        //navigation property as user can have many phone numbers.
        public ICollection<UserPhones> UserPhones { get; set; } = new HashSet<UserPhones>();
    }
}
