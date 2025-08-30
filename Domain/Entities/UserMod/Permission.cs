using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UserMod
{
    public class Permission : BaseEntity
    {
        [ForeignKey("PermissionGroup")]
        public long PermissionGroupId { get; set; }
        public PermissionGroup? PermissionGroup { get; set; }
        public string Action { get; set; }
        public string Description { get; set; } = string.Empty;
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
