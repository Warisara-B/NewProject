using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic.Faculty;
namespace Plexus.Database.Model
{
    [Table("EmployeeWorkInformations")]
    public class EmployeeWorkInformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public Guid FacultyId { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid? EmployeeGroupId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public EmployeeType Type { get; set; }

        public string? OfficeRoom { get; set; }

        public string? Remark { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

        [ForeignKey(nameof(FacultyId))]
        public virtual Faculty Faculty { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }

        [ForeignKey(nameof(EmployeeGroupId))]
        public virtual EmployeeGroup? EmployeeGroup { get; set; }

        public virtual IEnumerable<EmployeeExpertise>? Expertises { get; set; }
    }
}

