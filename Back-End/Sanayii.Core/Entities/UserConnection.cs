using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.Entities
{
    public class UserConnection
    {
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
