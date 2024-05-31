using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Payment;

namespace Plexus.Database.Model.Localization.Payment
{
    [Table("RateTypes", Schema = "localization")]
	public class RateTypeLocalization
	{
		public Guid RateTypeId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

        [ForeignKey(nameof(RateTypeId))]
		public virtual RateType RateType { get; set; }
	}
}