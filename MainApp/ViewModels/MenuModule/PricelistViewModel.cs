using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.MenuModule
{
    public class PricelistTypeViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsSelected { get; set; } // for bulk actions
    }

    public class PricelistViewModel
    {
        public long Id { get; set; }
        public long TypeId { get; set; }
        public string TypeName { get; set; } = "";
        public string Name { get; set; } = "";
        public bool IsActive { get; set; } = true;
        public bool IsTemporary { get; set; } = false;
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public string Notes { get; set; } = "";
        public bool IsSelected { get; set; }
    }
}
