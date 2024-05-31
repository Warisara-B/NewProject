using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Academic;
using Plexus.Entity.DTO.Academic;
using Plexus.Utility.ViewModel;
using Plexus.Utility.Extensions;
using Plexus.Database.Model.Localization.Academic;
using Plexus.Entity.Exception;
using Plexus.Entity.DTO.SearchFilter;

namespace Plexus.Entity.Provider.src.Academic
{
    public class AcademicProgramProvider : IAcademicProgramProvider
    {
        private readonly DatabaseContext _dbContext;

        public AcademicProgramProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AcademicProgramDTO Create(CreateAcademicProgramDTO request, string requester)
        {
            var model = new AcademicProgram
            {
                Name = request.Name,
                FormalName = request.FormalName,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.AcademicPrograms.Add(model);

                if (localizes.Any())
                {
                    _dbContext.AcademicProgramLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizes);

            return response;
        }

        public IEnumerable<AcademicProgramDTO> GetById(IEnumerable<Guid> ids)
        {
            var academicPrograms = _dbContext.AcademicPrograms.Include(x => x.Localizations)
                                                              .AsNoTracking()
                                                              .Where(x => ids.Contains(x.Id))
                                                              .ToList();

            var response = (from academicProgram in academicPrograms
                            select MapModelToDTO(academicProgram, academicProgram.Localizations))
                           .ToList();

            return response;
        }

        public AcademicProgramDTO GetById(Guid id)
        {
            var academicProgram = _dbContext.AcademicPrograms.Include(x => x.Localizations)
                                                             .AsNoTracking()
                                                             .SingleOrDefault(x => x.Id == id);

            if (academicProgram is null)
            {
                throw new AcademicProgramException.NotFound(id);
            }

            var response = MapModelToDTO(academicProgram, academicProgram.Localizations);

            return response;
        }

        public AcademicProgramDTO Update(AcademicProgramDTO request, string requester)
        {
            var academicProgram = _dbContext.AcademicPrograms.Include(x => x.Localizations)
                                                             .SingleOrDefault(x => x.Id == request.Id);

            if (academicProgram is null)
            {
                throw new AcademicProgramException.NotFound(request.Id);
            }

            var localizes = request.Localizations is null ? Enumerable.Empty<AcademicProgramLocalization>()
                                                          : (from data in request.Localizations
                                                             select new AcademicProgramLocalization
                                                             {
                                                                 Language = data.Language,
                                                                 AcademicProgramId = academicProgram.Id,
                                                                 Name = data.Name,
                                                                 FormalName = data.FormalName
                                                             })
                                                            .ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                academicProgram.Name = request.Name;
                academicProgram.FormalName = request.FormalName;
                academicProgram.UpdatedAt = DateTime.UtcNow;
                academicProgram.UpdatedBy = requester;

                _dbContext.AcademicProgramLocalizations.RemoveRange(academicProgram.Localizations);

                if (localizes.Any())
                {
                    _dbContext.AcademicProgramLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(academicProgram, localizes);

            return response;
        }

        public void Delete(Guid id)
        {
            var academicProgram = _dbContext.AcademicPrograms.SingleOrDefault(x => x.Id == id);

            if (academicProgram is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.AcademicPrograms.Remove(academicProgram);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static AcademicProgramDTO MapModelToDTO(AcademicProgram model, IEnumerable<AcademicProgramLocalization> localizations)
        {
            return new AcademicProgramDTO
            {
                Id = model.Id,
                Name = model.Name,
                FormalName = model.FormalName,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<AcademicProgramLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new AcademicProgramLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name,
                                                             FormalName = localize.FormalName
                                                         })
                                                        .ToList()
            };
        }

        private static IEnumerable<AcademicProgramLocalization> MapLocalizationDTOToModel(
                IEnumerable<AcademicProgramLocalizationDTO> localizations,
                AcademicProgram model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<AcademicProgramLocalization>();
            }

            var response = (from locale in localizations
                            select new AcademicProgramLocalization
                            {
                                AcademicProgram = model,
                                Language = locale.Language,
                                Name = locale.Name,
                                FormalName = locale.FormalName
                            })
                            .ToList();

            return response;
        }
    }
}