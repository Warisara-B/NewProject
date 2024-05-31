using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Academic.Faculty;
using Plexus.Database.Model.Localization.Academic.Faculty;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;

namespace Plexus.Entity.Provider.src.Academic
{
    public class FacultyProvider : IFacultyProvider
    {
        private readonly DatabaseContext _dbContext;

        public FacultyProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public FacultyDTO Create(CreateFacultyDTO request, string requester)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var model = new Faculty
            {
                Code = request.Code,
                Name = request.Name,
                FormalName = request.FormalName,
                LogoImagePath = request.LogoImagePath,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Faculties.Add(model);

                if (localizes.Any())
                {
                    _dbContext.FacultyLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizes);

            return response;
        }

        public IEnumerable<FacultyDTO> GetAll()
        {
            var faculties = _dbContext.Faculties.AsNoTracking()
                                                .Include(x => x.Localizations)
                                                .ToList();

            var response = (from faculty in faculties
                            orderby faculty.Code
                            select MapModelToDTO(faculty, faculty.Localizations))
                           .ToList();

            return response;
        }

        public FacultyDTO GetById(Guid id)
        {
            var faculty = _dbContext.Faculties.AsNoTracking()
                                              .Include(x => x.Localizations)
                                              .SingleOrDefault(x => x.Id == id);

            if (faculty is null)
            {
                throw new FacultyException.NotFound(id);
            }

            var response = MapModelToDTO(faculty, faculty.Localizations);

            return response;
        }

        public IEnumerable<FacultyDTO> GetById(IEnumerable<Guid> ids)
        {
            var faculties = _dbContext.Faculties.AsNoTracking()
                                                .Include(x => x.Localizations)
                                                .Where(x => ids.Contains(x.Id))
                                                .ToList();

            var response = (from faculty in faculties
                            select MapModelToDTO(faculty, faculty.Localizations))
                           .ToList();

            return response;
        }

        public FacultyDTO Update(FacultyDTO request, string requester)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var faculty = _dbContext.Faculties.Include(x => x.Localizations)
                                              .SingleOrDefault(x => x.Id == request.Id);
            if (faculty is null)
            {
                throw new FacultyException.NotFound(request.Id);
            }

            var localizes = MapLocalizationDTOToModel(request.Localizations, faculty).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                faculty.Name = request.Name;
                faculty.Code = request.Code;
                faculty.FormalName = request.FormalName;
                faculty.LogoImagePath = request.LogoImagePath;
                faculty.UpdatedAt = DateTime.UtcNow;
                faculty.UpdatedBy = requester;
                faculty.IsActive = request.IsActive;

                _dbContext.FacultyLocalizations.RemoveRange(faculty.Localizations);

                if (localizes.Any())
                {
                    _dbContext.FacultyLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(faculty, localizes);

            return response;
        }

        public void Delete(Guid id)
        {
            var faculty = _dbContext.Faculties.SingleOrDefault(x => x.Id == id);

            if (faculty is null)
            {
                return;
            }

            var departments = _dbContext.Departments.Where(x => x.FacultyId == id)
                                                    .ToList();

            if (departments.Any())
            {
                throw new FacultyException.ContainsDepartments(id);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Faculties.Remove(faculty);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static FacultyDTO MapModelToDTO(Faculty faculty, IEnumerable<FacultyLocalization> localizations)
        {
            if (faculty is null)
            {
                return null;
            }

            return new FacultyDTO
            {
                Id = faculty.Id,
                Code = faculty.Code,
                Name = faculty.Name,
                FormalName = faculty.FormalName,
                LogoImagePath = faculty.LogoImagePath,
                IsActive = faculty.IsActive,
                CreatedAt = faculty.CreatedAt,
                UpdatedAt = faculty.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<FacultyLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new FacultyLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name,
                                                             FormalName = localize.FormalName
                                                         })
                                                        .ToList()
            };
        }

        private static IEnumerable<FacultyLocalization> MapLocalizationDTOToModel(
            IEnumerable<FacultyLocalizationDTO>? localizations,
            Faculty model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<FacultyLocalization>();
            }

            var response = (from locale in localizations
                            select new FacultyLocalization
                            {
                                Faculty = model,
                                Language = locale.Language,
                                Name = locale.Name,
                                FormalName = locale.FormalName,
                            })
                           .ToList();

            return response;
        }
    }
}