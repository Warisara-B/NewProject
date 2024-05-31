using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Database;
using Plexus.Database.Model.Academic.Faculty;
using Plexus.Database.Model.Localization.Academic.Faculty;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Integration;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.src.Academic
{
    public class FacultyManager : IFacultyManager
    {
        private readonly IFacultyProvider _facultyProvider;
        private readonly IBlobStorageProvider _blobStorageProvider;
        private readonly DatabaseContext _dbContext;

        public FacultyManager(IFacultyProvider facultyProvider,
                              IBlobStorageProvider blobStoreageProvider,
                              DatabaseContext dbContext)
        {
            _facultyProvider = facultyProvider;
            _blobStorageProvider = blobStoreageProvider;
            _dbContext = dbContext;
        }

        public async Task<FacultyViewModel> CreateAsync(CreateFacultyViewModel request, Guid userId)
        {
            var faculties = _facultyProvider.GetAll()
                                            .ToList();

            var duplicateFaculties = faculties.Where(x => x.Code == request.Code)
                                              .ToList();

            if (duplicateFaculties.Any())
            {
                throw new FacultyException.Duplicate(request.Code);
            }

            var dto = new CreateFacultyDTO
            {
                Name = request.Name,
                Code = request.Code,
                FormalName = request.FormalName,
                IsActive = request.IsActive,
                Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList()
            };

            if (request.LogoImage != null
                && request.LogoImage.ContentType.StartsWith("image", true, null))
            {
                dto.LogoImagePath = await GetLogoImagePathAsync(request.LogoImage!);
            }

            var faculty = _facultyProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(faculty);

            return response;
        }

        public FacultyViewModel GetById(Guid id)
        {
            var faculty = _facultyProvider.GetById(id);

            var response = MapDTOToViewModel(faculty);

            return response;
        }

        public IEnumerable<BaseDropDownViewModel> GetDropdownList(SearchFacultyCriteriaViewModel parameters)
        {
            var faculties = Search().ToList();

            var response = (from faculty in faculties
                            orderby faculty.Code
                            select new BaseDropDownViewModel
                            {
                                Id = faculty.Id.ToString(),
                                Name = faculty.Name
                            })
                           .ToList();

            return response;
        }

        public IEnumerable<FacultyViewModel> Search(SearchFacultyCriteriaViewModel? parameters = null)
        {
            var query = GenerateSearchQuery(parameters);

            var faculties = query.ToList();

            var response = (from faculty in faculties
                            select MapModelToViewModel(faculty, faculty.Localizations))
                           .ToList();

            return response;
        }

        public PagedViewModel<FacultyViewModel> Search(SearchFacultyCriteriaViewModel parameters, int page, int pageSize)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedFaculty = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<FacultyViewModel>
            {
                Page = pagedFaculty.Page,
                TotalPage = pagedFaculty.TotalPage,
                TotalItem = pagedFaculty.TotalItem,
                Items = (from faculty in pagedFaculty.Items
                         select MapModelToViewModel(faculty, faculty.Localizations))
                        .ToList()
            };

            return response;
        }

        public async Task<FacultyViewModel> UpdateAsync(Guid id, UpdateFacultyViewModel request, Guid userId)
        {
            var faculties = _facultyProvider.GetAll()
                                            .ToList();

            var faculty = faculties.SingleOrDefault(x => x.Id == id);

            if (faculty is null)
            {
                throw new FacultyException.NotFound(id);
            }

            var duplicateFaculties = faculties.Where(x => x.Id != id
                                                          && x.Code == request.Code)
                                              .ToList();

            if (duplicateFaculties.Any())
            {
                throw new FacultyException.Duplicate(request.Code);
            }

            faculty.Name = request.Name;
            faculty.Code = request.Code;
            faculty.FormalName = request.FormalName;
            faculty.IsActive = request.IsActive;
            faculty.Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList();

            if (request.DeleteLogo)
            {
                faculty.LogoImagePath = null;
            }

            if (request.LogoImage != null
                && request.LogoImage.ContentType.StartsWith("image", true, null))
            {
                faculty.LogoImagePath = await GetLogoImagePathAsync(request.LogoImage);
            }

            var updatedFaculty = _facultyProvider.Update(faculty, userId.ToString());

            var response = MapDTOToViewModel(updatedFaculty);

            return response;
        }

        public void Delete(Guid id)
        {
            _facultyProvider.Delete(id);
        }

        private FacultyViewModel MapDTOToViewModel(FacultyDTO dto)
        {
            var response = new FacultyViewModel
            {
                Id = dto.Id,
                Code = dto.Code,
                LogoImageURL = _blobStorageProvider.GetBlobPublicUrl(dto.LogoImagePath),
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                Localizations = (from localize in dto.Localizations
                                 orderby localize.Language
                                 select new FacultyLocalizationViewModel
                                 {
                                     Language = localize.Language,
                                     Name = localize.Name,
                                     FormalName = localize.FormalName
                                 })
                                .ToList()
            };

            return response;
        }

        private FacultyViewModel MapModelToViewModel(Faculty model, IEnumerable<FacultyLocalization> localizations)
        {
            if (model is null)
            {
                return null;
            }

            return new FacultyViewModel
            {
                Id = model.Id,
                Code = model.Code,
                LogoImageURL = _blobStorageProvider.GetBlobPublicUrl(model.LogoImagePath),
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<FacultyLocalizationViewModel>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new FacultyLocalizationViewModel
                                                         {
                                                             Language = localize.Language,
                                                             Name = localize.Name,
                                                             FormalName = localize.FormalName
                                                         })
                                                        .ToList()
            };
        }

        private static IEnumerable<FacultyLocalizationDTO> MapLocalizationViewModelToDTO(
            IEnumerable<FacultyLocalizationViewModel>? localizations)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<FacultyLocalizationDTO>();
            }

            var response = (from locale in localizations
                            select new FacultyLocalizationDTO
                            {
                                Language = locale.Language,
                                Name = locale.Name,
                                FormalName = locale.FormalName
                            })
                           .ToList();

            return response;
        }

        private async Task<string> GetLogoImagePathAsync(IFormFile logoImage)
        {
            var imagePath = $"faculty/{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}.{logoImage.FileName.Split('.').Last()}";

            await _blobStorageProvider.UploadFileAsync(imagePath, logoImage.OpenReadStream());

            return imagePath;
        }

        private IQueryable<Faculty> GenerateSearchQuery(SearchFacultyCriteriaViewModel? parameters)
        {
            var query = _dbContext.Faculties.Include(x => x.Localizations)
                                            .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Code))
                {
                    query = query.Where(x => x.Code.Contains(parameters.Code));
                }

                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => x.Name.Contains(parameters.Name));
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