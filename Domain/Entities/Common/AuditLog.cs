using Domain.Entities.UserMod;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Common
{
    public class AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User? User { get; set; }
        public string Action { get; set; }
        public string Module { get; set; }
        public long EntityId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
