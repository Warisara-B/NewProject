using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.Facility;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Facility;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Facility
{
    public class BuildingManager : IBuildingManager
    {
        private IBuildingProvider _buildingProvider;
        private ICampusProvider _campusProvider;

        public BuildingManager(IBuildingProvider buildingProvider,
                               ICampusProvider campusProvider)
        {
            _buildingProvider = buildingProvider;
            _campusProvider = campusProvider;
        }

        public BuildingViewModel Create(CreateBuildingViewModel request, Guid userId)
        {
            var buildings = _buildingProvider.GetAll()
                                             .ToList();

            if (buildings.Any(x => string.Equals(x.Code, request.Code, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new BuildingException.Duplicate(request.Code);
            }

            var campus = _campusProvider.GetById(request.CampusId);

            var dto = new CreateBuildingDTO
            {
                Name = request.Name,
                Code = request.Code,
                CampusId = request.CampusId,
                IsActive = request.IsActive,
                Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList()
            };

            var building = _buildingProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(building, campus);

            return response;
        }

        public PagedViewModel<BuildingViewModel> Search(SearchBuildingCriteriaViewModel parameters, int page, int pageSize)
        {
            var dto = new SearchBuildingCriteriaDTO
            {
                Keyword = parameters.Keyword,
                CampusId = parameters.CampusId,
                IsActive = parameters.IsActive,
                SortBy = parameters.SortBy,
                OrderBy = parameters.OrderBy
            };

            var pagedBuilding = _buildingProvider.Search(dto, page, pageSize);

            var campuses = _campusProvider.GetAll()
                                          .ToList();

            var response = new PagedViewModel<BuildingViewModel>
            {
                Page = pagedBuilding.Page,
                TotalPage = pagedBuilding.TotalPage,
                TotalItem = pagedBuilding.TotalItem,
                Items = (from building in pagedBuilding.Items
                         let campus = campuses.SingleOrDefault(x => x.Id == building.CampusId)
                         select MapDTOToViewModel(building, campus))
                        .ToList()
            };

            return response;
        }

        public BuildingViewModel GetById(Guid id)
        {
            var building = _buildingProvider.GetById(id);

            var campus = _campusProvider.GetById(building.CampusId);

            var response = MapDTOToViewModel(building, campus);

            return response;
        }

        public BuildingViewModel Update(BuildingViewModel request, Guid userId)
        {
            var buildings = _buildingProvider.GetAll()
                                             .ToList();

            if (buildings.Any(x => x.Id != request.Id
                                   && string.Equals(x.Code, request.Code, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new BuildingException.Duplicate(request.Code);
            }

            var building = buildings.SingleOrDefault(x => x.Id == request.Id);

            if (building is null)
            {
                throw new BuildingException.NotFound(request.Id);
            }

            var campus = _campusProvider.GetById(request.CampusId);

            building.Name = request.Name;
            building.Code = request.Code;
            building.CampusId = request.CampusId;
            building.IsActive = request.IsActive;
            building.Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList();

            var updatedBuilding = _buildingProvider.Update(building, userId.ToString());

            var response = MapDTOToViewModel(building, campus);

            return response;
        }

        public void Delete(Guid id)
        {
            _buildingProvider.Delete(id);
        }

        public IEnumerable<BuildingAvailableTimeViewModel> GetAvailableTimes(Guid id)
        {
            var building = _buildingProvider.GetById(id);

            var availableTimes = _buildingProvider.GetAvailableTime(id)
                                                  .ToList();

            var response = (from time in availableTimes
                            select MapAvailableTimeDTOToViewModel(time))
                           .ToList();

            return response;
        }

        public IEnumerable<BuildingAvailableTimeViewModel> UpdateAvailableTimes(Guid buildingId, IEnumerable<BuildingAvailableTimeViewModel> requests)
        {
            var building = _buildingProvider.GetById(buildingId);

            var times = requests is null ? Enumerable.Empty<BuildingAvailableTimeDTO>()
                                         : (from time in requests
                                            select new BuildingAvailableTimeDTO
                                            {
                                                Day = time.Day,
                                                FromTime = time.FromTime.HasValue ? time.FromTime.Value.ToTimeSpan()
                                                                                  : null,
                                                ToTime = time.ToTime.HasValue ? time.ToTime.Value.ToTimeSpan()
                                                                              : null
                                            })
                                           .ToList();

            foreach (var time in times)
            {
                if (time.FromTime.HasValue && time.ToTime.HasValue
                   && time.FromTime.Value > time.ToTime.Value)
                {
                    throw new BuildingException.InvalidAvailableTime(TimeOnly.FromTimeSpan(time.FromTime.Value), TimeOnly.FromTimeSpan(time.ToTime.Value));
                }
            }

            var updateAvailableTimes = _buildingProvider.UpdateAvailableTime(buildingId, times)
                                                        .ToList();

            var response = (from time in updateAvailableTimes
                            select MapAvailableTimeDTOToViewModel(time))
                           .ToList();

            return response;
        }

        public IEnumerable<BuildingDropDownViewModel> GetDropDownList(SearchBuildingCriteriaViewModel parameters)
        {
            var dto = new SearchBuildingCriteriaDTO
            {
                Keyword = parameters.Keyword,
                CampusId = parameters.CampusId,
                IsActive = parameters.IsActive,
                SortBy = parameters.SortBy,
                OrderBy = parameters.OrderBy
            };

            var buildings = _buildingProvider.Search(dto)
                                             .ToList();

            var response = (from building in buildings
                            orderby building.Code, building.Name
                            select new BuildingDropDownViewModel
                            {
                                Id = building.Id.ToString(),
                                Name = building.Name,
                                CampusId = building.CampusId
                            })
                           .ToList();

            return response;
        }

        public static BuildingViewModel MapDTOToViewModel(BuildingDTO dto, CampusDTO? campus = null)
        {
            var response = new BuildingViewModel
            {
                Id = dto.Id,
                Code = dto.Code,
                CampusId = dto.CampusId,
                CampusName = campus?.Name,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                IsActive = dto.IsActive,
                Localizations = (from localize in dto.Localizations
                                 orderby localize.Language
                                 select new BuildingLocalizationViewModel
                                 {
                                     Language = localize.Language,
                                     Name = localize.Name
                                 })
                                .ToList()
            };

            return response;
        }

        private static BuildingAvailableTimeViewModel MapAvailableTimeDTOToViewModel(BuildingAvailableTimeDTO dto)
        {
            var response = new BuildingAvailableTimeViewModel
            {
                Day = dto.Day,
                FromTime = dto.FromTime.HasValue ? TimeOnly.FromTimeSpan(dto.FromTime.Value)
                                                 : null,
                ToTime = dto.ToTime.HasValue ? TimeOnly.FromTimeSpan(dto.ToTime.Value)
                                             : null
            };

            return response;
        }

        private static IEnumerable<BuildingLocalizationDTO> MapLocalizationViewModelToDTO(
            IEnumerable<BuildingLocalizationViewModel>? localizations)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<BuildingLocalizationDTO>();
            }

            var response = (from locale in localizations
                            select new BuildingLocalizationDTO
                            {
                                Language = locale.Language,
                                Name = locale.Name
                            })
                            .ToList();

            return response;
        }
    }
}

