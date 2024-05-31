using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Plexus.Database.Model
{
    [Table("EmployeeEducationalBackgrounds")]
    public class EmployeeEducationalBackground
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public string Institute { get; set; }

        public string DegreeLevel { get; set; }

        public string DegreeName { get; set; }

        public string? Branch { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? Country { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }
    }
}