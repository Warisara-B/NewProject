namespace Plexus.Entity.DTO
{
    public class CreatePublicationDTO
    {
        public Guid ArticleTypeId { get; set; }

        public string Authors { get; set; }

        public int Pages { get; set; }

        public int Year { get; set; }

        public string? CitationPages { get; set; }

        public string? CitationDOI { get; set; }
    }

    public class PublicationDTO : CreatePublicationDTO
    {
        public Guid Id { get; set; }
    }
}