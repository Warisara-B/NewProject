using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Database;
using Plexus.Database.Model;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Faculty;
using Plexus.Database.Model.Localization;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Integration;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;
using ServiceStack;

namespace Plexus.Client.src
{
    public class EmployeeManager : IEmployeeManager
    {
        private readonly IBlobStorageProvider _blobStoreageProvider;
        private readonly IEmployeeProvider _employeeProivder;
        private readonly IAcademicLevelProvider _academicLevelProvider;
        private readonly IFacultyProvider _facultyProvider;
        private readonly IDepartmentProvider _departmentProvider;
        private readonly IEmployeeGroupProvider _employeeGroupProvider;
        private readonly DatabaseContext _dbContext;

        public EmployeeManager(IBlobStorageProvider blobStorageProvider,
                                 IEmployeeProvider employeeProvider,
                                 IAcademicLevelProvider academicLevelProvider,
                                 IFacultyProvider facultyProvider,
                                 IDepartmentProvider departmentProvider,
                                 IEmployeeGroupProvider employeeGroupProvider,
                                 DatabaseContext dbContext)
        {
            _blobStoreageProvider = blobStorageProvider;
            _employeeProivder = employeeProvider;
            _academicLevelProvider = academicLevelProvider;
            _facultyProvider = facultyProvider;
            _departmentProvider = departmentProvider;
            _employeeGroupProvider = employeeGroupProvider;
            _dbContext = dbContext;
        }

        public EmployeeViewModel Create(CreateEmployeeViewModel request, Guid userId)
        {
            FacultyDTO? faculty = null;
            DepartmentDTO? department = null;
            InstructorTypeDTO? type = null;
            EmployeeGroupDTO? employeeGroup = null;
            var academicLevelIds = Enumerable.Empty<Guid>();

            if (request.Status != null)
            {
                faculty = _facultyProvider.GetById((Guid)request.Status.FacultyId);

                department = _departmentProvider.GetById((Guid)request.Status.DepartmentId);

                employeeGroup = _employeeGroupProvider.GetById((Guid)request.Status.EmployeeGroupId);

                if (request.Status.AcademicLevelIds != null && request.Status.AcademicLevelIds.Any())
                {
                    academicLevelIds = request.Status.AcademicLevelIds;
                }
            }

            List<AcademicLevelDTO>? academicLevels = null;

            if (academicLevelIds != null && academicLevelIds.Any())
            {
                academicLevels = _academicLevelProvider.GetById(academicLevelIds)
                                                       .ToList();

                foreach (var levelId in academicLevelIds)
                {
                    var matchedAcademicLevel = academicLevels.SingleOrDefault(x => x.Id == levelId);
                    if (matchedAcademicLevel is null)
                    {
                        throw new AcademicLevelException.NotFound(levelId);
                    }
                }
            }

            var dto = new CreateEmployeeDTO
            {
                Code = request.Code,
                Title = request.Title,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Gender = request.Gender,
                Nationality = request.Nationality,
                Race = request.Race,
                Religion = request.Religion,
                IsActive = request.IsActive,
                Address = request.Address is null ? null : MapAddressViewModelToDTO(request.Address),
                Status = request.Status is null ? null : MapWorkStatusViewModelToDTO(request.Status),
                Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList()
            };

            var instructor = _employeeProivder.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(instructor, faculty, department, type, employeeGroup, academicLevels);

            return response;
        }

