using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.CommonModule
{
    public class SettingsViewModel
    {
        public int Id { get; set; }

        public bool IsTaxable { get; set; }

        // Exactly one of these may be true when IsTaxable = true
        public bool IsTaxInclusive { get; set; }
        public bool IsTaxExclusive { get; set; }

        [System.ComponentModel.DataAnnotations.Range(0, 100, ErrorMessage = "Tax rate must be between 0 and 100.")]
        public int TaxRate { get; set; } = 5;

        public bool IsMultiplePriceForItem { get; set; }
    }
}
