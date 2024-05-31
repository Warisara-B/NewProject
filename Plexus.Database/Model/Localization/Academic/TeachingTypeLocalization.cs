using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model.Localization.Academic
{
    [Table("TeachingTypes", Schema = "localization")]
	public class TeachingTypeLocalization
	{
		public Guid TeachingTypeId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

		public string? Description { get; set; }

        [ForeignKey(nameof(TeachingTypeId))]
		public virtual TeachingType TeachingType { get; set; }
    }
}

