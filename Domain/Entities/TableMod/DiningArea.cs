using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.TableMod
{
    public class DiningArea :BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsReservable { get; set; } = true;
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
