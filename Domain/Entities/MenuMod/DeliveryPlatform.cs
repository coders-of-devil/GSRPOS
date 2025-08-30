using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MenuMod
{
    public class DeliveryPlatform : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
