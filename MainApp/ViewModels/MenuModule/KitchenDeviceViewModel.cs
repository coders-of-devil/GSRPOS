using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModels.MenuModule
{
    public class KitchenDeviceViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string ConnectionType { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public long? PreparationAreaId { get; set; }
        public string PreparationAreaName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int DefaultCopies { get; set; } = 1;
        public bool IsSelected { get; set; }
    }

}
