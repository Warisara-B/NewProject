using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;

namespace Plexus.Database.Model
{
	public class ApplicationUser
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string Username { get; set; }

		public string HashedPassword { get; set; }

		public string HashedKey { get; set; }

		public Guid? StudentId { get; set; }

		public Guid? InstructorId { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public ApplicationUserStatus Status { get; set; }

		public DateTime? LastLoginAt { get; set; }

		public DateTime? CreatedAt { get; set; }

		public string? CreatedBy { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public string? UpdateBy { get; set; }

		[ForeignKey(nameof(StudentId))]
		public virtual Student? Student { get; set; }

		[ForeignKey(nameof(InstructorId))]
		public virtual Employee? Instructor { get; set; }
	}
}

