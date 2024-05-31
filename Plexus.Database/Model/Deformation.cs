using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model
{
    [Table("Deformations")]
    public class Deformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }

        public string Name { get; set; }

        public string BookCode { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime ExpiredAt { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }
    }
}