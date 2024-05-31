using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Notification
{
    [Table("NotificationStudents")]
    public class NotificationStudent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }

        public Guid? StudentId { get; set; }

        public bool HasRead { get; set; }

        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(NotificationId))]
        public virtual Notification Notification { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student? Student { get; set; }
    }
}