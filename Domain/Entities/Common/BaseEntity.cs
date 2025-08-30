using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Common
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
        public DateTime ModifiedAt { get; set; } = DateTime.MinValue;
        public bool IsDeleted { get; set; } = false;
        public bool IsSynced { get; set; } = false;
        public DateTime SyncTime { get; set; } = DateTime.MinValue;
    }
}
