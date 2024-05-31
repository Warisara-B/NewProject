using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
namespace Plexus.Database.Model
{
    [Table("EmployeeExpertises")]
    public class EmployeeExpertise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid WorkInformationId { get; set; }

        public ExpertiseType Type { get; set; }

        public string Major { get; set; }

        public string Minor { get; set; }

        [ForeignKey(nameof(WorkInformationId))]
        public virtual EmployeeWorkInformation WorkInformation { get; set; }
    }
}