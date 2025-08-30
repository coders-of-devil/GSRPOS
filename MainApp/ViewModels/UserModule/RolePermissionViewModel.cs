using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.UserModule
{
    public class RolePermissionViewModel
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public long PermissionId { get; set; }
        public string PermissionName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsSelected { get; set; }
    }
}
