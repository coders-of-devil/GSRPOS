using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.CustomerModule
{
    public class CustomerViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateOnly? DOB { get; set; }
        public string Gender { get; set; } = string.Empty;
        public long? MembershipId { get; set; }
        public string MembershipName { get; set; } = string.Empty;
        public decimal LoyaltyPoints { get; set; }
        public decimal CreditBalance { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        public bool IsSelected { get; set; }
        public List<CustomerAddressViewModel> Addresses { get; set; } = new();
    }
}
