using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model.Registration
{
	[Table("RegistrationLogs")]
	public class RegistrationLog
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid TermId { get; set; }

		public Guid StudentId { get; set; }

		[Column(TypeName = "nvarchar(100)")]
        public RegistrationChannel RegistrationChannel { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		[ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }

        [ForeignKey(nameof(TermId))]
        public virtual Term Term { get; set; }
	}
}

