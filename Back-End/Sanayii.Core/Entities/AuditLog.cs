using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Core.Entities
{
    public class AuditLog
    {
        public int AuditId { get; set; } // Primary Key

        public string UserId { get; set; } // User who performed the action
        public string Type { get; set; } // INSERT, UPDATE, DELETE
        public string TableName { get; set; } // Name of the affected table
        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Action timestamp
        public string OldValues { get; set; } // JSON storing old values
        public string NewValues { get; set; } // JSON storing new values
        public string AffectedColumns { get; set; } // Only affected columns
        public string PrimaryKey { get; set; } // Primary key of the affected row
    }
}
