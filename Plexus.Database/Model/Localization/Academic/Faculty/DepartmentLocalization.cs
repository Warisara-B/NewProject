using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic.Faculty;

namespace Plexus.Database.Model.Localization.Academic.Faculty
{
    [Table("Departments", Schema = "localization")]
	public class DepartmentLocalization
	{
        public Guid DepartmentId { get; set; }

        public LanguageCode Language { get; set; }

        public string? Name { get; set; }

        public string? FormalName { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }
    }
}

