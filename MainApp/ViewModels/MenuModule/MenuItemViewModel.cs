using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.MenuModule
{
    public class MenuItemViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public long CategoryId { get; set; }
        public string CategoryName { get; set; } = "";
        public string StockKeepingUnit { get; set; } = "";
        public decimal TaxRate { get; set; } = 5;
        public string Barcode { get; set; } = "";
        public string ImagePath { get; set; } = ""; 
        public int DisplayOrder { get; set; }
        public int ExpectedPreparationTime { get; set; }
        public long PreparationAreaId { get; set; }
        public string PreparationAreaName { get; set; } = "";
        public bool IsActive { get; set; } = true;
        public bool IsAvailableToday { get; set; } = true;

        public bool IsSelected { get; set; } // for bulk actions
        public List<MenuItemTypeViewModel> Types { get; set; } = new();
    }
}
