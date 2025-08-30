using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.UserModule
{
    public class RolePermissionAssignmentViewModel
    {
        public long PermissionId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
