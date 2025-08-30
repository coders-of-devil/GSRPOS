using Domain.Entities.UserMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.UserModule
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public string Fullname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Hint { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}
