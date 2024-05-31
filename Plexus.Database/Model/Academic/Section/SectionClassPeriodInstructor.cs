using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic.Section
{
    [Table("SectionClassPeriodInstructors")]
    public class SectionClassPeriodInstructor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid SectionClassPeriodId { get; set; }

        public Guid InstructorId { get; set; }

        [ForeignKey(nameof(SectionClassPeriodId))]
        public virtual SectionClassPeriod SectionClassPeriod { get; set; }

        [ForeignKey(nameof(InstructorId))]
        public virtual Employee Instructor { get; set; }

    }
}