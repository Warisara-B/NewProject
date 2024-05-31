using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel;
using Plexus.Database;
using Plexus.Database.Model;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Database.Model.Academic.Faculty;
using Plexus.Database.Model.Localization;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Entity.DTO.Payment;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Integration;
using Plexus.Utility;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;
using ServiceStack;

namespace Plexus.Client.src
{
    public class StudentManager : IStudentManager
    {
        private IStudentProvider _studentProvider;
        private readonly IBlobStorageProvider _blobStorageProvider;
        private readonly IAcademicLevelProvider _academicLevelProvider;
        private readonly IFacultyProvider _facultyProvider;
        private readonly IDepartmentProvider _departmentProvider;
        private readonly ICurriculumVersionProvider _curriculumVersionProvider;
        private readonly DatabaseContext _dbContext;

        public StudentManager(IStudentProvider studentProvider,
                              IBlobStorageProvider blobStorageProvider,
                              IAcademicLevelProvider academicLevelProvider,
                              IFacultyProvider facultyProvider,
                              IDepartmentProvider departmentProvider,
                              ICurriculumVersionProvider curriculumVersionProvider,
                              DatabaseContext dbContext)
        {
            _studentProvider = studentProvider;
            _blobStorageProvider = blobStorageProvider;
            _academicLevelProvider = academicLevelProvider;
            _facultyProvider = facultyProvider;
            _departmentProvider = departmentProvider;
            _curriculumVersionProvider = curriculumVersionProvider;
            _dbContext = dbContext;
        }

        public StudentViewModel Create(CreateStudentViewModel request, Guid userId)
        {
            var academicLevel = _academicLevelProvider.GetById(request.AcademicLevelId);

            var faculty = _facultyProvider.GetById(request.FacultyId);

            var department = request.DepartmentId.HasValue ? _departmentProvider.GetByFacultyId(request.FacultyId)
                                                                                .SingleOrDefault(x => x.Id == request.DepartmentId.Value)
                                                           : null;

            var version = _curriculumVersionProvider.GetById(request.CurriculumVersionId);

            var dto = new StudentDTO
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
                Localizations = MapLocalizationViewModelToDTO(request.Localizations).ToList()
            };

            UpdateStudentCode(dto);

            var student = _studentProvider.Create(dto, userId.ToString());

            var response = MapDTOToViewModel(student, academicLevel, faculty, department, version);

            return response;
        }

        public PagedViewModel<StudentViewModel> Search(SearchCriteriaViewModel parameters, int page, int pageSize)
        {
            var pagedStudents = _studentProvider.Search(parameters, page, pageSize);

            var academicLevels = _academicLevelProvider.GetAll()
                                                       .ToList();

            var faculties = _facultyProvider.GetAll()
                                            .ToList();

            var departments = _departmentProvider.GetAll()
                                                 .ToList();

            var versions = _curriculumVersionProvider.GetAll()
                                                     .ToList();

            var studentFeeTypeIds = pagedStudents.Items.Where(x => x.StudentFeeTypeId.HasValue)
                                                       .Select(x => x.StudentFeeTypeId!.Value)
                                                       .Distinct()
                                                       .ToList();

            var response = new PagedViewModel<StudentViewModel>
            {
                Page = pagedStudents.Page,
                TotalPage = pagedStudents.TotalPage,
                TotalItem = pagedStudents.TotalItem,
                Items = (from student in pagedStudents.Items
                         let academicLevel = academicLevels.SingleOrDefault(x => x.Id == student.AcademicLevelId)
                         let faculty = faculties.SingleOrDefault(x => x.Id == student.FacultyId)
                         let department = student.DepartmentId.HasValue ? departments.SingleOrDefault(x => x.Id == student.DepartmentId)
                                                                        : null
                         let version = versions.SingleOrDefault(x => x.Id == student.CurriculumVersionId)
                         select MapDTOToViewModel(student, academicLevel, faculty, department, version))
                        .ToList()
            };

            return response;
        }

