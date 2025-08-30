using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UserMod
{
    public class RolePermission : BaseEntity
    {
        [ForeignKey("Role")]
        public long RoleId { get; set; }
        public Role? Role { get; set; }
        [ForeignKey("Permission")]
        public long PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}
