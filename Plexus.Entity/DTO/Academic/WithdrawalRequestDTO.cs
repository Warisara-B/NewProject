using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Plexus.Database.Enum.Academic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Entity.DTO.Academic
{
    public class WithdrawalRequestDTO
    {
        public Guid Id { get; set; }

        public Guid StudyCourseId { get; set; }

        public WithdrawalStatus Status { get; set; }

        public Guid StudentId { get; set; }

        public Guid CourseId { get; set; }

        public Guid? SectionId { get; set; }

        public Guid TermId { get; set; }

        public string Remark { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? ApprovedAt { get; set; }
    }
}

