using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;

namespace Plexus.Database.Model.Localization
{
    [Table("EmployeeGroups", Schema = "localization")]
    public class EmployeeGroupLocalization
    {
        public Guid EmployeeGroupId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

        [ForeignKey(nameof(EmployeeGroupId))]
		public virtual EmployeeGroup EmployeeGroup { get; set; }
    }
}