        public PagedViewModel<EmployeeInformationViewModel> Search(SearchEmployeeCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedEmployee = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<EmployeeInformationViewModel>
            {
                Page = pagedEmployee.Page,
                PageSize = pagedEmployee.PageSize,
                TotalPage = pagedEmployee.TotalPage,
                TotalItem = pagedEmployee.TotalItem,
                Items = (from employee in pagedEmployee.Items
                         select MapInformationViewModel(employee, employee.WorkInformation, employee.Localizations))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<InstructorDropDownViewModel> GetDropDownList(SearchCriteriaViewModel parameters)
        {
            var instructors = _employeeProivder.Search(parameters);

            var response = (from instructor in instructors
                            select MapDTOToDropDown(instructor))
                           .ToList();

            return response;
        }

        public IEnumerable<string> GetEmails(SearchCriteriaViewModel parameters)
        {
            var instructors = _employeeProivder.Search(parameters);

            var response = (from instructor in instructors
                            where !string.IsNullOrEmpty(instructor.Address?.EmailAddress)
                            select instructor.Address!.EmailAddress)
                           .Distinct()
                           .ToList();

            return response;
        }

        public EmployeeViewModel GetById(Guid id)
        {
            var instructor = _employeeProivder.GetById(id);

            FacultyDTO? faculty = null;
            DepartmentDTO? department = null;
            InstructorTypeDTO? type = null;
            EmployeeGroupDTO? employeeGroup = null;
            var academicLevelIds = Enumerable.Empty<Guid>();

            if (instructor.Status != null)
            {
                faculty = _facultyProvider.GetById((Guid)instructor.Status.FacultyId);

                department = _departmentProvider.GetById((Guid)instructor.Status.DepartmentId);

                employeeGroup = _employeeGroupProvider.GetById((Guid)instructor.Status.EmployeeGroupId);

                if (instructor.Status.AcademicLevelIds != null && instructor.Status.AcademicLevelIds.Any())
                {
                    academicLevelIds = instructor.Status.AcademicLevelIds;
                }
            }

            List<AcademicLevelDTO>? academicLevels = null;

            if (academicLevelIds != null && academicLevelIds.Any())
            {
                academicLevels = _academicLevelProvider.GetById(academicLevelIds)
                                                       .ToList();
            }

            var response = MapDTOToViewModel(instructor, faculty, department, type, employeeGroup, academicLevels);

            return response;
        }

        public EmployeeViewModel Update(EmployeeViewModel request, Guid userId)
        {
            var instructor = _employeeProivder.GetById(request.Id);

            FacultyDTO? faculty = null;
            DepartmentDTO? department = null;
            InstructorTypeDTO? type = null;
            EmployeeGroupDTO? employeeGroup = null;
            var academicLevelIds = Enumerable.Empty<Guid>();

            if (request.Status != null)
            {
                faculty = _facultyProvider.GetById((Guid)request.Status.FacultyId);

                department = _departmentProvider.GetById((Guid)request.Status.DepartmentId);

                employeeGroup = _employeeGroupProvider.GetById((Guid)request.Status.EmployeeGroupId);

                if (request.Status.AcademicLevelIds != null && request.Status.AcademicLevelIds.Any())
                {
                    academicLevelIds = request.Status.AcademicLevelIds;
                }
            }

            List<AcademicLevelDTO>? academicLevels = null;

            if (academicLevelIds != null && academicLevelIds.Any())
            {
                academicLevels = _academicLevelProvider.GetById(academicLevelIds)
                                                       .ToList();

                foreach (var levelId in academicLevelIds)
                {
                    var matchedAcademicLevel = academicLevels.SingleOrDefault(x => x.Id == levelId);
                    if (matchedAcademicLevel is null)
                    {
                        throw new AcademicLevelException.NotFound(levelId);
                    }
                }
            }

            instructor.Title = request.Title;
            instructor.Code = request.Code;
            instructor.FirstName = request.FirstName;
            instructor.MiddleName = request.MiddleName;
            instructor.LastName = request.LastName;
            instructor.Gender = request.Gender;
            instructor.Nationality = request.Nationality;
            instructor.Race = request.Race;
            instructor.Religion = request.Religion;
            instructor.IsActive = request.IsActive;
            instructor.Address = request.Address is null ? null : MapAddressViewModelToDTO(request.Address);
            instructor.Status = request.Status is null ? null : MapWorkStatusViewModelToDTO(request.Status);
            instructor.Localizations = MapLocalizationViewModelToDTO(request.Localizations);

            var updatedInstructor = _employeeProivder.Update(instructor, userId.ToString());

            var response = MapDTOToViewModel(updatedInstructor, faculty, department, type, employeeGroup, academicLevels);

            return response;
        }

        public void Delete(Guid id)
        {
            _employeeProivder.Delete(id);
        }

        public async Task UploadCardImageAsync(Guid id, IFormFile cardImage)
        {
            var instructor = _employeeProivder.GetById(id);

            if (cardImage is null || !cardImage.ContentType.StartsWith("image"))
            {
                return;
            }

            var cardImagePath = $"instructor/{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}.{cardImage.FileName.Split('.').Last()}";

            await _blobStoreageProvider.UploadFileAsync(cardImagePath, cardImage.OpenReadStream());

            _employeeProivder.UpdateCardImage(id, cardImagePath);
        }

        private EmployeeViewModel MapDTOToViewModel(EmployeeDTO dto, FacultyDTO? faculty = null, DepartmentDTO? department = null,
            InstructorTypeDTO? type = null, EmployeeGroupDTO? employeeGroup = null, IEnumerable<AcademicLevelDTO>? academicLevels = null)
        {
            var response = new EmployeeViewModel
            {
                Id = dto.Id,
                Code = dto.Code,
                Title = dto.Title,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                Nationality = dto.Nationality,
                Race = dto.Race,
                Religion = dto.Religion,
                IsActive = dto.IsActive,
                Address = dto.Address is null ? null : MapAddressDTOToViewModel(dto.Address),
                Status = dto.Status is null ? null : MapWorkStatusDTOToViewModel(dto.Status, faculty, department, type, employeeGroup, academicLevels),
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                CardImageUrl = _blobStoreageProvider.GetBlobPublicUrl(dto.CardImagePath),
                Localizations = (from localize in dto.Localizations
                                 select new EmployeeLocalizationViewModel
                                 {
                                     Language = localize.Language,
                                     FirstName = localize.FirstName,
                                     MiddleName = localize.MiddleName,
                                     LastName = localize.LastName
                                 })
                                .ToList()
            };

            return response;
        }

        private static EmployeeAddressViewModel MapAddressDTOToViewModel(EmployeeAddressDTO dto)
        {
            var response = new EmployeeAddressViewModel
            {
                Address = dto.Address,
                Country = dto.Country,
                Province = dto.Province,
                District = dto.District,
                SubDistrict = dto.SubDistrict,
                State = dto.State,
                City = dto.City,
                PostalCode = dto.PostalCode,
                PhoneNumber = dto.PhoneNumber,
                PhoneNumber2 = dto.PhoneNumber2,
                EmailAddress = dto.EmailAddress,
                PersonalEmailAddress = dto.PersonalEmailAddress,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };

            return response;
        }

        private static EmployeeWorkStatusViewModel MapWorkStatusDTOToViewModel(EmployeeWorkStatusDTO dto, FacultyDTO? faculty = null, DepartmentDTO? department = null,
            InstructorTypeDTO? type = null, EmployeeGroupDTO? employeeGroup = null, IEnumerable<AcademicLevelDTO>? academicLevels = null)
        {
            var response = new EmployeeWorkStatusViewModel
            {
                AcademicLevelIds = dto.AcademicLevelIds,
                AcademicLevels = dto.AcademicLevelIds is null || !dto.AcademicLevelIds.Any() ? null
                                                                                             : (from levelId in dto.AcademicLevelIds
                                                                                                let level = academicLevels is null || !academicLevels.Any() ? null
                                                                                                                                                            : academicLevels.SingleOrDefault(x => x.Id == levelId)
                                                                                                select new EmployeeAcademicLevelViewModel
                                                                                                {
                                                                                                    Id = levelId,
                                                                                                    NameEn = level?.Name
                                                                                                })
                                                                                               .ToList(),
                FacultyId = dto.FacultyId,
                DepartmentId = dto.DepartmentId,
                EmployeeGroupId = (Guid)dto.EmployeeGroupId,
                OfficeRoom = dto.OfficeRoom,
                Remark = dto.Remark,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                FacultyName = faculty?.Name,
                DepartmentName = department?.Name,
                TypeName = type?.Name,
                EmployeeGroupName = employeeGroup?.Name
            };

            return response;
        }

        private static IEnumerable<EmployeeLocalizationDTO> MapLocalizationViewModelToDTO(
            IEnumerable<EmployeeLocalizationViewModel>? localizations)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<EmployeeLocalizationDTO>();
            }

            var response = (from locale in localizations
                            select new EmployeeLocalizationDTO
                            {
                                Language = locale.Language,
                                FirstName = locale.FirstName,
                                MiddleName = locale.MiddleName,
                                LastName = locale.LastName
                            })
                           .ToList();

            return response;
        }

