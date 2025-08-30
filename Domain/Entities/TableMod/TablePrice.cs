using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.TableMod
{
    public class TablePrice : BaseEntity
    {
        public long TableId { get; set; }
        public DiningTable? Table { get; set; }
        public string DayType { get; set; } = string.Empty;
        public TimeOnly FromTime { get; set; }
        public TimeOnly ToTime { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerHour { get; set; }
        public bool IsActive { get; set; }
    }
}
