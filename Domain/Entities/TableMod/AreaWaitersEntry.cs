using Domain.Entities.Common;
using Domain.Entities.UserMod;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.TableMod
{
    public class AreaWaitersEntry : BaseEntity
    {
        [ForeignKey("Area")]
        public long AreaId { get; set; }
        public DiningArea? Area { get; set; }
        [ForeignKey("Waiter")]
        public long WaiterId { get; set; }
        public User? Waiter { get; set; }
        public DateTime AssignedOn { get; set; }
        public string Notes { get; set; } = string.Empty;

    }
}