        private static InstructorDropDownViewModel MapDTOToDropDown(EmployeeDTO dto)
        {
            var response = new InstructorDropDownViewModel
            {
                Id = dto.Id.ToString(),
                Code = dto.Code,
                Title = dto.Title,
                Name = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName
            };

            return response;
        }

        private EmployeeAddressDTO MapAddressViewModelToDTO(CreateEmployeeAddressViewModel model)
        {
            var response = new EmployeeAddressDTO
            {
                Address = model.Address,
                Country = model.Country,
                Province = model.Province,
                District = model.District,
                SubDistrict = model.SubDistrict,
                State = model.State,
                City = model.City,
                PostalCode = model.PostalCode,
                PhoneNumber = model.PhoneNumber,
                PhoneNumber2 = model.PhoneNumber2,
                EmailAddress = model.EmailAddress,
                PersonalEmailAddress = model.PersonalEmailAddress
            };

            return response;
        }

        private EmployeeWorkStatusDTO MapWorkStatusViewModelToDTO(CreateEmployeeWorkStatusViewModel model)
        {
            var response = new EmployeeWorkStatusDTO
            {
                AcademicLevelIds = model.AcademicLevelIds is null || !model.AcademicLevelIds.Any() ? null
                                                                                                   : model.AcademicLevelIds!.ToList(),
                FacultyId = model.FacultyId,
                DepartmentId = model.DepartmentId,
                EmployeeGroupId = model.EmployeeGroupId,
                OfficeRoom = model.OfficeRoom,
                Remark = model.Remark
            };

            return response;
        }

