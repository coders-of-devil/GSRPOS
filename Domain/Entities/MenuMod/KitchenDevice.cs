using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MenuMod
{
    public class KitchenDevice : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string ConnectionType { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public long? PreparationAreaId { get; set; }
        public PreparationArea? PreparationArea { get; set; }
        public bool IsActive { get; set; } = true;
        public int DefaultCopies { get; set; } = 1;
    }
}
