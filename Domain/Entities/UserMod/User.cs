using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UserMod
{
    public class User : BaseEntity
    {
        public string Fullname { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public DateTime LastLogin { get; set; } = DateTime.MinValue;
        public string Username { get; set; }
        public string Password { get; set; }
        public string Saltkey { get; set; }
        public string Hint { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsLocked { get; set; } = false;
        public int InvalidLogins { get; set; } = 0;
        public DateTime BlockedUntil { get; set; } = DateTime.MinValue;
        public string Remarks { get; set; } = string.Empty;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
}
