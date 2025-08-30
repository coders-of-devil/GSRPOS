using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.MenuModule
{
    public class MenuItemPricelistViewModel
    {
        public long Id { get; set; }
        public long PriceListId { get; set; }
        public long MenuItemId { get; set; }
        public string PriceListName { get; set; } = string.Empty;
        public string MenuItemName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
