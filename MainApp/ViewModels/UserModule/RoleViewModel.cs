using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.UserModule
{
    public class RoleViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsSystemRole { get; set; }
        public bool IsPermissionAssigned { get; set; }
        public bool IsActive { get; set; }
        public bool IsSelected { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
