using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MenuMod
{
    public class KitchenOrderRouting : BaseEntity
    {
        public long MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }
        public long KitchenDeviceId { get; set; }
        public KitchenDevice? KitchenDevice { get; set; }
        public int Copies { get; set; } = 1;
    }
}
