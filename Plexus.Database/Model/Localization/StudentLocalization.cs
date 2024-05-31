using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;

namespace Plexus.Database.Model.Localization
{
    [Table("Students", Schema = "localization")]
    public class StudentLocalization
    {
        public Guid StudentId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }
    }
}

