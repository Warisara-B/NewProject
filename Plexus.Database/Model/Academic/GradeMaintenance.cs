using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Plexus.Database.Model.Academic
{
    [Table("GradeMaintenance")]
    public class GradeMaintenance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string StudentCode { get; set; }

        public string CoursesCode { get; set; }

        public string Grade { get; set; }

        public string Remark { get; set; }

        public string PathFile { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsActive { get; set; }
    }
}