        public StudentViewModel GetById(Guid id)
        {
            var student = _studentProvider.GetById(id);

            var academicLevel = _academicLevelProvider.GetById(student.AcademicLevelId);

            var faculty = _facultyProvider.GetById(student.FacultyId);

            var department = student.DepartmentId.HasValue ? _departmentProvider.GetById(student.DepartmentId.Value)
                                                           : null;

            var version = _curriculumVersionProvider.GetById(student.CurriculumVersionId);

            var response = MapDTOToViewModel(student, academicLevel, faculty, department, version);

            return response;
        }

        public StudentViewModel GetByCode(string code)
        {
            var student = _studentProvider.GetByCode(code);

            var academicLevel = _academicLevelProvider.GetById(student.AcademicLevelId);

            var faculty = _facultyProvider.GetById(student.FacultyId);

            var department = student.DepartmentId.HasValue ? _departmentProvider.GetById(student.DepartmentId.Value)
                                                           : null;

            var version = _curriculumVersionProvider.GetById(student.CurriculumVersionId);

            var response = MapDTOToViewModel(student, academicLevel, faculty, department, version);

            return response;
        }

        public IEnumerable<StudentViewModel> GetByFile(IFormFile file)
        {
            var result = CSVUtility.ReadFile(file, false);

            //CONVERT FILE TO CODE LIST
            var codes = result.Select(x => x.ElementAt(0));

            if (!codes.Any())
            {
                return Enumerable.Empty<StudentViewModel>();
            }

            var students = _studentProvider.GetByCode(codes)
                                           .ToList();

            var academicLevelIds = students.Select(x => x.AcademicLevelId)
                                           .Distinct()
                                           .ToList();

            var academicLevels = _academicLevelProvider.GetById(academicLevelIds)
                                                       .ToList();

            var facultyIds = students.Select(x => x.FacultyId)
                                     .Distinct()
                                     .ToList();

            var faculties = _facultyProvider.GetById(facultyIds)
                                            .ToList();

            var departmentIds = students.Where(x => x.DepartmentId.HasValue)
                                        .Select(x => x.DepartmentId!.Value)
                                        .Distinct()
                                        .ToList();

            var departments = _departmentProvider.GetById(departmentIds)
                                                 .ToList();

            var versionIds = students.Select(x => x.CurriculumVersionId)
                                     .Distinct()
                                     .ToList();

            var versions = _curriculumVersionProvider.GetById(versionIds)
                                                     .ToList();

            var studentFeeTypeIds = students.Where(x => x.StudentFeeTypeId.HasValue)
                                            .Select(x => x.StudentFeeTypeId!.Value)
                                            .Distinct()
                                            .ToList();

            var response = (from student in students
                            let academicLevel = academicLevels.SingleOrDefault(x => x.Id == student.AcademicLevelId)
                            let faculty = faculties.SingleOrDefault(x => x.Id == student.FacultyId)
                            let department = student.DepartmentId.HasValue ? departments.SingleOrDefault(x => x.Id == student.DepartmentId.Value)
                                                                           : null
                            let version = versions.SingleOrDefault(x => x.Id == student.CurriculumVersionId)
                            select MapDTOToViewModel(student, academicLevel, faculty, department, version))
                           .ToList();

            return response;
        }

        public StudentViewModel Update(StudentViewModel request, Guid userId)
        {
            var student = _studentProvider.GetById(request.Id);

            var academicLevel = _academicLevelProvider.GetById(student.AcademicLevelId);

            var faculty = _facultyProvider.GetById(student.FacultyId);

            var department = request.DepartmentId.HasValue ? _departmentProvider.GetByFacultyId(request.FacultyId)
                                                                                .SingleOrDefault(x => x.Id == request.DepartmentId.Value)
                                                           : null;

            var version = _curriculumVersionProvider.GetById(student.CurriculumVersionId);

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
            student.Remark = request.Remark;
            student.Localizations = MapLocalizationViewModelToDTO(request.Localizations);

            var updatedStudent = _studentProvider.Update(student, userId.ToString());

            var response = MapDTOToViewModel(updatedStudent, academicLevel, faculty, department, version);

            return response;
        }

        public void Delete(Guid id)
        {
            _studentProvider.Delete(id);
        }

