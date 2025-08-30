using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UserMod
{
    public class UserRole : BaseEntity
    {
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User? User { get; set; }
        [ForeignKey("Role")]
        public long RoleId { get; set; }
        public Role? Role { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
