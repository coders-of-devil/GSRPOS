using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UserMod
{
    public class UserPass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User? User { get; set; }
        public string UsersPass { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
        public DateTime ModifiedAt { get; set; } = DateTime.MinValue;
        public bool IsSynced { get; set; } = false;
        public DateTime SyncTime { get; set; } = DateTime.MinValue;
    }
}
