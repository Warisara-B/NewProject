using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Localization.Academic;

namespace Plexus.Database.Model.Academic
{
    [Table("TeachingTypes")]
    public class TeachingType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsActive { get; set; }

        public virtual IEnumerable<TeachingTypeLocalization> Localizations { get; set; }
    }
}

