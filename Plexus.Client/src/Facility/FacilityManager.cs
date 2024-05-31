using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Facility;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Facility;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Facility
{
    public class FacilityManager : IFacilityManager
    {
        private readonly IFacilityProvider _facilityProvider;

        public FacilityManager(IFacilityProvider facilityProvider)
        {
            _facilityProvider = facilityProvider;
        }

        public FacilityViewModel Create(CreateFacilityViewModel request, Guid userId)
        {
            var dto = new CreateFacilityDTO
            {
                Name = request.Name,
                Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList()
            };

            var facility = _facilityProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(facility);

            return response;
        }

        public PagedViewModel<FacilityViewModel> Search(SearchFacilityCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var dto = new SearchFacilityCriteriaDTO
            {
                Keyword = parameters.Keyword,
                SortBy = parameters.SortBy,
                OrderBy = parameters.OrderBy
            };

            var pagedFacility = _facilityProvider.Search(dto, page, pageSize);

            var response = new PagedViewModel<FacilityViewModel>
            {
                Page = pagedFacility.Page,
                TotalPage = pagedFacility.TotalPage,
                TotalItem = pagedFacility.TotalItem,
                Items = (from facility in pagedFacility.Items
                         select MapDTOToViewModel(facility))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropDownList(SearchFacilityCriteriaViewModel parameters)
        {
            var dto = new SearchFacilityCriteriaDTO
            {
                Keyword = parameters.Keyword,
                SortBy = parameters.SortBy,
                OrderBy = parameters.OrderBy
            };

            var facilities = _facilityProvider.Search(dto)
                                              .ToList();

            var response = (from facility in facilities
                            select new BaseDropDownViewModel
                            {
                                Id = facility.Id.ToString(),
                                Name = facility.Name
                            })
                           .ToList();

            return response;
        }

        public FacilityViewModel GetById(Guid id)
        {
            var facility = _facilityProvider.GetById(id);

            var response = MapDTOToViewModel(facility);

            return response;
        }

        public FacilityViewModel Update(Guid id, CreateFacilityViewModel request, Guid userId)
        {
            var facility = _facilityProvider.GetById(id);

            facility.Name = request.Name;
            facility.Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList();

            var updatedFacility = _facilityProvider.Update(facility, userId.ToString());

            var response = MapDTOToViewModel(updatedFacility);

            return response;
        }

        public void Delete(Guid id)
        {
            _facilityProvider.Delete(id);
        }

        private static FacilityViewModel MapDTOToViewModel(FacilityDTO dto)
        {
            var response = new FacilityViewModel
            {
                Id = dto.Id,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                Localizations = (from locale in dto.Localizations
                                 orderby locale.Language
                                 select new FacilityLocalizationViewModel
                                 {
                                     Language = locale.Language,
                                     Name = locale.Name
                                 })
                                .ToList()
            };

            return response;
        }

        private static IEnumerable<FacilityLocalizationDTO> MapLocalizationViewModelToDTO(
            IEnumerable<FacilityLocalizationViewModel>? localizations)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<FacilityLocalizationDTO>();
            }

            var response = (from locale in localizations
                            select new FacilityLocalizationDTO
                            {
                                Language = locale.Language,
                                Name = locale.Name
                            })
                           .ToList();

            return response;
        }
    }
}