using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.UserModule
{
    public class PermissionViewModel
    {
        public long Id { get; set; }
        public long PermissionGroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string Action { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsSelected { get; set; }
    }
}