        public EmployeeInformationViewModel UpdateEmployeeGeneralInformation(Guid id, UpdateEmployeeGeneralInformationViewModel request)
        {
            var employee = _dbContext.Employees.Include(x => x.AcademicPosition)
                                                    .ThenInclude(x => x.Localizations)
                                               .Include(x => x.CareerPosition)
                                                    .ThenInclude(x => x.Localizations)
                                               .Include(x => x.WorkInformation)
                                                    .ThenInclude(x => x.Faculty)
                                                        .ThenInclude(x => x.Localizations)
                                               .Include(x => x.WorkInformation)
                                                    .ThenInclude(x => x.EmployeeGroup)
                                                        .ThenInclude(x => x.Localizations)
                                               .Include(x => x.Localizations)
                                               .FirstOrDefault(x => x.Id == id);

            if (employee is null)
            {
                throw new EmployeeException.NotFound(id);
            }

            AcademicPosition? academicPosition = null;
            CareerPosition? careerPosition = null;

            if (request.AcademicPositionId.HasValue)
            {
                academicPosition = _dbContext.AcademicPositions.Include(x => x.Localizations)
                                                   .FirstOrDefault(x => x.Id == request.AcademicPositionId);

                if (academicPosition is null)
                {
                    throw new AcademicPositionException.NotFound(request.AcademicPositionId.Value);
                }
            }

            if (request.CareerPositionId.HasValue)
            {
                careerPosition = _dbContext.CareerPositions.Include(x => x.Localizations)
                                                           .FirstOrDefault(x => x.Id == request.CareerPositionId);

                if (careerPosition is null)
                {
                    throw new CareerPositionException.NotFound(request.CareerPositionId.Value);
                }
            }

            var localizations = MapLocalizationViewModelToModel(request.Localizations, employee);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                employee.AcademicPositionId = request.AcademicPositionId;
                employee.CareerPositionId = request.CareerPositionId;
                employee.Code = request.Code;
                employee.Title = request.Title;
                employee.Gender = request.Gender;
                employee.Country = request.Country;
                employee.Nationality = request.Nationality;
                employee.Religion = request.Religion;
                employee.Race = request.Race;
                employee.CitizenNo = request.CitizenNo;
                employee.UniversityEmail = request.UniversityEmail;
                employee.PersonalEmail = request.PersonalEmail;
                employee.AlternativeEmail = request.AlternativeEmail;
                employee.PhoneNumber1 = request.PhoneNumber1;
                employee.PhoneNumber2 = request.PhoneNumber2;
                employee.IsActive = request.IsActive;

                _dbContext.EmployeeLocalizations.RemoveRange(employee.Localizations);

                if (localizations.Any())
                {
                    _dbContext.EmployeeLocalizations.AddRange(localizations);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapInformationViewModel(employee,
                                                   employee.WorkInformation,
                                                   employee.Localizations);

            return response;
        }

        public EmployeeInformationViewModel UpdateEmployeeWorkInformation(Guid id, UpdateEmployeeWorkInformationViewModel request)
        {
            var employee = _dbContext.Employees.Include(x => x.AcademicPosition)
                                                    .ThenInclude(x => x.Localizations)
                                               .Include(x => x.CareerPosition)
                                                    .ThenInclude(x => x.Localizations)
                                               .Include(x => x.WorkInformation)
                                                    .ThenInclude(x => x.Faculty)
                                                        .ThenInclude(x => x.Localizations)
                                               .Include(x => x.WorkInformation)
                                                    .ThenInclude(x => x.EmployeeGroup)
                                                        .ThenInclude(x => x.Localizations)
                                               .Include(x => x.Localizations)
                                               .FirstOrDefault(x => x.Id == id);

            if (employee is null)
            {
                throw new EmployeeException.NotFound(id);
            }

            var faculty = _dbContext.Faculties.Include(x => x.Localizations)
                                              .FirstOrDefault(x => x.Id == request.FacultyId);

            if (faculty is null)
            {
                throw new FacultyException.NotFound(request.FacultyId);
            }

            var department = _dbContext.Departments.Include(x => x.Localizations)
                                                   .FirstOrDefault(x => x.Id == request.DepartmentId);

            if (department is null)
            {
                throw new DepartmentException.NotFound(request.DepartmentId);
            }

            EmployeeGroup? employeeGroup = null;

            if (request.EmployeeGroupId.HasValue)
            {
                employeeGroup = _dbContext.EmployeeGroups.Include(x => x.Localizations)
                                                         .FirstOrDefault(x => x.Id == request.EmployeeGroupId);

                if (employeeGroup is null)
                {
                    throw new EmployeeGroupException.NotFound(request.EmployeeGroupId.Value);
                }
            }

            IEnumerable<EmployeeExpertise> expertises;

            if (employee.WorkInformation is null)
            {
                var workInfo = new EmployeeWorkInformation
                {
                    EmployeeId = id,
                    FacultyId = request.FacultyId,
                    DepartmentId = request.DepartmentId,
                    EmployeeGroupId = request.EmployeeGroupId,
                    Type = request.Type,
                    OfficeRoom = request.OfficeRoom,
                    Remark = request.Remark,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "requester", // TODO : Add requester.
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = "requester" // ToDO : Add requester.
                };

                expertises = MapExpertiseViewModelToModel(request.EmployeeExpertises, workInfo);

                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    _dbContext.EmployeeWorkInformations.Add(workInfo);
                    _dbContext.EmployeeExpertises.RemoveRange();

                    if (expertises.Any())
                    {
                        _dbContext.EmployeeExpertises.AddRange(expertises);
                    }

                    transaction.Commit();
                }
            }
            else
            {
                expertises = MapExpertiseViewModelToModel(request.EmployeeExpertises, employee.WorkInformation);

                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    employee.WorkInformation.FacultyId = request.FacultyId;
                    employee.WorkInformation.DepartmentId = request.DepartmentId;
                    employee.WorkInformation.EmployeeGroupId = request.EmployeeGroupId;
                    employee.WorkInformation.Type = request.Type;
                    employee.WorkInformation.OfficeRoom = request.OfficeRoom;
                    employee.WorkInformation.Remark = request.Remark;

                    if (employee.WorkInformation.Expertises != null)
                    {
                        _dbContext.EmployeeExpertises.RemoveRange(employee.WorkInformation.Expertises);
                    }

                    if (expertises.Any())
                    {
                        _dbContext.EmployeeExpertises.AddRange(expertises);
                    }

                    transaction.Commit();
                }
            }

