using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.UserModule
{
    public class PermissionGroupViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsModelAssociated { get; set; }
        public bool IsSelected { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
