using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CustomerMod
{
    public class CustomerAddress : BaseEntity
    {
        [Required]
        public long CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public string Label { get; set; } = string.Empty;
        public string FlatOrOffice { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public bool IsDefault { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
