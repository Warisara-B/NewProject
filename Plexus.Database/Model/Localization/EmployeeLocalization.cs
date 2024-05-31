using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;

namespace Plexus.Database.Model.Localization
{
    [Table("Employees", Schema = "localization")]
    public class EmployeeLocalization
    {
        public Guid EmployeeId { get; set; }

        public LanguageCode Language { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }
    }
}