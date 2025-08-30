using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.CustomerModule
{
    public class CustomerAddressViewModel
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string Label { get; set; } = string.Empty;
        public string FlatOrOffice { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
