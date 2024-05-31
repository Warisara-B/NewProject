using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Academic;

namespace Plexus.Database.Model.Academic
{
    [Table("WithdrawalRequests")]
    public class WithdrawalRequest
    {
        [Key]
        public Guid Id { get; set; }

        public Guid StudyCourseId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public WithdrawalStatus Status { get; set; }

        public string Remark { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public string? ApprovedBy { get; set; }

        [ForeignKey(nameof(StudyCourseId))]
        public StudyCourse StudyCourse { get; set; }
    }
}

