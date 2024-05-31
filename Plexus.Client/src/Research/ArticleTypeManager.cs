using Azure;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Research;
using Plexus.Entity.DTO.Research;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Research
{
    public class ArticleTypeManager : IArticleTypeManager
    {
        private readonly IArticleTypeProvider _articleTypeProvider;

        public ArticleTypeManager(IArticleTypeProvider articleTypeProvider)
        {
            _articleTypeProvider = articleTypeProvider;
        }

        public ArticleTypeViewModel Create(CreateArticleTypeViewModel request, Guid userId)
        {
            var dto = new CreateArticleTypeDTO
            {
                Name = request.Name,
                IsActive = request.IsActive,
                Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList()
            };

            var articleType = _articleTypeProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(articleType);

            return response;
        }

        public PagedViewModel<ArticleTypeViewModel> Search(SearchArticleTypeCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var dto = new SearchArticleTypeCriteriaDTO
            {
                Keyword = parameters.Keyword,
                IsActive = parameters.IsActive,
                SortBy = parameters.SortBy,
                OrderBy = parameters.OrderBy
            };

            var pagedArticleType = _articleTypeProvider.Search(dto, page, pageSize);

            var response = new PagedViewModel<ArticleTypeViewModel>
            {
                Page = pagedArticleType.Page,
                TotalPage = pagedArticleType.TotalPage,
                TotalItem = pagedArticleType.TotalItem,
                Items = (from articleType in pagedArticleType.Items
                         select MapDTOToViewModel(articleType))
                         .ToList()
            };

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchArticleTypeCriteriaViewModel parameters)
        {
            var dto = new SearchArticleTypeCriteriaDTO
            {
                Keyword = parameters.Keyword,
                IsActive = parameters.IsActive,
                SortBy = parameters.SortBy,
                OrderBy = parameters.OrderBy
            };

            var articleTypes = _articleTypeProvider.Search(dto);

            var response = (from articleType in articleTypes
                            select MapDTOToDropDown(articleType))
                            .ToList();

            return response;
        }

        public ArticleTypeViewModel GetById(Guid id)
        {
            var articleType = _articleTypeProvider.GetById(id);

            var response = MapDTOToViewModel(articleType);

            return response;
        }

        public ArticleTypeViewModel Update(Guid id, CreateArticleTypeViewModel request, Guid userId)
        {
            var articleType = _articleTypeProvider.GetById(id);

            articleType.Name = request.Name;
            articleType.IsActive = request.IsActive;
            articleType.Localizations = MapLocalizationViewModelToDTO(request.Localizations);

            var updatedArticleType = _articleTypeProvider.Update(articleType, userId.ToString());

            var response = MapDTOToViewModel(updatedArticleType);

            return response;
        }

        public void Delete(Guid id)
        {
            _articleTypeProvider.Delete(id);
        }

        private static ArticleTypeViewModel MapDTOToViewModel(ArticleTypeDTO dto)
        {
            var response = new ArticleTypeViewModel
            {
                Id = dto.Id,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                Localizations = (from data in dto.Localizations
                                 orderby data.Language
                                 select new ArticleTypeLocalizationViewModel
                                 {
                                     Language = data.Language,
                                     Name = data.Name
                                 })
                                .ToList()
            };

            return response;
        }

        private static BaseDropDownViewModel MapDTOToDropDown(ArticleTypeDTO dto)
        {
            var response = new BaseDropDownViewModel
            {
                Id = dto.Id.ToString(),
                Name = dto.Name
            };

            return response;
        }

        private static IEnumerable<ArticleTypeLocalizationDTO> MapLocalizationViewModelToDTO(
                IEnumerable<ArticleTypeLocalizationViewModel>? localizations)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<ArticleTypeLocalizationDTO>();
            }

            var response = (from locale in localizations
                            select new ArticleTypeLocalizationDTO
                            {
                                Language = locale.Language,
                                Name = locale.Name
                            })
                           .ToList();

            return response;
        }
    }
}