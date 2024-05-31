using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic
{
    [Table("GradeLogs")]
    public class GradeLog
    {
        [Key]
        public long Id { get; set; }

        public Guid StudyCourseId { get; set; }

        public string? FromGrade { get; set; }

        public string? ToGrade { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        [ForeignKey(nameof(StudyCourseId))]
        public virtual StudyCourse StudyCourse { get; set; }
    }
}

