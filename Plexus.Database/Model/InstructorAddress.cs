using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model
{
    [Table("InstructorAddresses")]
    public class InstructorAddress
    {
        [Key]
        public Guid InstructorId { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }

        public string Province { get; set; }

        public string? District { get; set; }

        public string? SubDistrict { get; set; }

        public string? State { get; set; }

        public string? City { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber { get; set; }

        public string? PhoneNumber2 { get; set; }

        public string? EmailAddress { get; set; }

        public string? PersonalEmailAddress { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(InstructorId))]
        public virtual Employee Instructor { get; set; }
    }
}