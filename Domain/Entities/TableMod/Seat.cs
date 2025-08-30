using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.TableMod
{
    public class Seat : BaseEntity
    {
        [ForeignKey("Table")]
        public long TableId { get; set; }
        public DiningTable? Table { get; set; }
        public string Name { get; set; }
        public bool IsOccupied { get; set; } = false;
    }
}