            _dbContext.SaveChanges();

            var response = MapInformationViewModel(employee,
                                                   employee.WorkInformation,
                                                   employee.Localizations);

            return response;
        }

        public EmployeeInformationViewModel UpdateEmployeeEducationalBackground(Guid id, IEnumerable<EmployeeEducationalBackgroundViewModel> request)
        {
            var employee = _dbContext.Employees.Include(x => x.AcademicPosition)
                                                    .ThenInclude(x => x.Localizations)
                                               .Include(x => x.CareerPosition)
                                                    .ThenInclude(x => x.Localizations)
                                               .Include(x => x.WorkInformation)
                                                    .ThenInclude(x => x.Faculty)
                                                        .ThenInclude(x => x.Localizations)
                                               .Include(x => x.WorkInformation)
                                                    .ThenInclude(x => x.EmployeeGroup)
                                                        .ThenInclude(x => x.Localizations)
                                               .Include(x => x.Localizations)
                                               .FirstOrDefault(x => x.Id == id);

            if (employee is null)
            {
                throw new EmployeeException.NotFound(id);
            }

            var educationalBackgrounds = MapEducationalBackgroundViewModelToModel(request, employee);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (employee.EducationalBackgrounds != null)
                {
                    _dbContext.EmployeeEducationalBackgrounds.RemoveRange(educationalBackgrounds);
                }

                if (educationalBackgrounds.Any())
                {
                    _dbContext.EmployeeEducationalBackgrounds.AddRange(educationalBackgrounds);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapInformationViewModel(employee,
                                                   employee.WorkInformation,
                                                   employee.Localizations);

            return response;
        }

        private static EmployeeInformationViewModel MapInformationViewModel(Employee employee,
                                                                            EmployeeWorkInformation? workInformation,
                                                                            IEnumerable<EmployeeLocalization> localizations)
        {
            var response = new EmployeeInformationViewModel
            {
                Id = employee.Id,
                AcademicPositionId = employee.AcademicPositionId,
                AcademicPositionName = employee.AcademicPosition?.Name,
                CareerPositionId = employee.CareerPositionId,
                CareerPositionName = employee.CareerPosition?.Name,
                Code = employee.Code,
                Title = employee.Title,
                Gender = employee.Gender,
                Country = employee.Country,
                Nationality = employee.Nationality,
                Religion = employee.Religion,
                Race = employee.Race,
                CitizenNo = employee.CitizenNo,
                UniversityEmail = employee.UniversityEmail,
                PersonalEmail = employee.PersonalEmail,
                AlternativeEmail = employee.AlternativeEmail,
                PhoneNumber1 = employee.PhoneNumber1,
                PhoneNumber2 = employee.PhoneNumber2,
                IsActive = employee.IsActive,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt,
                Localizations = (from locale in localizations
                                 orderby locale.Language
                                 select new EmployeeLocalizationViewModel
                                 {
                                     Language = locale.Language,
                                     FirstName = locale.FirstName,
                                     MiddleName = locale.MiddleName,
                                     LastName = locale.LastName
                                 })
                                 .ToList()
            };

            var workInfo = workInformation is null ? null
                                                   : new EmployeeWorkInformationViewModel
                                                   {
                                                       FacultyId = workInformation.FacultyId,
                                                       FacultyName = workInformation.Faculty?.Name,
                                                       DepartmentId = workInformation.DepartmentId,
                                                       DepartmentName = workInformation.Department?.Name,
                                                       EmployeeGroupId = workInformation.EmployeeGroupId,
                                                       EmployeeGroupName = workInformation.EmployeeGroup?.Name,
                                                       Type = workInformation.Type,
                                                       OfficeRoom = workInformation.OfficeRoom,
                                                       Remark = workInformation.Remark
                                                   };

            var expertises = workInformation?.Expertises is null ? Enumerable.Empty<EmployeeExpertiseViewModel>()
                                                                 : (from expertise in workInformation.Expertises
                                                                    select new EmployeeExpertiseViewModel
                                                                    {
                                                                        Type = expertise.Type,
                                                                        Major = expertise.Major,
                                                                        Minor = expertise.Minor
                                                                    })
                                                                    .ToList();

            var educationalBackgrounds = employee.EducationalBackgrounds is null ? Enumerable.Empty<EmployeeEducationalBackgroundViewModel>()
                                                                                 : (from background in employee.EducationalBackgrounds
                                                                                    select new EmployeeEducationalBackgroundViewModel
                                                                                    {
                                                                                        Institute = background.Institute,
                                                                                        DegreeLevel = background.DegreeLevel,
                                                                                        DegreeName = background.DegreeName,
                                                                                        Branch = background.Branch,
                                                                                        StartDate = background.StartDate,
                                                                                        EndDate = background.EndDate,
                                                                                        Country = background.Country
                                                                                    })
                                                                                    .ToList();

            workInfo?.EmployeeExpertises?.ToList().AddRange(expertises);
            response.WorkInformation = workInfo;
            response.EducationalBackgrounds?.ToList().AddRange(educationalBackgrounds);
            return response;
        }

        private static IEnumerable<EmployeeLocalization> MapLocalizationViewModelToModel(IEnumerable<EmployeeLocalizationViewModel>? localizations, Employee model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<EmployeeLocalization>();
            }

            var response = (from locale in localizations
                            select new EmployeeLocalization
                            {
                                Employee = model,
                                Language = locale.Language,
                                FirstName = locale.FirstName,
                                MiddleName = locale.MiddleName,
                                LastName = locale.LastName
                            })
                           .ToList();

            return response;
        }