        public async Task UploadCardImageAsync(Guid id, IFormFile cardImage)
        {
            var student = _studentProvider.GetById(id);

            if (cardImage is null || !cardImage.ContentType.StartsWith("image"))
            {
                return;
            }

            var cardImagePath = $"student/{student.Code}/profile.{cardImage.FileName.Split('.').Last()}";

            await _blobStorageProvider.UploadFileAsync(cardImagePath, cardImage.OpenReadStream());

            _studentProvider.UploadCardImage(id, cardImagePath);
        }

        public StudentCardViewModel GetStudentCard(Guid id)
        {
            var student = _dbContext.Students.Include(x => x.Localizations)
                                             .FirstOrDefault(x => x.Id == id);

            if (student is null)
            {
                throw new StudentException.NotFound(id);
            }

            var academicLevel = _dbContext.AcademicLevels.Include(x => x.Localizations)
                                                         .FirstOrDefault(x => x.Id == student.AcademicLevelId);

            var faculty = _dbContext.Faculties.Include(x => x.Localizations)
                                              .FirstOrDefault(x => x.Id == student.FacultyId);

            var department = _dbContext.Departments.Include(x => x.Localizations)
                                                   .FirstOrDefault(x => x.Id == student.DepartmentId);

            var curriculumVersion = _dbContext.CurriculumVersions.Include(x => x.Localizations)
                                                                 .FirstOrDefault(x => x.Id == student.CurriculumVersionId);

            var studyPlan = _dbContext.StudyPlans.FirstOrDefault(x => x.Id == student.Id);

            var studentTerm = _dbContext.StudentTerms.AsNoTracking()
                                                     .Include(x => x.Term)
                                                     .FirstOrDefault(x => x.StudentId == id
                                                                          && x.Term.AcademicLevelId == student.AcademicLevelId
                                                                          && x.Term.CollegeCalendarType == student.CurriculumVersion.CollegeCalendarType
                                                                          && x.Term.IsCurrent);

            Employee? advisor = null;

            if (studentTerm != null)
            {
                advisor = _dbContext.Employees.AsNoTracking()
                                              .Include(x => x.Localizations)
                                              .FirstOrDefault(x => x.Id == studentTerm.AdvisorId.Value);
            }

            var response = MapStudentCardViewModel(student, academicLevel, faculty, department, curriculumVersion, studyPlan, advisor);

            return response;
        }

