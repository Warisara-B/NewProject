using Microsoft.AspNetCore.Http;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Entity.Exception;
using Plexus.Integration;
using Plexus.Utility.ViewModel;
using Plexus.Database.Model.Academic.Faculty;
using Plexus.Database;
using Microsoft.EntityFrameworkCore;
using Plexus.Database.Model.Localization.Academic.Faculty;
using Plexus.Utility.Extensions;
using ServiceStack;

namespace Plexus.Client.src.Academic
{
    public class DepartmentManager : IDepartmentManager
    {
        private readonly IBlobStorageProvider _blobStorageProvider;
        private readonly DatabaseContext _dbContext;

        public DepartmentManager(IBlobStorageProvider blobStorageProvider,
                                 DatabaseContext dbContext)
        {
            _blobStorageProvider = blobStorageProvider;
            _dbContext = dbContext;
        }

        public async Task<DepartmentViewModel> CreateAsync(CreateDepartmentViewModel request, Guid userId)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var faculty = _dbContext.Faculties.AsNoTracking()
                                              .Include(x => x.Localizations)
                                              .FirstOrDefault(x => x.Id == request.FacultyId);

            if (faculty is null)
            {
                throw new FacultyException.NotFound(request.FacultyId);
            }

            var duplicateDepartments = _dbContext.Departments.Where(x => x.FacultyId == request.FacultyId
                                                                    && x.Code == request.Code)
                                                             .ToList();

            if (duplicateDepartments.Any())
            {
                throw new DepartmentException.Duplicate(request.Code);
            }

            var model = new Department
            {
                FacultyId = request.FacultyId,
                Code = request.Code,
                Name = request.Name,
                FormalName = request.FormalName,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "requester", // TODO : Add requester.
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "requester" // TODO : Add requester.
            };

            if (request.LogoImage != null
                && request.LogoImage.ContentType.StartsWith("image", true, null))
            {
                model.LogoImagePath = await GetLogoImagePathAsync(request.LogoImage);
            }

            var localizes = MapLocalizationViewModelToModel(request.Localizations, model).ToList();

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

            var response = MapModelToViewModel(model, faculty, localizes);

            return response;
        }

        public DepartmentViewModel GetById(Guid id)
        {
            var department = _dbContext.Departments.AsNoTracking()
                                                   .Include(x => x.Faculty)
                                                        .ThenInclude(x => x.Localizations)
                                                   .Include(x => x.Localizations)
                                                   .FirstOrDefault(x => x.Id == id);

            if (department is null)
            {
                throw new DepartmentException.NotFound(id);
            }

            var response = MapModelToViewModel(department, department.Faculty, department.Localizations);

            return response;
        }

        public IEnumerable<DepartmentDropDownViewModel> GetDropdownList(SearchDepartmentCriteriaViewModel? parameters)
        {
            var departments = Search(parameters).ToList();

            var response = (from department in departments
                            orderby department.Code
                            select new DepartmentDropDownViewModel
                            {
                                Id = department.Id.ToString(),
                                FacultyId = department.FacultyId,
                                Name = department.Name
                            })
                           .ToList();

            return response;
        }

        public IEnumerable<DepartmentViewModel> Search(SearchDepartmentCriteriaViewModel? parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var departments = query.ToList();

            var response = (from department in departments
                            select MapModelToViewModel(department, department.Faculty, department.Localizations))
                           .ToList();

            return response;
        }

        public PagedViewModel<DepartmentViewModel> Search(SearchDepartmentCriteriaViewModel criteria, int page, int pageSize)
        {
            var query = GenerateSearchQuery(criteria);

            var pagedDepartment = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<DepartmentViewModel>
            {
                Page = pagedDepartment.Page,
                TotalPage = pagedDepartment.TotalPage,
                TotalItem = pagedDepartment.TotalItem,
                Items = (from department in pagedDepartment.Items
                         select MapModelToViewModel(department, department.Faculty, department.Localizations))
                        .ToList()
            };

            return response;
        }

        public async Task<DepartmentViewModel> UpdateAsync(Guid id, UpdateDepartmentViewModel request, Guid userId)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var faculty = _dbContext.Faculties.Include(x => x.Localizations)
                                              .FirstOrDefault(x => x.Id == request.FacultyId);

            if (faculty is null)
            {
                throw new FacultyException.NotFound(request.FacultyId);
            }

            var department = _dbContext.Departments.Include(x => x.Localizations)
                                                   .SingleOrDefault(x => x.Id == id);
            if (department is null)
            {
                throw new DepartmentException.NotFound(id);
            }

            var duplicateDepartments = _dbContext.Departments.Where(x => x.Id != id
                                                                    && x.FacultyId == request.FacultyId
                                                                    && x.Code == request.Code)
                                                             .ToList();

            var localizes = MapLocalizationViewModelToModel(request.Localizations, department).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                department.FacultyId = request.FacultyId;
                department.Name = request.Name;
                department.Code = request.Code;
                department.FormalName = request.FormalName;
                department.UpdatedAt = DateTime.UtcNow;
                department.UpdatedBy = "requester"; // TODO : Add requester.
                department.IsActive = request.IsActive;

                if (request.DeleteLogo)
                {
                    department.LogoImagePath = null;
                }

                if (request.LogoImage != null
                    && request.LogoImage.ContentType.StartsWith("image", true, null))
                {
                    department.LogoImagePath = await GetLogoImagePathAsync(request.LogoImage);
                }

                _dbContext.DepartmentLocalizations.RemoveRange(department.Localizations);

                if (localizes.Any())
                {
                    _dbContext.DepartmentLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToViewModel(department, faculty, localizes);

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

        private DepartmentViewModel MapModelToViewModel(Department department, Faculty faculty, IEnumerable<DepartmentLocalization> localizations)
        {
            if (department is null)
            {
                return null;
            }

            return new DepartmentViewModel
            {
                Id = department.Id,
                FacultyId = department.FacultyId,
                FacultyName = faculty.Name,
                Code = department.Code,
                LogoImageURL = _blobStorageProvider.GetBlobPublicUrl(department.LogoImagePath),
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt,
                UpdatedAt = department.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<DepartmentLocalizationViewModel>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new DepartmentLocalizationViewModel
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name,
                                                             FormalName = localize.FormalName
                                                         })
                                                        .ToList()
            };
        }

        private static IEnumerable<DepartmentLocalization> MapLocalizationViewModelToModel(
            IEnumerable<DepartmentLocalizationViewModel>? localizations,
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

        private async Task<string> GetLogoImagePathAsync(IFormFile logoImage)
        {
            var imagePath = $"/department/{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}.{logoImage.FileName.Split('.').Last()}";

            await _blobStorageProvider.UploadFileAsync(imagePath, logoImage.OpenReadStream());

            return imagePath;
        }

        private IQueryable<Department> GenerateSearchQuery(SearchDepartmentCriteriaViewModel? parameters)
        {
            var query = _dbContext.Departments.Include(x => x.Faculty)
                                                    .ThenInclude(x => x.Localizations)
                                              .Include(x => x.Localizations)
                                              .AsNoTracking();

            if (parameters is not null)
            {
                if (parameters.FacultyId.HasValue)
                {
                    query = query.Where(x => x.FacultyId == parameters.FacultyId.Value);
                }

                if (!string.IsNullOrEmpty(parameters.Code))
                {
                    query = query.Where(x => x.Code.Contains(parameters.Code));
                }

                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => x.Name!.Contains(parameters.Name));
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