        private static IEnumerable<EmployeeExpertise> MapExpertiseViewModelToModel(IEnumerable<EmployeeExpertiseViewModel>? expertises,
                                                                                   EmployeeWorkInformation workInformation)
        {
            if (expertises is null)
            {
                return Enumerable.Empty<EmployeeExpertise>();
            }

            var response = (from expertise in expertises
                            select new EmployeeExpertise
                            {
                                WorkInformation = workInformation,
                                Type = expertise.Type,
                                Major = expertise.Major,
                                Minor = expertise.Minor
                            })
                            .ToList();

            return response;
        }

        private static IEnumerable<EmployeeEducationalBackground> MapEducationalBackgroundViewModelToModel(IEnumerable<EmployeeEducationalBackgroundViewModel>? educationalBackgrounds, Employee model)
        {
            if (educationalBackgrounds is null)
            {
                return Enumerable.Empty<EmployeeEducationalBackground>();
            }

            var response = (from background in educationalBackgrounds
                            select new EmployeeEducationalBackground
                            {
                                Employee = model,
                                Institute = background.Institute,
                                DegreeLevel = background.DegreeLevel,
                                DegreeName = background.DegreeName,
                                Branch = background.Branch,
                                StartDate = background.StartDate,
                                EndDate = background.EndDate,
                                Country = background.Country
                            })
                            .ToList();

            return response;
        }

