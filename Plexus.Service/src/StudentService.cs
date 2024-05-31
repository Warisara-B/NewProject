using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Student;
using Plexus.Entity.Exception;
using Plexus.Integration;
using Plexus.Service.ViewModel;

namespace Plexus.Service.src
{
    public class StudentService : IStudentService
    {
        private readonly DatabaseContext _dbContext;
        private readonly IBlobStorageProvider _storageProvider;

        public StudentService(DatabaseContext dbContext,
                              IBlobStorageProvider storageProvider)
        {
            _dbContext = dbContext;
            _storageProvider = storageProvider;
        }

        public StudentProfileCardViewModel GetStudentCardById(Guid studentId, LanguageCode language)
        {
            var student = _dbContext.Students.AsNoTracking()
                                             .Include(x => x.Localizations)
                                             .Include(x => x.Faculty)
                                                .ThenInclude(x => x.Localizations)
                                             .Include(x => x.Department)
                                                .ThenInclude(x => x.Localizations)
                                             .SingleOrDefault(x => x.Id == studentId);

            if (student is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            var locale = student.Localizations.SingleOrDefault(x => x.Language == language);

            var faculty = student.Faculty;
            var facultyLocale = faculty.Localizations.SingleOrDefault(x => x.Language == language);

            var response = new StudentProfileCardViewModel
            {
                Id = student.Id,
                FirstName = locale?.FirstName ?? student.FirstName,
                MiddleName = locale?.MiddleName ?? student.MiddleName,
                LastName = locale?.LastName ?? student.LastName,
                Code = student.Code,
                GPAX = null,
                CompletedCredit = null,
                ProfileImageUrl = student.ProfileImageUrl,
                Faculty = new StudentFacultyInformationViewModel
                {
                    LogoUrl = string.IsNullOrEmpty(faculty.LogoImagePath) ? null
                                                                          : _storageProvider.GetBlobPublicUrl(faculty.LogoImagePath),
                    FacultyName = facultyLocale?.Name ?? faculty.Name
                }
            };

            if (student.Department is not null)
            {
                var departmentLocale = student.Department.Localizations.SingleOrDefault(x => x.Language == language);
                response.Faculty.DepartmentName = departmentLocale?.Name ?? student.Department.Name;
            }

            return response;
        }

        public StudentFullProfileViewModel GetStudentFullProfileById(Guid studentId, LanguageCode language)
        {
            var student = _dbContext.Students.AsNoTracking()
                                             .Include(x => x.Localizations)
                                             .Include(x => x.Addresses)
                                             .Include(x => x.Faculty)
                                                .ThenInclude(x => x.Localizations)
                                             .Include(x => x.Department)
                                                .ThenInclude(x => x.Localizations)
                                             .SingleOrDefault(x => x.Id == studentId);

            if (student is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            var contactPersons = _dbContext.StudentGuardians.AsNoTracking()
                                                            .Include(x => x.Localizations)
                                                            .Where(x => x.StudentId == student.Id)
                                                            .ToList();

            var locale = student.Localizations.SingleOrDefault(x => x.Language == language);
            var faculty = student.Faculty;
            var facultyLocale = faculty.Localizations.SingleOrDefault(x => x.Language == language);

            var response = new StudentFullProfileViewModel
            {
                Id = student.Id,
                FirstName = locale?.FirstName ?? student.FirstName,
                MiddleName = locale?.MiddleName ?? student.MiddleName,
                LastName = locale?.LastName ?? student.LastName,
                Code = student.Code,
                ProfileImageUrl = student.ProfileImageUrl,
                Faculty = new StudentFacultyInformationViewModel
                {
                    LogoUrl = string.IsNullOrEmpty(faculty.LogoImagePath) ? null
                                                                          : _storageProvider.GetBlobPublicUrl(faculty.LogoImagePath),
                    FacultyName = facultyLocale?.Name ?? faculty.Name
                }
            };

            if (student.Department is not null)
            {
                var departmentLocale = student.Department.Localizations.SingleOrDefault(x => x.Language == language);
                response.Faculty.DepartmentName = departmentLocale?.Name ?? student.Department.Name;
            }

            // MAP CONTACT INFORMATION
            var primaryAddress = student.Addresses?.FirstOrDefault(x => x.Type.HasFlag(AddressType.PERMANENT));
            var mailingAddress = student.Addresses?.FirstOrDefault(x => x.Type.HasFlag(AddressType.MAILING));

            var primaryAddressText = primaryAddress is null ? null
                                                            : $"{primaryAddress.Address1} {primaryAddress.Address2} " +
                                                              $"{primaryAddress.District} {primaryAddress.Province} {primaryAddress.PostalCode}";
            var mailingText = mailingAddress is null ? null
                                                     : $"{mailingAddress.Address1} {mailingAddress.Address2} " +
                                                       $"{mailingAddress.District} {mailingAddress.Province} {mailingAddress.PostalCode}";

            var informations = new List<StudentInformationViewModel>
            {
                new StudentInformationViewModel { Key = StudentInformationKey.EMAIL, Value = student.UniversityEmail },
                new StudentInformationViewModel { Key = StudentInformationKey.NATIONALITY, Value = student.Nationality },
                new StudentInformationViewModel { Key = StudentInformationKey.RACE, Value = student.Race },
                new StudentInformationViewModel { Key = StudentInformationKey.RELIGION, Value = student.Religion },
                new StudentInformationViewModel { Key = StudentInformationKey.CURRENT_ADDRESS, Value = mailingText },
                new StudentInformationViewModel { Key = StudentInformationKey.OFFICIAL_ADDRESS, Value = primaryAddressText }
            };

            response.Informations = informations;

            if (contactPersons.Any())
            {
                var contacts = new List<StudentContactPersonViewModel>();

                foreach (var person in contactPersons.OrderBy(x => x.FirstName))
                {
                    var contactLocale = person.Localizations.SingleOrDefault(x => x.Language == language);

                    var contactViewModel = new StudentContactPersonViewModel
                    {
                        Id = person.Id,
                        FullName = locale is null ? $"{person.FirstName} {person.MiddleName} {person.LastName}"
                                                  : $"{locale.FirstName} {locale.MiddleName} {locale.LastName}",
                        Relationship = person.Relationship,
                        Address = null // NO ADDRESS MODEL YET
                    };
                    contacts.Add(contactViewModel);
                }

                response.ContactPersons = contacts;
            }

            return response;
        }
    }
}