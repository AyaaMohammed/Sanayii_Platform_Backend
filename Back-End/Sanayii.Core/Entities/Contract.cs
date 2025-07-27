using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.Entities
{
    public class Contract : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxViolationsAllowed { get; set; }
        public string Status { get; set; }

        [ForeignKey("ArtisanId")]
        public Artisan Artisan { get; set; }
        public string ArtisanId { get; set; }
        public ICollection<Violation> Violations { get; set; } = new HashSet<Violation>();
    }
}
