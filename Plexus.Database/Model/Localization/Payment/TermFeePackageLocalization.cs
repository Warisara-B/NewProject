using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Payment;

namespace Plexus.Database.Model.Localization.Payment
{
    [Table("TermFeePackages", Schema = "localization")]
	public class TermFeePackageLocalization
	{
		public Guid TermFeePackageId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

        [ForeignKey(nameof(TermFeePackageId))]
		public virtual TermFeePackage TermFeePackage { get; set; }
	}
}