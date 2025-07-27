using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Artisan> Artisans { get; set; } = new HashSet<Artisan>();
        public ICollection<Service> Services { get; set; } = new HashSet<Service>();
    }
}
