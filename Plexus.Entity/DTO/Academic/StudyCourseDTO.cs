using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Plexus.Database.Enum.Academic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Academic
{
    public class CreateStudyCourseDTO
    {
        public Guid StudentId { get; set; }

        public Guid TermId { get; set; }

        public Guid CourseId { get; set; }

        public Guid? SectionId { get; set; }

        public StudyCourseStatus Status { get; set; }

        public RegistrationChannel RegistrationChannel { get; set; }

        public decimal Credit { get; set; }

        public decimal RegistrationCredit { get; set; }

        public Guid? GradeId { get; set; }

        public decimal? GradeWeight { get; set; }

        public DateTime? GradePublishedAt { get; set; }

        public string? Remark { get; set; }
    }

    public class StudyCourseDTO : CreateStudyCourseDTO
    {
        public Guid Id { get; set; }

        public DateTime? PaidAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateStudyCourseDTO
    {
        public Guid Id { get; set; }

        public StudyCourseStatus Status { get; set; }

        public Guid? GradeId { get; set; }

        public decimal? GradeWeight { get; set; }

        public DateTime? GradePublishedAt { get; set; }
    }
}

