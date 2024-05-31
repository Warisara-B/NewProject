using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Plexus.Database;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Student;
using Plexus.Database.Model;
using Plexus.Database.Model.Localization;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Student;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider.src
{
    public class StudentProvider : IStudentProvider
    {
        private readonly DatabaseContext _dbContext;

        public StudentProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public StudentDTO Create(CreateStudentDTO request, string requester)
        {
            var model = new Student
            {
                Code = request.Code,
                Title = request.Title,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Gender = request.Gender,
                CitizenId = request.CitizenId,
                Race = request.Race,
                Nationality = request.Nationality,
                Religion = request.Religion,
                BirthDate = request.BirthDate,
                BirthCountry = request.BirthCountry,
                AcademicLevelId = request.AcademicLevelId,
                CurriculumVersionId = request.CurriculumVersionId,
                FacultyId = request.FacultyId,
                DepartmentId = request.DepartmentId,
                BatchCode = request.BatchCode,
                GPA = 0,
                Remark = request.Remark,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var localizes = MapLocalizationDTOToModel(request.Localizations, model).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Students.Add(model);

                if (localizes.Any())
                {
                    _dbContext.StudentLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(model, localizes);

            return response;
        }

        public PagedViewModel<StudentDTO> Search(SearchCriteriaViewModel parameters, int page, int pageSize)
        {
            var query = GenerateSeachQuery(parameters);

            var pagedStudent = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<StudentDTO>
            {
                Page = pagedStudent.Page,
                TotalPage = pagedStudent.TotalPage,
                TotalItem = pagedStudent.TotalItem,
                Items = (from student in pagedStudent.Items
                         select MapModelToDTO(student, student.Localizations))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<StudentDTO> Search(SearchCriteriaViewModel parameters)
        {
            var query = GenerateSeachQuery(parameters);

            var students = query.ToList();

            var response = (from student in students
                            select MapModelToDTO(student, student.Localizations))
                           .ToList();

            return response;
        }

        public StudentDTO GetById(Guid id)
        {
            var student = _dbContext.Students.Include(x => x.Localizations)
                                             .AsNoTracking()
                                             .SingleOrDefault(x => x.Id == id);

            if (student is null)
            {
                throw new StudentException.NotFound(id);
            }

            var response = MapModelToDTO(student, student.Localizations);

            return response;
        }

        public StudentDTO GetByCode(string code)
        {
            var student = _dbContext.Students.Include(x => x.Localizations)
                                             .AsNoTracking()
                                             .SingleOrDefault(x => x.Code == code);

            if (student is null)
            {
                throw new StudentException.CodeNotFound(code);
            }

            var response = MapModelToDTO(student, student.Localizations);

            return response;
        }

        public IEnumerable<StudentDTO> GetById(IEnumerable<Guid> ids)
        {
            var students = _dbContext.Students.Include(x => x.Localizations)
                                              .AsNoTracking()
                                              .Where(x => ids.Contains(x.Id))
                                              .ToList(); ;

            var response = (from student in students
                            orderby student.Code
                            select MapModelToDTO(student, student.Localizations))
                           .ToList();

            return response;
        }

        public IEnumerable<StudentDTO> GetByCode(IEnumerable<string> codes)
        {
            var students = _dbContext.Students.Include(x => x.Localizations)
                                              .AsNoTracking()
                                              .Where(x => codes.Contains(x.Code))
                                              .ToList(); ;

            var response = (from student in students
                            select MapModelToDTO(student, student.Localizations))
                           .ToList();

            return response;
        }

        public StudentDTO Update(StudentDTO request, string requester)
        {
            var student = _dbContext.Students.Include(x => x.Localizations)
                                             .SingleOrDefault(x => x.Id == request.Id);

            if (student is null)
            {
                throw new StudentException.NotFound(request.Id);
            }

            var localizes = MapLocalizationDTOToModel(request.Localizations, student).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                student.Title = request.Title;
                student.FirstName = request.FirstName;
                student.MiddleName = request.MiddleName;
                student.LastName = request.LastName;
                student.Gender = request.Gender;
                student.CitizenId = request.CitizenId;
                student.Race = request.Race;
                student.Nationality = request.Nationality;
                student.Religion = request.Religion;
                student.BirthDate = request.BirthDate;
                student.BirthCountry = request.BirthCountry;
                student.AcademicLevelId = request.AcademicLevelId;
                student.CurriculumVersionId = request.CurriculumVersionId;
                student.FacultyId = request.FacultyId;
                student.DepartmentId = request.DepartmentId;
                student.BatchCode = request.BatchCode;
                student.Remark = request.Remark;
                student.UpdatedAt = DateTime.UtcNow;

                _dbContext.StudentLocalizations.RemoveRange(student.Localizations);

                if (localizes.Any())
                {
                    _dbContext.StudentLocalizations.AddRange(localizes);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToDTO(student, localizes);

            return response;
        }

        public void Delete(Guid id)
        {
            var student = _dbContext.Students.SingleOrDefault(x => x.Id == id);

            if (student is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Students.Remove(student);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public void UploadCardImage(Guid id, string cardImagePath)
        {
            var student = _dbContext.Students.SingleOrDefault(x => x.Id == id);

            if (student is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                student.CardImageUrl = cardImagePath;

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        public int GetNextCode(int batchCode)
        {
            var students = _dbContext.Students.AsNoTracking()
                                              .Where(x => x.BatchCode == batchCode)
                                              .ToList();

            if (!students.Any())
            {
                return 1;
            }

            if (int.TryParse(students.Max(x => x.Code.Substring(3, x.Code.Length - 3)), out int lastStudent))
            {
                if (lastStudent > 9999)
                {
                    throw new StudentException.InvalidStudentCode();
                }

                return lastStudent + 1;
            }

            throw new StudentException.InvalidStudentCode();
        }

        public StudentProfileCardDTO GetStudentCardById(Guid studentId)
        {
            var card = _dbContext.Students.AsNoTracking()
                                          .FirstOrDefault(x => x.Id == studentId);

            if (card is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            return MapModelToCardDTO(card);
        }

        public StudentFullProfileDTO GetStudentFullProfileById(Guid studentId)
        {
            var student = _dbContext.Students.AsNoTracking()
                                             .FirstOrDefault(x => x.Id == studentId);

            if (student is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            return MapModelToProfileDTO(student);
        }

        public StudentGeneralInfoDTO UpdateGeneralInfo(Guid studentId, UpdateStudentGeneralInfoDTO request)
        {
            var student = _dbContext.Students.AsNoTracking()
                                             .FirstOrDefault(x => x.Id == studentId);

            if (student is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            var localizes = MapLocalizationDTOToModel(request.Localizations, student).ToList();

            var passports = MapPassportDTOToModel(request.Passports, student).ToList();

            var deformations = MapDeformationDTOToModel(request.Deformations!, student).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                student.Title = request.Title;
                student.FirstName = request.FirstName;
                student.LastName = request.LastName;
                student.Gender = request.Gender;
                student.BirthDate = request.BirthDate;
                student.BirthCountry = request.BirthCountry;
                student.Nationality = request.Nationality;
                student.Religion = request.Religion;
                student.Race = request.Race;
                student.CitizenId = request.CitizenId;
                student.BankBranch = request.BankBranch;
                student.BankAccountNo = request.BankAccountNo;
                student.BankAccountUpdatedAt = request.BankAccountUpdatedAt;
                student.Remark = request.Remark;

                _dbContext.StudentLocalizations.RemoveRange(student.Localizations);

                _dbContext.Passports.RemoveRange(student.Passports!);

                _dbContext.Deformations.RemoveRange(student.Deformations!);

                if (localizes.Any())
                {
                    _dbContext.StudentLocalizations.AddRange(localizes);
                }

                if (passports.Any())
                {
                    _dbContext.Passports.AddRange(passports);
                }

                if (deformations.Any())
                {
                    _dbContext.Deformations.AddRange(deformations);
                }

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapModelToGeneralInfoDTO(student, passports, deformations, localizes);

            return response;
        }

        public static StudentDTO MapModelToDTO(Student model, IEnumerable<StudentLocalization> localizations)
        {
            var response = new StudentDTO
            {
                Id = model.Id,
                Code = model.Code,
                Title = model.Title,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Gender = model.Gender,
                CitizenId = model.CitizenId,
                Race = model.Race,
                Nationality = model.Nationality,
                Religion = model.Religion,
                BirthDate = model.BirthDate,
                BirthCountry = model.BirthCountry,
                AcademicLevelId = model.AcademicLevelId.Value,
                CurriculumVersionId = model.CurriculumVersionId,
                FacultyId = model.FacultyId,
                DepartmentId = model.DepartmentId,
                BatchCode = model.BatchCode.Value,
                Remark = model.Remark,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<StudentLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new StudentLocalizationDTO
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

        private static StudentGeneralInfoDTO MapModelToGeneralInfoDTO(Student model, IEnumerable<Passport> passports,
                                                                      IEnumerable<Deformation> deformations,
                                                                      IEnumerable<StudentLocalization> localizations)
        {
            var response = new StudentGeneralInfoDTO
            {
                Id = model.Id,
                Title = model.Title,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Gender = model.Gender,
                BirthDate = model.BirthDate,
                BirthCountry = model.BirthCountry,
                Nationality = model.Nationality,
                Religion = model.Religion,
                Race = model.Race,
                CitizenId = model.CitizenId,
                Passports = passports is null ? Enumerable.Empty<PassportDTO>()
                                              : (from passport in passports
                                                 select new PassportDTO
                                                 {
                                                     Number = passport.Number,
                                                     FilePath = passport.FilePath,
                                                     IssuedAt = passport.IssuedAt,
                                                     ExpiredAt = passport.ExpiredAt,
                                                     IsActive = passport.IsActive
                                                 })
                                                 .ToList(),
                Deformations = deformations is null ? Enumerable.Empty<DeformationDTO>()
                                                    : (from deformation in deformations
                                                       select new DeformationDTO
                                                       {
                                                           Name = deformation.Name,
                                                           BookCode = deformation.BookCode,
                                                           IssuedAt = deformation.IssuedAt,
                                                           ExpiredAt = deformation.ExpiredAt,
                                                       })
                                                       .ToList(),
                BankBranch = model.BankBranch,
                BankAccountNo = model.BankBranch,
                BankAccountUpdatedAt = model.BankAccountUpdatedAt,
                Remark = model.Remark,
                Localizations = localizations is null ? Enumerable.Empty<StudentLocalizationDTO>()
                                                      : (from localize in localizations
                                                         orderby localize.Language
                                                         select new StudentLocalizationDTO
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

        private static IEnumerable<StudentLocalization> MapLocalizationDTOToModel(
            IEnumerable<StudentLocalizationDTO>? localizations,
            Student model)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<StudentLocalization>();
            }

            var response = (from locale in localizations
                            select new StudentLocalization
                            {
                                Student = model,
                                Language = locale.Language,
                                FirstName = locale.FirstName,
                                MiddleName = locale.MiddleName,
                                LastName = locale.LastName
                            })
                           .ToList();

            return response;
        }

        private static IEnumerable<Passport> MapPassportDTOToModel(IEnumerable<PassportDTO>? passports, Student model)
        {
            if (passports is null)
            {
                return Enumerable.Empty<Passport>();
            }

            var response = (from passport in passports
                            select new Passport
                            {
                                Student = model,
                                Number = passport.Number,
                                IssuedAt = passport.IssuedAt,
                                ExpiredAt = passport.ExpiredAt,
                                FilePath = passport.FilePath,
                                IsActive = passport.IsActive
                            })
                            .ToList();

            return response;
        }

        private static IEnumerable<Deformation> MapDeformationDTOToModel(IEnumerable<DeformationDTO>? deformations, Student model)
        {
            if (deformations is null)
            {
                return Enumerable.Empty<Deformation>();
            }

            var response = (from deformation in deformations
                            select new Deformation
                            {
                                Student = model,
                                Name = deformation.Name,
                                BookCode = deformation.BookCode,
                                IssuedAt = deformation.IssuedAt,
                                ExpiredAt = deformation.ExpiredAt
                            })
                            .ToList();

            return response;
        }

        private static StudentProfileCardDTO MapModelToCardDTO(Student model)
        {
            return new StudentProfileCardDTO
            {
                Id = model.Id,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Code = model.Code,
                GPAX = null,
                CompletedCredit = null,
                ProfileImageUrl = model.CardImageUrl
            };
        }

        private static StudentFullProfileDTO MapModelToProfileDTO(Student model)
        {
            return new StudentFullProfileDTO
            {
                Id = model.Id,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Code = model.Code,
                ProfileImageUrl = model.CardImageUrl,
                Informations = MapInformationDTO(model)?.ToList(),
                ContactPersons = model.Guardians is null ? null
                                                         : (from contact in model.Guardians
                                                            select new StudentContactPersonDTO
                                                            {
                                                                Id = contact.Id,
                                                                FullName = $"{contact.FirstName} {contact.MiddleName} {contact.LastName}",
                                                                Relationship = contact.Relationship.ToString(),
                                                                Address = null // TODO: Map contact person address
                                                            }).ToList()
            };
        }
        private static IEnumerable<StudentInformationDTO>? MapInformationDTO(Student model)
        {
            return new List<StudentInformationDTO>
            {
                new StudentInformationDTO { Key = StudentInformationKey.EMAIL, Value = model.UniversityEmail },
                new StudentInformationDTO { Key = StudentInformationKey.NATIONALITY, Value = model.Nationality },
                new StudentInformationDTO { Key = StudentInformationKey.RACE, Value = model.Race },
                new StudentInformationDTO { Key = StudentInformationKey.RELIGION, Value = model.Religion },
                new StudentInformationDTO
                {
                    Key = StudentInformationKey.CURRENT_ADDRESS,
                    Value = model.Addresses?.FirstOrDefault(x => x.Type == AddressType.MAILING)?.Address1
                },
                new StudentInformationDTO
                {
                    Key = StudentInformationKey.OFFICIAL_ADDRESS,
                    Value = model.Addresses?.FirstOrDefault(x => x.Type == AddressType.OFFICIAL)?.Address1
                }
            };
        }

        private IQueryable<Student> GenerateSeachQuery(SearchCriteriaViewModel? parameters)
        {
            var query = _dbContext.Students.Include(x => x.Localizations)
                                           .Include(x => x.CurriculumVersion)
                                           .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Code))
                {
                    query = query.Where(x => x.Code.Contains(parameters.Code));
                }

                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(x => x.FirstName.Contains(parameters.Name)
                                             || x.MiddleName.Contains(parameters.Name)
                                             || x.LastName.Contains(parameters.Name));
                }

                if (parameters.AcademicLevelId.HasValue)
                {
                    query = query.Where(x => x.AcademicLevelId == parameters.AcademicLevelId.Value);
                }

                if (parameters.FacultyId.HasValue)
                {
                    query = query.Where(x => x.FacultyId == parameters.FacultyId.Value);
                }

                if (parameters.DepartmentId.HasValue)
                {
                    query = query.Where(x => x.DepartmentId == parameters.DepartmentId.Value);
                }

                if (parameters.CurriculumId.HasValue)
                {
                    query = query.Where(x => x.CurriculumVersion.CurriculumId == parameters.CurriculumId);
                }

                if (parameters.CurriculumVersionId.HasValue)
                {
                    query = query.Where(x => x.CurriculumVersionId == parameters.CurriculumVersionId);
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

