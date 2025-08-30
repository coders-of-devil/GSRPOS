using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.MenuModule
{
    public class MenuItemTypeViewModel
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public string TypeName { get; set; } = "";
        public long PricelistId { get; set; }
        public string PricelistName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; } = "";
    }
}
