using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Localization.Academic;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;

namespace Plexus.Entity.Provider.src.Academic
{
    public class AcademicLevelProvider : IAcademicLevelProvider
    {
        private readonly DatabaseContext _dbContext;

        public AcademicLevelProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AcademicLevelDTO Create(CreateAcademicLevelDTO request, string requester)
        {
            var model = new AcademicLevel
            {
                Name = request.Name,
                FormalName = request.FormalName,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester,
                IsActive = request.IsActive
            };

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.AcademicLevels.Add(model);

                if (localizes.Any())
                {
                    _dbContext.AcademicLevelLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizes);

            return response;
        }

        public IEnumerable<AcademicLevelDTO> GetAll()
        {
            var academicLevels = _dbContext.AcademicLevels.AsNoTracking()
                                                          .Include(x => x.Localizations)
                                                          .ToList();

            var response = (from academicLevel in academicLevels
                            select MapModelToDTO(academicLevel, academicLevel.Localizations))
                           .ToList();

            return response;
        }

        public AcademicLevelDTO GetById(Guid id)
        {
            var academicLevel = _dbContext.AcademicLevels.AsNoTracking()
                                                         .Include(x => x.Localizations)
                                                         .SingleOrDefault(x => x.Id == id);

            if (academicLevel is null)
            {
                throw new AcademicLevelException.NotFound(id);
            }

            var response = MapModelToDTO(academicLevel, academicLevel.Localizations);

            return response;
        }

        public IEnumerable<AcademicLevelDTO> GetById(IEnumerable<Guid> ids)
        {
            var academicLevels = _dbContext.AcademicLevels.AsNoTracking()
                                                          .Include(x => x.Localizations)
                                                          .Where(x => ids.Contains(x.Id))
                                                          .ToList();

            var response = (from academicLevel in academicLevels
                            select MapModelToDTO(academicLevel, academicLevel.Localizations))
                           .ToList();

            return response;
        }
        public IEnumerable<AcademicLevelDTO> Search(SearchAcademicLevelCriteriaDTO? parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var academicLevels = query.ToList();

            var response = (from academicLevel in academicLevels
                            select MapModelToDTO(academicLevel, academicLevel.Localizations))
                           .ToList();

            return response;
        }

        public void Delete(Guid id)
        {
            var academicLevel = _dbContext.AcademicLevels.SingleOrDefault(x => x.Id == id);

            if (academicLevel is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.AcademicLevels.Remove(academicLevel);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static AcademicLevelDTO MapModelToDTO(AcademicLevel model, IEnumerable<AcademicLevelLocalization> localizations)
        {
            return new AcademicLevelDTO
            {
                Id = model.Id,
                Name = model.Name,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                IsActive = model.IsActive,
                Localizations = localizations is null ? Enumerable.Empty<AcademicLevelLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new AcademicLevelLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name,
                                                             FormalName = localize.FormalName
                                                         })
                                                        .ToList()
            };
        }

        private static IEnumerable<AcademicLevelLocalization> MapLocalizationDTOToModel(
                IEnumerable<AcademicLevelLocalizationDTO> localizations,
                AcademicLevel model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<AcademicLevelLocalization>();
            }

            var response = (from locale in localizations
                            select new AcademicLevelLocalization
                            {
                                AcademicLevel = model,
                                Language = locale.Language,
                                Name = locale.Name,
                                FormalName = locale.FormalName
                            })
                            .ToList();

            return response;
        }

        private IQueryable<AcademicLevel> GenerateSearchQuery(SearchAcademicLevelCriteriaDTO? parameters = null)
        {
            var query = _dbContext.AcademicLevels.Include(x => x.Localizations)
                                                 .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Name));
                }

                if (parameters.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == parameters.IsActive.Value);
                }
            }

            query = query.OrderBy(x => x.UpdatedAt);

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

