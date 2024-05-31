using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Academic
{
    public class CreateDepartmentDTO
	{
        public Guid FacultyId { get; set; }
        
		public string Code { get; set; }
		
		public string Name { get; set; }

		public string? FormalName { get; set; }

		public string? LogoImagePath { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<DepartmentLocalizationDTO> Localizations { get; set; }
	}

	public class DepartmentDTO : CreateDepartmentDTO
	{
		public Guid Id { get; set; }
		
		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
	}

	public class DepartmentLocalizationDTO
	{
		public LanguageCode Language { get; set; }

        public string? Name { get; set; }

        public string? FormalName { get; set; }
	}
}

