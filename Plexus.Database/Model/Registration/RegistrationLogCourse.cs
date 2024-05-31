using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model.Registration
{
	[Table("RegistrationLogCourses")]
	public class RegistrationLogCourse
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

        public Guid RegistrationLogId { get; set; }
        
        public Guid StudyCourseId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public RegistrationLogAction Action { get; set; }

        [ForeignKey(nameof(RegistrationLogId))]
        public virtual RegistrationLog Log { get; set; }

        [ForeignKey(nameof(StudyCourseId))]
        public virtual StudyCourse StudyCourse { get; set; }
	}
}