        public StudentGeneralInfoViewModel GetStudentGeneralInfo(Guid studentId)
        {
            var student = _dbContext.Students.Include(x => x.Localizations)
                                             .Include(x => x.Passports)
                                             .Include(x => x.Deformations)
                                             .Include(x => x.StudentAcademicStatuses)
                                             .AsNoTracking()
                                             .FirstOrDefault(x => x.Id == studentId);

            if (student is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            var response = MapGeneralInfoViewModel(student, student.Passports, student.Deformations, student.Localizations);

            return response;
        }

        public StudentGeneralInfoViewModel UpdateStudentGeneralInfo(Guid studentId, CreateStudentGeneralInfoViewModel request)
        {
            var student = _dbContext.Students.Include(x => x.Localizations)
                                             .Include(x => x.Passports)
                                             .Include(x => x.Deformations)
                                             .FirstOrDefault(x => x.Id == studentId);

            if (student is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            var existingStatus = _dbContext.StudentAcademicStatuses.Where(x => x.Id == studentId)
                                                                   .ToList();

            var localizes = MapLocalizationViewModelToModel(request.Localizations, student).ToList();

            var status = MapStudentStatusViewModelToModel(request.StudentStatus, student);

            var passports = MapPassportViewModelToModel(request.Passports, student).ToList();

            var deformations = MapDeformationViewModelToModel(request.Deformations, student).ToList();

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

                if (status != null)
                {
                    _dbContext.StudentAcademicStatuses.Add(status);
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

            var response = MapGeneralInfoViewModel(student, passports, deformations, localizes);

            return response;
        }

        public StudentContactViewModel GetStudentContact(Guid studentId)
        {
            var student = _dbContext.Students.AsNoTracking()
                                             .FirstOrDefault(x => x.Id == studentId);

            if (student is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            var response = MapStudentContactInfoViewModel(student);

            return response;
        }

        public StudentContactViewModel UpdateStudentContact(Guid studentId, CreateStudentContactViewModel request)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.Id == studentId);

            if (student is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                student.UniversityEmail = request.UniversityEmail;
                student.PersonalEmail = request.PersonalEmail;
                student.AlternativeEmail = request.AlternativeEmail;
                student.Facebook = request.Facebook;
                student.Line = request.Line;
                student.Other = request.Other;
                student.PhoneNumber1 = request.PhoneNumber1;
                student.PhoneNumber2 = request.PhoneNumber2;

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapStudentContactInfoViewModel(student);

            return response;
        }

        public StudentAcademicInfoViewModel GetStudentAcademicInfo(Guid studentId)
        {
            var student = _dbContext.Students.AsNoTracking()
                                             .FirstOrDefault(x => x.Id == studentId);

            if (student is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            var academicLevel = _dbContext.AcademicLevels.Include(x => x.Localizations)
                                                         .FirstOrDefault(x => x.Id == student.AcademicLevelId);

            var academicProgram = _dbContext.AcademicPrograms.Include(x => x.Localizations)
                                                             .FirstOrDefault(x => x.Id == student.AcademicProgramId);

            var response = MapAcademicInfoViewModel(student, academicLevel, academicProgram);

            return response;
        }

        public StudentAcademicInfoViewModel UpdateStudentAcademicInfo(Guid studentId, CreateStudentAcademicInfoViewModel request)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.Id == studentId);

            if (student is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            var academicLevel = _dbContext.AcademicLevels.FirstOrDefault(x => x.Id == request.AcademicLevelId);
            var academicProgram = _dbContext.AcademicPrograms.FirstOrDefault(x => x.Id == request.AcademicProgramId);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (request.AcademicLevelId.HasValue)
                {
                    student.AcademicLevelId = request.AcademicLevelId.Value;
                }

                if (request.AcademicProgramId.HasValue)
                {
                    student.AcademicProgramId = request.AcademicProgramId;
                }

                student.PreviousCode = request.PreviousCode;
                student.Code = request.StudentCode;
                student.BatchCode = request.Batch;

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapAcademicInfoViewModel(student, academicLevel, academicProgram);

            return response;
        }

        public StudentCurriculumViewModel GetStudentCurriculumInfo(Guid studentId)
        {
            var student = _dbContext.Students.AsNoTracking()
                                             .FirstOrDefault(x => x.Id == studentId);

            if (student is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            var faculty = _dbContext.Faculties.AsNoTracking()
                                              .Include(x => x.Localizations)
                                              .FirstOrDefault(x => x.Id == student.FacultyId);

            var department = _dbContext.Departments.AsNoTracking()
                                                   .Include(x => x.Localizations)
                                                   .FirstOrDefault(x => x.Id == student.DepartmentId);

            var curriculumVersion = _dbContext.CurriculumVersions.AsNoTracking()
                                                                 .Include(x => x.Localizations)
                                                                 .FirstOrDefault(x => x.Id == student.CurriculumVersionId);

            var studyPlan = _dbContext.StudyPlans.AsNoTracking()
                                                 .FirstOrDefault(x => x.Id == student.StudyPlanId);

            var response = MapCurriculumViewModel(student, faculty, department, curriculumVersion, studyPlan);

            return response;
        }

        public StudentCurriculumViewModel UpdateStudentCurriculumInfo(Guid studentId, CreateStudentCurriculumViewModel request)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.Id == studentId);

            if (student is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            var faculty = _dbContext.Faculties.AsNoTracking()
                                              .Include(x => x.Localizations)
                                              .FirstOrDefault(x => x.Id == request.FacultyId);

            var department = _dbContext.Departments.AsNoTracking()
                                                   .Include(x => x.Localizations)
                                                   .FirstOrDefault(x => x.Id == request.DepartmentId);

            var curriculumVersion = _dbContext.CurriculumVersions.AsNoTracking()
                                                                 .Include(x => x.Localizations)
                                                                 .FirstOrDefault(x => x.Id == request.CurriculumVersionId);

            var studyPlan = _dbContext.StudyPlans.AsNoTracking()
                                                 .FirstOrDefault(x => x.Id == request.StudyPlanId);

            var log = new StudentCurriculumLog
            {
                Student = student,
                Faculty = faculty,
                Department = department,
                CurriculumVersion = curriculumVersion,
                StudyPlan = studyPlan
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                student.FacultyId = request.FacultyId;
                student.DepartmentId = request.DepartmentId;
                student.CurriculumVersionId = request.CurriculumVersionId;
                student.StudyPlanId = request.StudyPlanId;

                _dbContext.StudentCurriculumLogs.Add(log);

                transaction.Commit();
            }

            _dbContext.SaveChanges();

            var response = MapCurriculumViewModel(student, faculty, department, curriculumVersion, studyPlan);

            return response;
        }

        public IEnumerable<StudentCurriculumViewModel> GetStudentCurriculumLog(Guid studentId)
        {
            var student = _dbContext.Students.AsNoTracking()
                                             .FirstOrDefault(x => x.Id == studentId);

            if (student is null)
            {
                throw new StudentException.NotFound(studentId);
            }

            var logs = _dbContext.StudentCurriculumLogs.Include(x => x.Faculty)
                                                        .ThenInclude(x => x.Localizations)
                                                      .Include(x => x.Department)
                                                        .ThenInclude(x => x.Localizations)
                                                      .Include(x => x.CurriculumVersion)
                                                        .ThenInclude(x => x.Localizations)
                                                      .Include(x => x.StudyPlan)
                                                      .Where(x => x.StudentId == studentId)
                                                      .ToList();

            var response = (from log in logs
                            orderby log.UpdatedAt
                            select MapCurriculumViewModel(student, log.Faculty, log.Department, log.CurriculumVersion, log.StudyPlan));

            return response;

        }
        #region Private Methods

        private static IEnumerable<StudentLocalization> MapLocalizationViewModelToModel(
                       IEnumerable<StudentLocalizationViewModel>? localizations,
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

        private static StudentAcademicStatus MapStudentStatusViewModelToModel(StudentStatusViewModel? status, Student model)
        {
            if (status is null)
            {
                return null;
            }

            var response = new StudentAcademicStatus
            {
                Student = model,
                Status = status.Status,
                EffectiveDate = status.EffectiveDate,
                Remark = status.Remark
            };

            return response;
        }

        private static IEnumerable<Passport> MapPassportViewModelToModel(IEnumerable<CreatePassportViewModel>? passports, Student model)
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

        private static IEnumerable<Deformation> MapDeformationViewModelToModel(IEnumerable<CreateDeformationViewModel>? deformations, Student model)
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

        public StudentCardViewModel MapStudentCardViewModel(Student student, AcademicLevel? academicLevel, Faculty? faculty, Department? department, CurriculumVersion? curriculumVersion, StudyPlan? studyPlan, Employee? employee)
        {
            var response = new StudentCardViewModel
            {
                Id = student.Id,
                CardImageUrl = _blobStorageProvider.GetBlobPublicUrl(student.CardImageUrl),
                Code = student.Code,
                Title = student.Title,
                AcademicLevelName = academicLevel is null ? null
                                                          : academicLevel.Name,
                FacultyName = faculty is null ? null
                                              : faculty.Name,
                DepartmentName = department is null ? null
                                                    : department.Name,
                CurriculumVersionName = curriculumVersion is null ? null
                                                                  : curriculumVersion.Name,
                GPAX = student.GPA,
                CompletedCredit = student.CompletedCredit,
                StudyPlanName = studyPlan is null ? null
                                                  : studyPlan.Name,
                Advisor = employee is null ? null
                                           : new AdvisorViewModel
                                           {
                                               Id = employee.Id,
                                               Title = employee.Title,
                                               Localizations = (from locale in employee.Localizations
                                                                orderby locale.Language
                                                                select new EmployeeLocalizationViewModel
                                                                {
                                                                    Language = locale.Language,
                                                                    FirstName = locale.FirstName,
                                                                    MiddleName = locale.MiddleName,
                                                                    LastName = locale.LastName
                                                                })
                                                                .ToList()
                                           },
                IsActive = student.IsActive,
                StudentStatuses = student.StudentAcademicStatuses is null ? Enumerable.Empty<StudentStatusViewModel>()
                                                                          : (from status in student.StudentAcademicStatuses
                                                                             orderby status.EffectiveDate
                                                                             select new StudentStatusViewModel
                                                                             {
                                                                                 Status = status.Status,
                                                                                 EffectiveDate = status.EffectiveDate,
                                                                                 Remark = status.Remark
                                                                             })
                                                                             .ToList(),
                Localizations = (from locale in student.Localizations
                                 orderby locale.Language
                                 select new StudentLocalizationViewModel
                                 {
                                     Language = locale.Language,
                                     FirstName = locale.FirstName,
                                     MiddleName = locale.MiddleName,
                                     LastName = locale.LastName
                                 })
                                 .ToList()
            };

            return response;
        }

        public StudentGeneralInfoViewModel MapGeneralInfoViewModel(Student student, IEnumerable<Passport>? passports, IEnumerable<Deformation>? deformations, IEnumerable<StudentLocalization>? localizations)
        {
            var response = new StudentGeneralInfoViewModel
            {
                Id = student.Id,
                Title = student.Title,
                Gender = student.Gender,
                BirthDate = student.BirthDate,
                BirthCountry = student.BirthCountry,
                Nationality = student.Nationality,
                Religion = student.Religion,
                Race = student.Race,
                CitizenId = student.CitizenId,
                Passports = passports is null ? Enumerable.Empty<CreatePassportViewModel>()
                                              : (from passport in passports
                                                 orderby passport.IssuedAt
                                                 select new CreatePassportViewModel
                                                 {
                                                     Number = passport.Number,
                                                     IssuedAt = passport.IssuedAt,
                                                     ExpiredAt = passport.ExpiredAt,
                                                     FilePath = _blobStorageProvider.GetBlobPublicUrl(passport.FilePath),
                                                     IsActive = passport.IsActive
                                                 })
                                                 .ToList(),
                Deformations = deformations is null ? Enumerable.Empty<CreateDeformationViewModel>()
                                                    : (from deformation in student.Deformations
                                                       orderby deformation.IssuedAt
                                                       select new CreateDeformationViewModel
                                                       {
                                                           Name = deformation.Name,
                                                           BookCode = deformation.BookCode,
                                                           IssuedAt = deformation.IssuedAt,
                                                           ExpiredAt = deformation.ExpiredAt
                                                       })
                                                       .ToList(),
                StudentStatus = student.StudentAcademicStatuses is null ? Enumerable.Empty<StudentStatusViewModel>()
                                                                        : (from status in student.StudentAcademicStatuses
                                                                           orderby status.EffectiveDate descending
                                                                           select new StudentStatusViewModel
                                                                           {
                                                                               Status = status.Status,
                                                                               EffectiveDate = status.EffectiveDate,
                                                                               Remark = status.Remark
                                                                           })
                                                                           .ToList(),
                BankBranch = student.BankBranch,
                BankAccountNo = student.BankAccountNo,
                BankAccountUpdatedAt = student.BankAccountUpdatedAt,
                Remark = student.Remark,
                CreatedAt = student.CreatedAt,
                UpdatedAt = student.UpdatedAt,
                Localizations = localizations is null ? Enumerable.Empty<StudentLocalizationViewModel>()
                                                      : (from localize in student.Localizations
                                                         orderby localize.Language
                                                         select new StudentLocalizationViewModel
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

        private static StudentContactViewModel MapStudentContactInfoViewModel(Student student)
        {
            return new StudentContactViewModel
            {
                Id = student.Id,
                UniversityEmail = student.UniversityEmail,
                PersonalEmail = student.PersonalEmail,
                AlternativeEmail = student.AlternativeEmail,
                Facebook = student.Facebook,
                Line = student.Line,
                Other = student.Other,
                PhoneNumber1 = student.PhoneNumber1,
                PhoneNumber2 = student.PhoneNumber2,
            };
        }

        private static StudentAcademicInfoViewModel MapAcademicInfoViewModel(Student student, AcademicLevel? academicLevel, AcademicProgram? academicProgram)
        {
            return new StudentAcademicInfoViewModel
            {
                StudentId = student.Id,
                AcademicLevelId = student.AcademicLevelId,
                AcademicLevelName = academicLevel is null ? null
                                                          : academicLevel.Name,
                AcademicProgramId = student.AcademicProgramId,
                AcademicProgramName = academicProgram is null ? null
                                                          : academicProgram.Name,
                PreviousCode = student.PreviousCode,
                StudentCode = student.Code,
                Batch = student.BatchCode
            };
        }

        private static StudentCurriculumViewModel MapCurriculumViewModel(Student student, Faculty? faculty, Department? department, CurriculumVersion? curriculumVersion, StudyPlan? studyPlan)
        {
            return new StudentCurriculumViewModel
            {
                StudentId = student.Id,
                FacultyId = faculty is null ? Guid.Empty
                                            : faculty.Id,
                FacultyName = faculty is null ? null
                                              : faculty.Name,
                DepartmentId = department is null ? Guid.Empty
                                                  : department.Id,
                DepartmentName = department is null ? null
                                                    : department.Name,
                CurriculumVersionId = curriculumVersion is null ? Guid.Empty
                                                                : curriculumVersion.Id,
                CurriculumVersionName = curriculumVersion is null ? null
                                                                  : curriculumVersion.Name,
                StudyPlanId = studyPlan is null ? Guid.Empty
                                                : studyPlan.Id,
                StudyPlanName = studyPlan is null ? null
                                                  : studyPlan.Name,
            };
        }

        public StudentViewModel MapDTOToViewModel(StudentDTO dto, AcademicLevelDTO? academicLevel = null, FacultyDTO? faculty = null,
                                                  DepartmentDTO? department = null, CurriculumVersionDTO? version = null,
                                                  StudentFeeTypeDTO? studentFeeType = null)
        {
            var response = new StudentViewModel
            {
                Id = dto.Id,
                Code = dto.Code,
                Title = dto.Title,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                CitizenId = dto.CitizenId,
                Race = dto.Race,
                Nationality = dto.Nationality,
                Religion = dto.Religion,
                BirthDate = dto.BirthDate,
                BirthCountry = dto.BirthCountry,
                // CardImageUrl = _blobStoreageProvider.GetBlobPublicUrl(dto.CardImagePath),
                AcademicLevelId = dto.AcademicLevelId,
                CurriculumVersionId = dto.CurriculumVersionId,
                FacultyId = dto.FacultyId,
                DepartmentId = dto.DepartmentId,
                BatchCode = dto.BatchCode,
                GPA = dto.GPA,
                Remark = dto.Remark,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                AcademicLevelName = academicLevel?.Name,
                FacultyName = faculty?.Name,
                DepartmentName = department?.Name,
                CurriculumVersionName = version?.Name,
                StudentFeeTypeName = studentFeeType?.Name,
                Localizations = (from localize in dto.Localizations
                                 orderby localize.Language
                                 select new StudentLocalizationViewModel
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

        private static IEnumerable<StudentLocalizationDTO> MapLocalizationViewModelToDTO(
            IEnumerable<StudentLocalizationViewModel>? localizations)
        {
            if (localizations is null)
            {
                return Enumerable.Empty<StudentLocalizationDTO>();
            }

            var response = (from locale in localizations
                            select new StudentLocalizationDTO
                            {
                                Language = locale.Language,
                                FirstName = locale.FirstName,
                                MiddleName = locale.MiddleName,
                                LastName = locale.LastName
                            })
                           .ToList();

            return response;
        }

        private void UpdateStudentCode(StudentDTO dto)
        {
            // TODO: code generator logic
            var nextCode = _studentProvider.GetNextCode(dto.BatchCode);
            var code = $"{dto.BatchCode}{nextCode.ToString("D4")}";

            dto.Code = code;
        }

        #endregion
    }
}