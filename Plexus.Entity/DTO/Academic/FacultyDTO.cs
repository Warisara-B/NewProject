using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Academic
{
    public class CreateFacultyDTO
    {
        public string Code { get; set; }
		
		public string Name { get; set; }

		public string? FormalName { get; set; }

		public string? LogoImagePath { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<FacultyLocalizationDTO> Localizations { get; set; }
    }

    public class FacultyDTO : CreateFacultyDTO
    {
        public Guid Id { get; set; }
       
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class FacultyLocalizationDTO
    {
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

		public string? FormalName { get; set; }
    }
}