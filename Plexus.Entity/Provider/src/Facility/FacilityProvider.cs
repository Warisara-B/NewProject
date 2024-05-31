using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Facility;
using Plexus.Utility.ViewModel;
using FacilityModel = Plexus.Database.Model.Facility.Facility;
using Plexus.Utility.Extensions;
using Plexus.Database.Model.Localization.Facility;
using Plexus.Entity.Exception;
using Plexus.Entity.DTO.SearchFilter;

namespace Plexus.Entity.Provider.src.Facility
{
    public class FacilityProvider : IFacilityProvider
    {
        private readonly DatabaseContext _dbContext;

        public FacilityProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public FacilityDTO Create(CreateFacilityDTO request, string requester)
        {
            var model = new FacilityModel
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Facilities.Add(model);

                if (localizes.Any())
                {
                    _dbContext.FacilityLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizes);

            return response;
        }

        public PagedViewModel<FacilityDTO> Search(SearchFacilityCriteriaDTO parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedFacility = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<FacilityDTO>
            {
                Page = pagedFacility.Page,
                TotalPage = pagedFacility.TotalPage,
                TotalItem = pagedFacility.TotalItem,
                Items = (from facility in pagedFacility.Items
                         select MapModelToDTO(facility, facility.Localizations))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<FacilityDTO> Search(SearchFacilityCriteriaDTO parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var facilities = query.ToList();

            var response = (from facility in facilities
                            select MapModelToDTO(facility, facility.Localizations))
                           .ToList();

            return response;
        }

        public IEnumerable<FacilityDTO> GetById(IEnumerable<Guid> ids)
        {
            var facilities = _dbContext.Facilities.Include(x => x.Localizations)
                                                  .AsNoTracking()
                                                  .Where(x => ids.Contains(x.Id))
                                                  .ToList();

            var response = (from facility in facilities
                            select MapModelToDTO(facility, facility.Localizations))
                           .ToList();

            return response;
        }

        public FacilityDTO GetById(Guid id)
        {
            var facility = _dbContext.Facilities.Include(x => x.Localizations)
                                                .AsNoTracking()
                                                .SingleOrDefault(x => x.Id == id);

            if (facility is null)
            {
                throw new FacilityException.NotFound(id);
            }

            var response = MapModelToDTO(facility, facility.Localizations);

            return response;
        }

        public FacilityDTO Update(FacilityDTO request, string requester)
        {
            var facility = _dbContext.Facilities.Include(x => x.Localizations)
                                                .SingleOrDefault(x => x.Id == request.Id);

            if (facility is null)
            {
                throw new FacilityException.NotFound(request.Id);
            }

            var localizes = MapLocalizationDTOToModel(request.Localizations, facility).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                facility.Name = request.Name;
                facility.UpdatedAt = DateTime.UtcNow;
                facility.UpdatedBy = requester;

                _dbContext.FacilityLocalizations.RemoveRange(facility.Localizations);

                if (localizes.Any())
                {
                    _dbContext.FacilityLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(facility, localizes);

            return response;
        }

        public void Delete(Guid id)
        {
            var facility = _dbContext.Facilities.SingleOrDefault(x => x.Id == id);

            if (facility is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Facilities.Remove(facility);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private IQueryable<FacilityModel> GenerateSearchQuery(SearchFacilityCriteriaDTO? parameters)
        {
            var query = _dbContext.Facilities.Include(x => x.Localizations)
                                             .AsNoTracking();

            if (parameters != null)
            {
                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Keyword));
                }
            }

            query = query.OrderBy(x => x.Name);

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

        private static FacilityDTO MapModelToDTO(FacilityModel model, IEnumerable<FacilityLocalization> localizations)
        {
            return new FacilityDTO
            {
                Id = model.Id,
                Name = model.Name,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<FacilityLocalizationDTO>()
                                                      : (from localize in localizations
                                                         select new FacilityLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name
                                                         })
                                                        .ToList()
            };
        }

        private static IEnumerable<FacilityLocalization> MapLocalizationDTOToModel(
            IEnumerable<FacilityLocalizationDTO>? localizations,
            FacilityModel model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<FacilityLocalization>();
            }

            var response = (from locale in localizations
                            select new FacilityLocalization
                            {
                                Facility = model,
                                Language = locale.Language,
                                Name = locale.Name
                            })
                           .ToList();

            return response;
        }
    }
}