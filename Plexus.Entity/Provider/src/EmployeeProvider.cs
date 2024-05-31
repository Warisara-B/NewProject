using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Model;
using Plexus.Database.Model.Localization;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;
using Plexus.Utility.Extensions;
using Plexus.Entity.Exception;

namespace Plexus.Entity.Provider.src
{
    public class EmployeeProvider : IEmployeeProvider
    {
        private readonly DatabaseContext _dbContext;

        public EmployeeProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public EmployeeDTO Create(CreateEmployeeDTO request, string requester)
        {
            var model = new Employee
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
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            var address = request.Address is null ? null : MapInstructorAddress(request.Address, model, requester);

            var (status, academicLevels) = request.Status is null ? (null, null) : MapInstructorWorkStatus(request.Status, model, requester);

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Employees.Add(model);

                if (address != null)
                {
                    _dbContext.InstructorAddresses.Add(address);
                }

                if (status != null)
                {
                    _dbContext.EmployeeWorkInformations.Add(status);
                }

                if (academicLevels != null && academicLevels.Any())
                {
                    _dbContext.InstructorAcademicLevels.AddRange(academicLevels);
                }

                if (localizes.Any())
                {
                    _dbContext.EmployeeLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, address, status, academicLevels, localizes);

            return response;
        }

