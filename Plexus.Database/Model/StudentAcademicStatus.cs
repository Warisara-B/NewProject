using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Student;

namespace Plexus.Database.Model
{
    [Table("StudentAcademicStatus")]
    public class StudentAcademicStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public AcademicStatus Status { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string? Remark { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }
    }
}