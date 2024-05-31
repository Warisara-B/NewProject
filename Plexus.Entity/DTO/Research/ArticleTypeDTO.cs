namespace Plexus.Entity.DTO.Research
{
    public class CreateArticleTypeDTO
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<ArticleTypeLocalizationDTO> Localizations { get; set; }
    }

    public class ArticleTypeDTO : CreateArticleTypeDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class ArticleTypeLocalizationDTO : LocalizationDTO
    {
        public string? Name { get; set; }
    }
}