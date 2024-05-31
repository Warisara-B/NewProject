using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic;

namespace Plexus.Database.Model.Academic
{
    [Table("StudyCourses")]
    public class StudyCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }

        public Guid TermId { get; set; }

        public Guid CourseId { get; set; }

        public Guid? SectionId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public StudyCourseStatus Status { get; set; }
        
        [Column(TypeName = "nvarchar(100)")]
        public RegistrationChannel RegistrationChannel { get; set; }

        public Guid? SectionSeatId { get; set; }

        public decimal Credit { get; set; }

        public decimal RegistrationCredit { get; set; }

        public Guid? GradeId { get; set; }

        public decimal? GradeWeight { get; set; }

        public DateTime? PaidAt { get; set; }

        public DateTime? GradePublishedAt { get; set; }

        public string? Remark { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }

        [ForeignKey(nameof(TermId))]
        public virtual Term Term { get; set; }

        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }

        [ForeignKey(nameof(SectionId))]
        public virtual Section.Section? Section { get; set; }

        [ForeignKey(nameof(SectionSeatId))]
        public virtual SectionSeat? SectionSeat { get; set; }

        [ForeignKey(nameof(GradeId))]
        public virtual Grade? Grade { get; set; }
    }
}

