using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Localization.Academic;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;

namespace Plexus.Entity.Provider.src.Academic
{
    public class TeachingTypeProvider : ITeachingTypeProvider
    {
        private readonly DatabaseContext _dbContext;

        public TeachingTypeProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TeachingTypeDTO Create(CreateTeachingTypeDTO request, string requester)
        {
            var model = new TeachingType
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester,
                IsActive = request.IsActive
            };

            var localizes = request.Localizations is null ? Enumerable.Empty<TeachingTypeLocalization>()
                                                              : (from localize in request.Localizations
                                                                 select new TeachingTypeLocalization
                                                                 {
                                                                     TeachingType = model,
                                                                     Language = localize.Language,
                                                                     Name = localize.Name,
                                                                     Description = localize.Description
                                                                 })
                                                                .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.TeachingTypes.Add(model);

                if (localizes.Any())
                {
                    _dbContext.TeachingTypeLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizes);

            return response;
        }

        public TeachingTypeDTO GetById(Guid id)
        {
            var teachingType = _dbContext.TeachingTypes
                                         .AsNoTracking()
                                         .Include(x => x.Localizations)
                                         .SingleOrDefault(x => x.Id == id);

            if (teachingType is null)
            {
                throw new TeachingTypeException.NotFound(id);
            }

            var response = MapModelToDTO(teachingType, teachingType.Localizations);

            return response;
        }

        public TeachingTypeDTO Update(TeachingTypeDTO request, string requester)
        {
            var teachingType = _dbContext.TeachingTypes.Include(x => x.Localizations)
                                                       .SingleOrDefault(x => x.Id == request.Id);

            if (teachingType is null)
            {
                throw new TeachingTypeException.NotFound(request.Id);
            }

            var localizes = request.Localizations is null ? Enumerable.Empty<TeachingTypeLocalization>()
                                                          : (from data in request.Localizations
                                                             select new TeachingTypeLocalization
                                                             {
                                                                 Language = data.Language,
                                                                 TeachingTypeId = teachingType.Id,
                                                                 Name = data.Name,
                                                                 Description = data.Description
                                                             })
                                                            .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                teachingType.Name = request.Name;
                teachingType.Description = request.Description;
                teachingType.UpdatedAt = DateTime.UtcNow;
                teachingType.UpdatedBy = requester;
                teachingType.IsActive = request.IsActive;

                _dbContext.TeachingTypeLocalizations.RemoveRange(teachingType.Localizations);

                if (localizes.Any())
                {
                    _dbContext.TeachingTypeLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(teachingType, localizes);

            return response;
        }

        public void Delete(Guid id)
        {
            var teachingType = _dbContext.TeachingTypes.SingleOrDefault(x => x.Id == id);

            if (teachingType is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.TeachingTypes.Remove(teachingType);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static TeachingTypeDTO MapModelToDTO(TeachingType model, IEnumerable<TeachingTypeLocalization> localizations)
        {
            return new TeachingTypeDTO
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                IsActive = model.IsActive,
                Localizations = localizations is null ? Enumerable.Empty<TeachingTypeLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new TeachingTypeLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name,
                                                             Description = localize.Description
                                                         })
                                                        .ToList()
            };
        }

        private static IEnumerable<TeachingTypeLocalization> MapLocalizationDTOToModel(
                IEnumerable<TeachingTypeLocalizationDTO> localizations,
                TeachingType model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<TeachingTypeLocalization>();
            }

            var response = (from locale in localizations
                            select new TeachingTypeLocalization
                            {
                                TeachingType = model,
                                Language = locale.Language,
                                Name = locale.Name,
                                Description = locale.Description
                            })
                            .ToList();

            return response;
        }
    }
}