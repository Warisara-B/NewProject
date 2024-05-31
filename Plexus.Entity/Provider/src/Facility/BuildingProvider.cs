using System;
using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Facility;
using Plexus.Database.Model.Localization.Facility;
using Plexus.Entity.DTO.Facility;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;
using ServiceStack;

namespace Plexus.Entity.Provider.src.Facility
{
    public class BuildingProvider : IBuildingProvider
    {
        private readonly DatabaseContext _dbContext;

        public BuildingProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public BuildingDTO Create(CreateBuildingDTO request, string requester)
        {
            var model = new Building
            {
                Name = request.Name,
                Code = request.Code,
                CampusId = request.CampusId,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Buildings.Add(model);

                if (localizes.Any())
                {
                    _dbContext.BuildingLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizes);

            return response;
        }

        public IEnumerable<BuildingDTO> GetAll()
        {
            var buildings = _dbContext.Buildings.AsNoTracking()
                                                .Include(x => x.Localizations)
                                                .ToList();

            var response = (from building in buildings
                            orderby building.Code
                            select MapModelToDTO(building, building.Localizations))
                           .ToList();

            return response;
        }

        public IEnumerable<BuildingDTO> Search(SearchBuildingCriteriaDTO parameters)
        {
            var query = GenerateSeachQuery(parameters);

            var buildings = query.ToList();

            var response = (from building in buildings
                            select MapModelToDTO(building, building.Localizations))
                            .ToList();

            return response;
        }

        public PagedViewModel<BuildingDTO> Search(SearchBuildingCriteriaDTO parameters, int page, int pageSize)
        {
            var query = GenerateSeachQuery(parameters);

            var pagedBuilding = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<BuildingDTO>
            {
                Page = pagedBuilding.Page,
                TotalPage = pagedBuilding.TotalPage,
                TotalItem = pagedBuilding.TotalItem,
                Items = (from building in pagedBuilding.Items
                         select MapModelToDTO(building, building.Localizations))
                        .ToList()
            };

            return response;
        }

        public BuildingDTO GetById(Guid id)
        {
            var building = _dbContext.Buildings.AsNoTracking()
                                               .Include(x => x.Localizations)
                                               .SingleOrDefault(x => x.Id == id);

            if (building is null)
            {
                throw new BuildingException.NotFound(id);
            }

            var response = MapModelToDTO(building, building.Localizations);

            return response;
        }

        public BuildingDTO Update(BuildingDTO request, string requester)
        {
            var building = _dbContext.Buildings.Include(x => x.Localizations)
                                               .SingleOrDefault(x => x.Id == request.Id);

            if (building is null)
            {
                throw new BuildingException.NotFound(request.Id);
            }

            var localizes = MapLocalizationDTOToModel(request.Localizations, building).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                building.Name = request.Name;
                building.Code = request.Code;
                building.CampusId = request.CampusId;
                building.IsActive = request.IsActive;
                building.UpdatedAt = DateTime.UtcNow;
                building.UpdatedBy = requester;

                _dbContext.BuildingLocalizations.RemoveRange(building.Localizations);

                if (localizes.Any())
                {
                    _dbContext.BuildingLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(building, localizes);

            return response;
        }

        public void Delete(Guid id)
        {
            var building = _dbContext.Buildings.SingleOrDefault(x => x.Id == id);

            if (building is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Buildings.Remove(building);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public IEnumerable<BuildingAvailableTimeDTO> GetAvailableTime(Guid buildingId)
        {
            var avialableTimes = _dbContext.BuildingAvailableTimes.AsNoTracking()
                                                                  .Where(x => x.BuildingId == buildingId)
                                                                  .ToList();

            var response = MapAvailableTimeToDTO(avialableTimes);

            return response;
        }

        public IEnumerable<BuildingAvailableTimeDTO> UpdateAvailableTime(Guid buildingId, IEnumerable<BuildingAvailableTimeDTO> requests)
        {
            var availableTimes = _dbContext.BuildingAvailableTimes.Where(x => x.BuildingId == buildingId)
                                                                  .ToList();

            var newTimes = requests is null ? Enumerable.Empty<BuildingAvailableTime>()
                                            : (from data in requests
                                               orderby data.Day
                                               select new BuildingAvailableTime
                                               {
                                                   Day = data.Day,
                                                   BuildingId = buildingId,
                                                   FromTime = data.FromTime,
                                                   ToTime = data.ToTime
                                               }).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (availableTimes.Any())
                {
                    _dbContext.BuildingAvailableTimes.RemoveRange(availableTimes);
                }

                if (newTimes.Any())
                {
                    _dbContext.BuildingAvailableTimes.AddRange(newTimes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapAvailableTimeToDTO(newTimes);

            return response;
        }

        private static BuildingDTO MapModelToDTO(Building model, IEnumerable<BuildingLocalization> localizations)
        {
            var response = new BuildingDTO
            {
                Id = model.Id,
                Name = model.Name,
                Code = model.Code,
                CampusId = model.CampusId,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<BuildingLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new BuildingLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name
                                                         })
                                                        .ToList()
            };

            return response;
        }

        private IQueryable<Building> GenerateSeachQuery(SearchBuildingCriteriaDTO? parameters)
        {
            var query = _dbContext.Buildings.Include(x => x.Localizations)
                                            .AsNoTracking();

            if (parameters is not null)
            {
                if (parameters.CampusId.HasValue)
                {
                    query = query.Where(x => x.CampusId == parameters.CampusId.Value);
                }

                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.Code.Contains(parameters.Keyword) || x.Name.Contains(parameters.Keyword));
                }

                if (parameters.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == parameters.IsActive.Value);
                }
            }

            query = query.OrderBy(x => x.Code);

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.SortBy))
                {
                    try
                    {
                        query = query.OrderBy(parameters.SortBy, parameters.OrderBy);
                    }
                    catch (System.Exception)
                    {
                        // invalid property name
                    }
                }
            }

            return query;
        }

        private static IEnumerable<BuildingAvailableTimeDTO> MapAvailableTimeToDTO(IEnumerable<BuildingAvailableTime> models)
        {
            var response = (from day in Enum.GetValues<DayOfWeek>()
                            orderby day
                            let matchingDay = models.OrderBy(x => x.FromTime)
                                                    .FirstOrDefault(x => x.Day == day)
                            select new BuildingAvailableTimeDTO
                            {
                                Day = day,
                                FromTime = matchingDay is null ? null : matchingDay.FromTime,
                                ToTime = matchingDay is null ? null : matchingDay.ToTime
                            })
                           .ToList();

            return response;
        }

        private static IEnumerable<BuildingLocalization> MapLocalizationDTOToModel(
            IEnumerable<BuildingLocalizationDTO>? localizations,
            Building model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<BuildingLocalization>();
            }

            var response = (from locale in localizations
                            select new BuildingLocalization
                            {
                                Building = model,
                                Language = locale.Language,
                                Name = locale.Name
                            })
                            .ToList();

            return response;
        }
    }
}

