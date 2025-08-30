using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.TableMod
{
    public class DiningTable : BaseEntity
    {
        [ForeignKey("Area")]
        public long AreaId { get; set; }
        public DiningArea? Area { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsOccupied { get; set; } = false;
        public bool IsReservable { get; set; } = true;
    }
}
