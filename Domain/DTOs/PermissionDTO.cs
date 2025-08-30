using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class PermissionDTO
    {
        public string Groupname { get; set; } 
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
