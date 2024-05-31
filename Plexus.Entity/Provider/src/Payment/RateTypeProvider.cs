using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Localization.Payment;
using Plexus.Database.Model.Payment;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src.Payment
{
    public class RateTypeProvider : IRateTypeProvider
    {
        private readonly DatabaseContext _dbContext;

        public RateTypeProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public RateTypeDTO Create(CreateRateTypeDTO request, string requester)
        {
            var model = new RateType
            {
                Name = request.Name,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizations = request.Localizations is null ? Enumerable.Empty<RateTypeLocalization>()
                                                              : (from localize in request.Localizations
                                                                 select new RateTypeLocalization
                                                                 {
                                                                    RateType = model,
                                                                    Language = localize.Language,
                                                                    Name = localize.Name
                                                                 })
                                                                .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.RateTypes.Add(model);

                if (localizations.Any())
                {
                    _dbContext.RateTypeLocalizations.AddRange(localizations);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizations);

            return response;
        }

        public PagedViewModel<RateTypeDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedRateType = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<RateTypeDTO>
            {
                Page = pagedRateType.Page,
                TotalPage = pagedRateType.TotalPage,
                TotalItem = pagedRateType.TotalItem,
                Items = (from rateType in pagedRateType.Items
                         select MapModelToDTO(rateType, rateType.Localizations))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<RateTypeDTO> Search(SearchCriteriaViewModel parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var rateTypes = query.ToList();

            var response = (from rateType in rateTypes
                            select MapModelToDTO(rateType, rateType.Localizations))
                           .ToList();
            
            return response;
        }

        public RateTypeDTO GetById(Guid id)
        {
            var rateType = _dbContext.RateTypes.Include(x => x.Localizations)
                                               .AsNoTracking()
                                               .SingleOrDefault(x => x.Id == id);

            if (rateType is null)
            {
                throw new RateTypeException.NotFound(id);
            }

            var response = MapModelToDTO(rateType, rateType.Localizations);

            return response;
        }

        public IEnumerable<RateTypeDTO> GetById(IEnumerable<Guid> ids)
        {
            var rateTypes = _dbContext.RateTypes.Include(x => x.Localizations)
                                                .AsNoTracking()
                                                .Where(x => ids.Contains(x.Id))
                                                .ToList();
            
            var response = (from rateType in rateTypes
                            select MapModelToDTO(rateType, rateType.Localizations))
                           .ToList();
            
            return response;
        }

        public RateTypeDTO Update(RateTypeDTO request, string requester)
        {
            var rateType = _dbContext.RateTypes.Include(x => x.Localizations)
                                               .SingleOrDefault(x => x.Id == request.Id);

            if (rateType is null)
            {
                throw new RateTypeException.NotFound(request.Id);
            }

            var localizations = request.Localizations is null ? Enumerable.Empty<RateTypeLocalization>()
                                                              : (from localize in request.Localizations
                                                                 select new RateTypeLocalization
                                                                 {
                                                                     RateTypeId = rateType.Id,
                                                                     Language = localize.Language,
                                                                     Name = localize.Name
                                                                 })
                                                                .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                rateType.Name = request.Name;
                rateType.IsActive = request.IsActive;
                rateType.UpdatedAt = DateTime.UtcNow;
                rateType.UpdatedBy = requester;

                _dbContext.RateTypeLocalizations.RemoveRange(rateType.Localizations);

                if (localizations.Any())
                {
                    _dbContext.RateTypeLocalizations.AddRange(localizations);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(rateType, localizations);

            return response;
        }

        public void Delete(Guid id)
        {
            var rateType = _dbContext.RateTypes.SingleOrDefault(x => x.Id == id);

            if (rateType is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.RateTypes.Remove(rateType);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static RateTypeDTO MapModelToDTO(RateType model, IEnumerable<RateTypeLocalization> localizations)
        {
            var response = new RateTypeDTO
            {
                Id = model.Id,
                Name = model.Name,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<RateTypeLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new RateTypeLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name
                                                         })
                                                        .ToList()
            };

            return response;
        }

        private IQueryable<RateType> GenerateSearchQuery(SearchCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.RateTypes.Include(x => x.Localizations)
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
    }
}