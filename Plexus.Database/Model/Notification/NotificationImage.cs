using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Database.Model.Notification
{
    [Table("NotificationImages")]
    public class NotificationImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }

        public string UrlImage { get; set; }

        [ForeignKey(nameof(NotificationId))]
        public virtual Notification Notification { get; set; }
    }
}
