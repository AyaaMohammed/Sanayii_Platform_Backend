using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.Entities.Identity
{
    public class Admin : AppUser
    {
        public decimal Salary { get; set; }
    }
}
