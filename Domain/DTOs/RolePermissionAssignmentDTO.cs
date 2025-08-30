using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class RolePermissionAssignmentDTO
    {
        public long PermissionId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
        public bool InitiallySelected { get; set; }
    }
}