        private IQueryable<Employee> GenerateSearchQuery(SearchEmployeeCriteriaViewModel? parameters)
        {
            var query = _dbContext.Employees.Include(x => x.AcademicPosition)
                                                .ThenInclude(x => x.Localizations)
                                            .Include(x => x.CareerPosition)
                                                .ThenInclude(x => x.Localizations)
                                            .Include(x => x.WorkInformation)
                                                .ThenInclude(x => x.Faculty)
                                                    .ThenInclude(x => x.Localizations)
                                            .Include(x => x.WorkInformation)
                                                .ThenInclude(x => x.EmployeeGroup)
                                                    .ThenInclude(x => x.Localizations)
                                            .Include(x => x.Localizations)
                                            .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Code))
                {
                    query = query.Where(x => !string.IsNullOrEmpty(x.Code) && x.Code.Contains(parameters.Code));
                }

                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => x.FirstName.Contains(parameters.Name)
                                             || (!string.IsNullOrEmpty(x.MiddleName)
                                                 && x.MiddleName.Contains(parameters.Name))
                                             || x.LastName.Contains(parameters.Name));
                }

                if (parameters.FacultyId.HasValue)
                {
                    query = query.Where(x => x.WorkInformation != null && x.WorkInformation.FacultyId == parameters.FacultyId.Value);
                }

                if (parameters.DepartmentId.HasValue)
                {
                    query = query.Where(x => x.WorkInformation != null && x.WorkInformation.DepartmentId == parameters.DepartmentId.Value);
                }

                if (parameters.Type is not null)
                {
                    query = query.Where(x => x.WorkInformation != null && x.WorkInformation.Type == parameters.Type);
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