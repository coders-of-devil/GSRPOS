using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UserMod
{
    public class PermissionGroup : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsModelAssociated { get; set; }
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
