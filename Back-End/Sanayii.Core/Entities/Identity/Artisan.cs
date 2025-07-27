using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.Entities.Identity
{
    public class Artisan : AppUser
    {
        public int NationalityId { get; set; }
        public int Rating { get; set; }
        public Contract Contract { get; set; }
        public Category Category { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    }
}
