using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Localization.Payment;
using Plexus.Database.Model.Payment;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Payment;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;

namespace Plexus.Entity.Provider.src.Payment
{
    public class FeeItemProvider : IFeeItemProvider
    {
        private readonly DatabaseContext _dbContext;

        public FeeItemProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public FeeItemDTO GetById(Guid id)
        {
            var feeItem = _dbContext.FeeItems.Include(x => x.Localizations)
                                             .AsNoTracking()
                                             .SingleOrDefault(x => x.Id == id);

            if (feeItem is null)
            {
                throw new FeeItemException.NotFound(id);
            }

            var response = MapModelToDTO(feeItem, feeItem.Localizations);

            return response;
        }

        public IEnumerable<FeeItemDTO> GetById(IEnumerable<Guid> ids)
        {
            var feeItems = _dbContext.FeeItems.Include(x => x.Localizations)
                                              .AsNoTracking()
                                              .Where(x => ids.Contains(x.Id))
                                              .ToList();

            var response = (from feeItem in feeItems
                            select MapModelToDTO(feeItem, feeItem.Localizations))
                           .ToList();

            return response;
        }

        private static FeeItemDTO MapModelToDTO(FeeItem model, IEnumerable<FeeItemLocalization> localizations)
        {
            return new FeeItemDTO
            {
                Id = model.Id,
                FeeGroupId = model.FeeGroupId,
                Code = model.Code,
                Name = model.Name,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                IsActive = model.IsActive,
                Localizations = localizations is null ? Enumerable.Empty<FeeItemLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new FeeItemLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name
                                                         })
                                                        .ToList()
            };
        }

        private static IEnumerable<FeeItemLocalization> MapLocalizationDTOToModel(
                IEnumerable<FeeItemLocalizationDTO> localizations,
                FeeItem model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<FeeItemLocalization>();
            }

            var response = (from locale in localizations
                            select new FeeItemLocalization
                            {
                                FeeItem = model,
                                Language = locale.Language,
                                Name = locale.Name
                            })
                           .ToList();

            return response;
        }

        private IQueryable<FeeItem> GenerateSearchQuery(SearchCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.FeeItems.Include(x => x.Localizations)
                                           .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.Code.Contains(parameters.Keyword)
                                             || x.Name.Contains(parameters.Keyword));
                }

                if (parameters.FeeGroupId.HasValue)
                {
                    query = query.Where(x => x.FeeGroupId == parameters.FeeGroupId.Value);
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
    }
}