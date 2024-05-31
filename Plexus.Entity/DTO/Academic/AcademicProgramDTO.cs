using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Academic
{
    public class CreateAcademicProgramDTO
    {
        public string Name { get; set; }
        public string FormalName { get; set; }

        public IEnumerable<AcademicProgramLocalizationDTO> Localizations { get; set; }
    }

    public class AcademicProgramDTO : CreateAcademicProgramDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
    }

    public class AcademicProgramLocalizationDTO
    {
		public LanguageCode Language { get; set; }

        public string? Name { get; set; }
        public string? FormalName { get; set; }
    }
}