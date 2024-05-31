using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Localization;

namespace Plexus.Database.Model
{
    [Table("EmployeeGroups")]
    public class EmployeeGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public virtual IEnumerable<EmployeeGroupLocalization> Localizations { get; set; }
    }
}