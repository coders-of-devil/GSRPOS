using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UserMod
{
    public class UserPermission : BaseEntity
    {
        public long UserId { get; set; }
        public User? User { get; set; }
        public long PermissionId { get; set; }
        public Permission? Permission { get; set; }
        public bool IsOverWriting { get; set; }
        public bool IsUnderWriting { get; set; }
        public bool IsTemporary { get; set; }
        public DateTime AssignedUntil { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}
