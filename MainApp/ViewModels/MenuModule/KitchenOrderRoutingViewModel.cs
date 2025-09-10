using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.MenuModule
{
    public class KitchenOrderRoutingViewModel
    {
        public long Id { get; set; }
        public long MenuItemId { get; set; }
        public string MenuItemName { get; set; } = string.Empty;
        public long KitchenDeviceId { get; set; }
        public string KitchenDeviceName { get; set; } = string.Empty;
        public int Copies { get; set; } = 1;
    }
}
