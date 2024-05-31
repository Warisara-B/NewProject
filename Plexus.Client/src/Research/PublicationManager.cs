using Plexus.Client.ViewModel.Research;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Research
{
    public class PublicationManager : IPublicationManager
    {
        private readonly IPublicationProvider _publicationProvider;

        private readonly IArticleTypeProvider _articleTypeProvider;

        public PublicationManager(IPublicationProvider publicationProvider,
                                  IArticleTypeProvider articleTypeProvider)
        {
            _publicationProvider = publicationProvider;
            _articleTypeProvider = articleTypeProvider;
        }

        public PublicationViewModel Create(CreatePublicationViewModel request, Guid userId)
        {
            var articleType = _articleTypeProvider.GetById(request.ArticleTypeId);

            var dto = new CreatePublicationDTO
            {
                ArticleTypeId = request.ArticleTypeId,
                Authors = request.Authors,
                Pages = request.Pages,
                Year = request.Year,
                CitationPages = request.CitationPages,
                CitationDOI = request.CitationDOI
            };

            var publication = _publicationProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(publication);

            return response;
        }

        public PagedViewModel<PublicationViewModel> Search(SearchPublicationCriteriaViewModel criteria, int page = 1, int pageSize = 25)
        {
            var dto = new SearchPublicationCriteriaDTO
            {
                Keyword = criteria.Keyword,
                ArticleTypeId = criteria.ArticleTypeId,
                Year = criteria.Year,
                SortBy = criteria.SortBy,
                OrderBy = criteria.OrderBy
            };

            var pagedPublication = _publicationProvider.Search(dto, page, pageSize);

            var response = new PagedViewModel<PublicationViewModel>
            {
                Page = pagedPublication.Page,
                TotalPage = pagedPublication.TotalPage,
                TotalItem = pagedPublication.TotalItem,
                Items = (from publication in pagedPublication.Items
                         select MapDTOToViewModel(publication))
                        .ToList()
            };

            return response;
        }

        public PublicationViewModel GetById(Guid id)
        {
            var publication = _publicationProvider.GetById(id);

            var response = MapDTOToViewModel(publication);

            return response;
        }

        public PublicationViewModel Update(Guid id, CreatePublicationViewModel request, Guid userId)
        {
            var articleType = _articleTypeProvider.GetById(request.ArticleTypeId);

            var publications = _publicationProvider.GetByArticleTypeId(request.ArticleTypeId);

            var publication = publications.SingleOrDefault(x => x.Id == id);

            if (publication is null)
            {
                throw new PublicationException.NotFound(id);
            }

            publication.ArticleTypeId = request.ArticleTypeId;
            publication.Authors = request.Authors;
            publication.Pages = request.Pages;
            publication.Year = request.Year;
            publication.CitationPages = request.CitationPages;
            publication.CitationDOI = request.CitationDOI;

            var updatedPublication = _publicationProvider.Update(publication, userId.ToString());

            var response = MapDTOToViewModel(updatedPublication);

            return response;
        }

        public void Delete(Guid id)
        {
            _publicationProvider.Delete(id);
        }

        private static PublicationViewModel MapDTOToViewModel(PublicationDTO dto)
        {
            var response = new PublicationViewModel
            {
                Id = dto.Id,
                ArticleTypeId = dto.ArticleTypeId,
                Authors = dto.Authors,
                Pages = dto.Pages,
                Year = dto.Year,
                CitationPages = dto.CitationPages,
                CitationDOI = dto.CitationDOI
            };

            return response;
        }
    }
}