        public PagedViewModel<EmployeeDTO> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedInstructor = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<EmployeeDTO>
            {
                Page = pagedInstructor.Page,
                TotalPage = pagedInstructor.TotalPage,
                TotalItem = pagedInstructor.TotalItem,
                Items = (from instructor in pagedInstructor.Items
                         select MapModelToDTO(instructor, null, instructor.WorkInformation, instructor.AcademicLevels, instructor.Localizations))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<EmployeeDTO> Search(SearchCriteriaViewModel parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var instructors = query.ToList();

            var response = (from instructor in instructors
                            select MapModelToDTO(instructor, null, instructor.WorkInformation, instructor.AcademicLevels, instructor.Localizations))
                           .ToList();

            return response;
        }

        public IEnumerable<EmployeeDTO> GetById(IEnumerable<Guid> ids)
        {
            var instructors = _dbContext.Employees.Include(x => x.Localizations)
                                                    .Include(x => x.WorkInformation)
                                                    .Include(x => x.AcademicLevels)
                                                    .AsNoTracking()
                                                    .Where(x => ids.Contains(x.Id))
                                                    .ToList();

            var response = (from instructor in instructors
                            select MapModelToDTO(instructor, null, instructor.WorkInformation, instructor.AcademicLevels, instructor.Localizations))
                           .ToList();

            return response;
        }

        public EmployeeDTO GetById(Guid id)
        {
            var instructor = _dbContext.Employees.Include(x => x.Localizations)
                                                   .Include(x => x.WorkInformation)
                                                   .Include(x => x.AcademicLevels)
                                                   .AsNoTracking()
                                                   .SingleOrDefault(x => x.Id == id);

            if (instructor is null)
            {
                throw new EmployeeException.InstructorNotFound(id);
            }

            var response = MapModelToDTO(instructor, null, instructor.WorkInformation, instructor.AcademicLevels, instructor.Localizations);

            return response;
        }

        public EmployeeDTO Update(EmployeeDTO request, string requester)
        {
            var instructor = _dbContext.Employees.Include(x => x.Localizations)
                                                   .Include(x => x.WorkInformation)
                                                   .Include(x => x.AcademicLevels)
                                                   .SingleOrDefault(x => x.Id == request.Id);

            if (instructor is null)
            {
                throw new EmployeeException.InstructorNotFound(request.Id);
            }

            var localizes = MapLocalizationDTOToModel(request.Localizations, instructor).ToList();

            var address = request.Address is null ? null : MapInstructorAddress(request.Address, instructor, requester);

            var (status, academicLevels) = request.Status is null ? (null, null) : MapInstructorWorkStatus(request.Status, instructor, requester);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
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
                instructor.UpdatedAt = DateTime.UtcNow;
                instructor.UpdatedBy = requester;

                if (address != null)
                {
                    _dbContext.InstructorAddresses.Add(address);
                }

                if (instructor.WorkInformation != null)
                {
                    _dbContext.EmployeeWorkInformations.Remove(instructor.WorkInformation);
                }

                if (status != null)
                {
                    _dbContext.EmployeeWorkInformations.Add(status);
                }

                if (instructor.AcademicLevels != null && instructor.AcademicLevels.Any())
                {
                    _dbContext.InstructorAcademicLevels.RemoveRange(instructor.AcademicLevels);
                }

                if (academicLevels != null && academicLevels.Any())
                {
                    _dbContext.InstructorAcademicLevels.AddRange(academicLevels);
                }

                _dbContext.EmployeeLocalizations.RemoveRange(instructor.Localizations);

                if (localizes.Any())
                {
                    _dbContext.EmployeeLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(instructor, address, status, academicLevels, localizes);

            return response;
        }

        public void Delete(Guid id)
        {
            var instructor = _dbContext.Employees.Include(x => x.Localizations)
                                                   .SingleOrDefault(x => x.Id == id);

            if (instructor is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Employees.Remove(instructor);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public void UpdateCardImage(Guid id, string cardImagePath)
        {
            var instructor = _dbContext.Employees.SingleOrDefault(x => x.Id == id);

            if (instructor is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                instructor.CardImageUrl = cardImagePath;

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private static EmployeeDTO MapModelToDTO(Employee model, InstructorAddress? address, EmployeeWorkInformation? status,
            IEnumerable<InstructorAcademicLevel>? academicLevels, IEnumerable<EmployeeLocalization> localizations)
        {
            var response = new EmployeeDTO
            {
                Id = model.Id,
                Code = model.Code,
                Title = model.Title,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Gender = model.Gender,
                Nationality = model.Nationality,
                Race = model.Race,
                Religion = model.Religion,
                IsActive = model.IsActive,
                Address = address is null ? null : MapAddressModelToDTO(address),
                Status = status is null ? null : MapWorkStatusModelToDTO(status, academicLevels),
                CardImagePath = model.CardImageUrl,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<EmployeeLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new EmployeeLocalizationDTO
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

        private static EmployeeAddressDTO MapAddressModelToDTO(InstructorAddress model)
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
                PersonalEmailAddress = model.PersonalEmailAddress,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }

        private static EmployeeWorkStatusDTO MapWorkStatusModelToDTO(EmployeeWorkInformation model, IEnumerable<InstructorAcademicLevel>? academicLevels)
        {
            var response = new EmployeeWorkStatusDTO
            {
                AcademicLevelIds = academicLevels is null || !academicLevels.Any() ? null
                                                                                   : academicLevels.Select(x => x.AcademicLevelId)
                                                                                                   .ToList(),
                FacultyId = model.FacultyId,
                DepartmentId = model.DepartmentId,
                EmployeeGroupId = model.EmployeeGroupId,
                OfficeRoom = model.OfficeRoom,
                Remark = model.Remark,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }

        private static IEnumerable<EmployeeLocalization> MapLocalizationDTOToModel(
            IEnumerable<EmployeeLocalizationDTO>? localizations,
            Employee model)
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

        private InstructorAddress MapInstructorAddress(CreateEmployeeAddressDTO request, Employee instructor, string requester)
        {
            var response = new InstructorAddress
            {
                Instructor = instructor,
                Address = request.Address,
                Country = request.Country,
                Province = request.Province,
                District = request.District,
                SubDistrict = request.SubDistrict,
                State = request.State,
                City = request.City,
                PostalCode = request.PostalCode,
                PhoneNumber = request.PhoneNumber,
                PhoneNumber2 = request.PhoneNumber2,
                EmailAddress = request.EmailAddress,
                PersonalEmailAddress = request.PersonalEmailAddress,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            return response;
        }

        private (EmployeeWorkInformation, IEnumerable<InstructorAcademicLevel>?) MapInstructorWorkStatus(CreateEmployeeWorkStatusDTO request, Employee instructor, string requester)
        {
            var status = new EmployeeWorkInformation
            {
                Employee = instructor,
                FacultyId = request.FacultyId,
                DepartmentId = request.DepartmentId,
                EmployeeGroupId = request.EmployeeGroupId,
                OfficeRoom = request.OfficeRoom,
                Remark = request.Remark,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = requester
            };

            if (request.AcademicLevelIds is null || !request.AcademicLevelIds.Any())
            {
                return (status, null);
            }

            var academicLevels = (from levelId in request.AcademicLevelIds
                                  select new InstructorAcademicLevel
                                  {
                                      Instructor = instructor,
                                      AcademicLevelId = levelId,
                                      UpdatedAt = DateTime.UtcNow,
                                      UpdatedBy = requester
                                  })
                                 .ToList();

            return (status, academicLevels);
        }

        private IQueryable<Employee> GenerateSearchQuery(SearchCriteriaViewModel? parameters)
        {
            var query = _dbContext.Employees.Include(x => x.Localizations)
                                              .Include(x => x.WorkInformation)
                                              .Include(x => x.AcademicLevels)
                                              .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Code))
                {
                    query = query.Where(x => !string.IsNullOrEmpty(x.Code) && x.Code.Contains(parameters.Code));
                }

                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.FirstName.Contains(parameters.Keyword)
                                             || (!string.IsNullOrEmpty(x.MiddleName)
                                                 && x.MiddleName.Contains(parameters.Keyword))
                                             || x.LastName.Contains(parameters.Keyword));
                }

                if (parameters.FacultyId.HasValue)
                {
                    query = query.Where(x => x.WorkInformation != null && x.WorkInformation.FacultyId == parameters.FacultyId.Value);
                }

                if (parameters.AcademicLevelId.HasValue)
                {
                    query = query.Where(x => x.AcademicLevels != null && x.AcademicLevels.Any(y => y.AcademicLevelId == parameters.AcademicLevelId));
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