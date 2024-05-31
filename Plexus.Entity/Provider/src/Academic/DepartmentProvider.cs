using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model.Academic.Faculty;
using Plexus.Database.Model.Localization.Academic.Faculty;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;

namespace Plexus.Entity.Provider.src.Academic
{
    public class DepartmentProvider : IDepartmentProvider
    {
        private readonly DatabaseContext _dbContext;

        public DepartmentProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DepartmentDTO Create(CreateDepartmentDTO request, string requester)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var model = new Department
            {
                FacultyId = request.FacultyId,
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
                _dbContext.Departments.Add(model);

                if (localizes.Any())
                {
                    _dbContext.DepartmentLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizes);

            return response;
        }

        public IEnumerable<DepartmentDTO> GetAll()
        {
            var departments = _dbContext.Departments.AsNoTracking()
                                                    .Include(x => x.Localizations)
                                                    .ToList();

            var response = (from department in departments
                            orderby department.Code
                            select MapModelToDTO(department, department.Localizations))
                           .ToList();

            return response;
        }

        public DepartmentDTO GetById(Guid id)
        {
            var department = _dbContext.Departments.AsNoTracking()
                                                   .Include(x => x.Localizations)
                                                   .SingleOrDefault(x => x.Id == id);

            if (department is null)
            {
                throw new DepartmentException.NotFound(id);
            }

            var response = MapModelToDTO(department, department.Localizations);

            return response;
        }

        public IEnumerable<DepartmentDTO> GetById(IEnumerable<Guid> ids)
        {
            var departments = _dbContext.Departments.AsNoTracking()
                                                    .Include(x => x.Localizations)
                                                    .Where(x => ids.Contains(x.Id))
                                                    .ToList();

            var response = (from department in departments
                            select MapModelToDTO(department, department.Localizations))
                           .ToList();

            return response;
        }

        public IEnumerable<DepartmentDTO> GetByFacultyId(Guid facultyId)
        {
            var departments = _dbContext.Departments.AsNoTracking()
                                                    .Where(x => x.FacultyId == facultyId)
                                                    .ToList();

            var response = (from department in departments
                            orderby department.Code
                            select MapModelToDTO(department, department.Localizations))
                           .ToList();

            return response;
        }

        public DepartmentDTO Update(DepartmentDTO request, string requester)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var department = _dbContext.Departments.Include(x => x.Localizations)
                                                   .SingleOrDefault(x => x.Id == request.Id);
            if (department is null)
            {
                throw new DepartmentException.NotFound(request.Id);
            }

            var localizes = MapLocalizationDTOToModel(request.Localizations, department).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                department.FacultyId = request.FacultyId;
                department.Name = request.Name;
                department.Code = request.Code;
                department.FormalName = request.FormalName;
                department.LogoImagePath = request.LogoImagePath;
                department.UpdatedAt = DateTime.UtcNow;
                department.UpdatedBy = requester;
                department.IsActive = request.IsActive;

                _dbContext.DepartmentLocalizations.RemoveRange(department.Localizations);

                if (localizes.Any())
                {
                    _dbContext.DepartmentLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(department, localizes);

            return response;
        }

        public void Delete(Guid id)
        {
            var department = _dbContext.Departments.SingleOrDefault(x => x.Id == id);
            if (department is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Departments.Remove(department);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static DepartmentDTO MapModelToDTO(Department department, IEnumerable<DepartmentLocalization> localizations)
        {
            if (department is null)
            {
                return null;
            }

            return new DepartmentDTO
            {
                Id = department.Id,
                FacultyId = department.FacultyId,
                Code = department.Code,
                Name = department.Name,
                FormalName = department.FormalName,
                LogoImagePath = department.LogoImagePath,
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt,
                UpdatedAt = department.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<DepartmentLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new DepartmentLocalizationDTO
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name,
                                                             FormalName = localize.FormalName
                                                         })
                                                        .ToList()
            };
        }

        private static IEnumerable<DepartmentLocalization> MapLocalizationDTOToModel(
            IEnumerable<DepartmentLocalizationDTO>? localizations,
            Department model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<DepartmentLocalization>();
            }

            var response = (from locale in localizations
                            select new DepartmentLocalization
                            {
                                Department = model,
                                Language = locale.Language,
                                Name = locale.Name,
                                FormalName = locale.FormalName
                            })
                           .ToList();

            return response;
        }
    }
}