using Plexus.Client.ViewModel.Research;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO.Research;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Research
{
    public class ResearchCategoryManager : IResearchCategoryManager
    {
        private readonly IResearchCategoryProvider _researchCategoryProvider;

        public ResearchCategoryManager(IResearchCategoryProvider researchCategoryProvider)
        {
            _researchCategoryProvider = researchCategoryProvider;
        }

        public ResearchCategoryViewModel Create(CreateResearchCategoryViewModel request, Guid userId)
        {
            var dto = new CreateResearchCategoryDTO
            {
                Name = request.Name,
                IsActive = request.IsActive,
                Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList()
            };

            var researchCategory = _researchCategoryProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(researchCategory);

            return response;
        }


        public PagedViewModel<ResearchCategoryViewModel> Search(SearchResearchCategoryCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var dto = new SearchResearchCategoryCriteriaDTO
            {
                Keyword = parameters.Keyword,
                IsActive = parameters.IsActive,
                SortBy = parameters.SortBy,
                OrderBy = parameters.OrderBy
            };

            var pagedResearchCategory = _researchCategoryProvider.Search(dto, page, pageSize);

            var response = new PagedViewModel<ResearchCategoryViewModel>
            {
                Page = pagedResearchCategory.Page,
                TotalPage = pagedResearchCategory.TotalPage,
                TotalItem = pagedResearchCategory.TotalItem,
                Items = (from researchCategory in pagedResearchCategory.Items
                         select MapDTOToViewModel(researchCategory))
                         .ToList()
            };

            return response;
        }

        public ResearchCategoryViewModel GetById(Guid id)
        {
            var researchCategory = _researchCategoryProvider.GetById(id);

            var response = MapDTOToViewModel(researchCategory);

            return response;
        }

        public ResearchCategoryViewModel Update(Guid id, CreateResearchCategoryViewModel request, Guid userId)
        {
            var researchCategory = _researchCategoryProvider.GetById(id);

            researchCategory.Name = request.Name;
            researchCategory.IsActive = request.IsActive;
            researchCategory.Localizations = MapLocalizationViewModelToDTO(request.Localizations);

            var updatedResearchCategory = _researchCategoryProvider.Update(researchCategory, userId.ToString());

            var response = MapDTOToViewModel(updatedResearchCategory);

            return response;
        }

        public void Delete(Guid id)
        {
            _researchCategoryProvider.Delete(id);
        }

        private static ResearchCategoryViewModel MapDTOToViewModel(ResearchCategoryDTO dto)
        {
            var response = new ResearchCategoryViewModel
            {
                Id = dto.Id,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                Localizations = (from data in dto.Localizations
                                 orderby data.Language
                                 select new ResearchCategoryLocalizationViewModel
                                 {
                                     Language = data.Language,
                                     Name = data.Name
                                 })
                                .ToList()
            };
            return response;
        }
        private static IEnumerable<ResearchCategoryLocalizationDTO> MapLocalizationViewModelToDTO(
                IEnumerable<ResearchCategoryLocalizationViewModel>? localizations)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<ResearchCategoryLocalizationDTO>();
            }
            var response = (from locale in localizations
                            select new ResearchCategoryLocalizationDTO
                            {
                                Language = locale.Language,
                                Name = locale.Name
                            })
                           .ToList();
            return response;
        }
    }
}