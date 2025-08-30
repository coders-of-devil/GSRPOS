using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UserMod
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public bool IsSystemRole { get; set; }
        public bool IsPermissionAssigned { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
