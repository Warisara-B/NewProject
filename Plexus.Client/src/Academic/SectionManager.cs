using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Database;
using Plexus.Database.Enum.Academic.Section;
using Plexus.Database.Model;
using Plexus.Database.Model.Academic;
using SectionModel = Plexus.Database.Model.Academic.Section.Section;
using Plexus.Database.Model.Academic.Section;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Database.Model.Academic.Faculty;
using Plexus.Database.Model.Facility;
using Plexus.Entity.Exception;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;
using ServiceStack;
using Plexus.Database.Enum.Academic;

namespace Plexus.Client.src.Academic
{
    public class SectionManager : ISectionManager
    {
        private readonly DatabaseContext _dbContext;
        private readonly IUnitOfWork _uow;
        private readonly IAsyncRepository<AcademicLevel> _academicLevelRepository;
        private readonly IAsyncRepository<Term> _termRepository;
        private readonly IAsyncRepository<Course> _courseRepository;
        private readonly IAsyncRepository<Campus> _campusRepository;
        private readonly IAsyncRepository<Faculty> _facultyRepository;
        private readonly IAsyncRepository<Department> _departmentRepository;
        private readonly IAsyncRepository<CurriculumVersion> _curriculumVersionRepository;
        private readonly IAsyncRepository<SectionModel> _sectionRepository;
        private readonly IAsyncRepository<SectionSeat> _sectionSeatRepository;
        private readonly IAsyncRepository<SectionSeatUsage> _sectionSeatUsageRepository;
        private readonly IAsyncRepository<SectionSchedule> _sectionScheduleRepository;
        private readonly IAsyncRepository<SectionInstructor> _sectionInstructorRepository;
        private readonly IAsyncRepository<SectionClassPeriod> _sectionClassPeriodRepository;
        private readonly IAsyncRepository<SectionClassPeriodInstructor> _sectionClassPeriodInstructorRepository;
        private readonly IAsyncRepository<SectionExamination> _sectionExaminationRepository;
        private readonly IAsyncRepository<Employee> _employeeRepository;
        private readonly IAsyncRepository<StudyCourse> _studyCourseRepository;

        public SectionManager(DatabaseContext dbContext,
                              IUnitOfWork uow,
                              IAsyncRepository<AcademicLevel> academicLevelRepository,
                              IAsyncRepository<Term> termRepository,
                              IAsyncRepository<Course> courseRepository,
                              IAsyncRepository<Campus> campusRepository,
                              IAsyncRepository<Faculty> facultyRepository,
                              IAsyncRepository<Department> departmentRepository,
                              IAsyncRepository<CurriculumVersion> curriculumVersionRepository,
                              IAsyncRepository<SectionModel> sectionRepository,
                              IAsyncRepository<SectionSeat> sectionSeatRepository,
                              IAsyncRepository<SectionSeatUsage> sectionSeatUsageRepository,
                              IAsyncRepository<SectionSchedule> sectionScheduleRepository,
                              IAsyncRepository<SectionInstructor> sectionInstructorRepository,
                              IAsyncRepository<SectionClassPeriod> sectionClassPeriodRepository,
                              IAsyncRepository<SectionClassPeriodInstructor> sectionClassPeriodInstructorRepository,
                              IAsyncRepository<SectionExamination> sectionExaminationRepository,
                              IAsyncRepository<Employee> employeeRepository,
                              IAsyncRepository<StudyCourse> studyCourseRepository)
        {
            _dbContext = dbContext;
            _uow = uow;
            _academicLevelRepository = academicLevelRepository;
            _termRepository = termRepository;
            _courseRepository = courseRepository;
            _campusRepository = campusRepository;
            _facultyRepository = facultyRepository;
            _departmentRepository = departmentRepository;
            _curriculumVersionRepository = curriculumVersionRepository;
            _sectionRepository = sectionRepository;
            _sectionSeatRepository = sectionSeatRepository;
            _sectionSeatUsageRepository = sectionSeatUsageRepository;
            _sectionScheduleRepository = sectionScheduleRepository;
            _sectionInstructorRepository = sectionInstructorRepository;
            _sectionClassPeriodRepository = sectionClassPeriodRepository;
            _sectionClassPeriodInstructorRepository = sectionClassPeriodInstructorRepository;
            _sectionExaminationRepository = sectionExaminationRepository;
            _employeeRepository = employeeRepository;
            _studyCourseRepository = studyCourseRepository;
        }

        public SectionViewModel Create(CreateSectionViewModel request)
        {
            var academicLevel = _academicLevelRepository.Query()
                                                        .Include(x => x.Localizations)
                                                        .FirstOrDefault(x => x.Id == request.AcademicLevelId);

            if (academicLevel is null)
            {
                throw new AcademicLevelException.NotFound(request.AcademicLevelId);
            }

            var term = _termRepository.Query()
                                      .FirstOrDefault(x => x.Id == request.TermId);

            if (term is null)
            {
                throw new TermException.NotFound(request.TermId);
            }

            var course = _courseRepository.Query()
                                          .Include(x => x.Localizations)
                                          .FirstOrDefault(x => x.Id == request.CourseId);

            if (course is null)
            {
                throw new CourseException.NotFound(request.CourseId);
            }

            var campus = _campusRepository.Query()
                                          .Include(x => x.Localizations)
                                          .FirstOrDefault(x => x.Id == request.CampusId);

            if (campus is null)
            {
                throw new CampusException.NotFound(request.CampusId);
            }

            Faculty? faculty = null;
            Department? department = null;
            CurriculumVersion? curriculumVersion = null;

            if (request.FacultyId.HasValue)
            {
                faculty = _facultyRepository.Query()
                                            .Include(x => x.Localizations)
                                            .FirstOrDefault(x => x.Id == request.FacultyId);

                if (faculty is null)
                {
                    throw new FacultyException.NotFound(request.FacultyId.Value);
                }
            }

            if (request.DepartmentId.HasValue)
            {
                department = _departmentRepository.Query()
                                                  .Include(x => x.Localizations)
                                                  .FirstOrDefault(x => x.Id == request.DepartmentId);

                if (department is null)
                {
                    throw new DepartmentException.NotFound(request.DepartmentId.Value);
                }
            }

            if (request.CurriculumVersionId.HasValue)
            {
                curriculumVersion = _curriculumVersionRepository.Query()
                                                                .Include(x => x.Localizations)
                                                                .FirstOrDefault(x => x.Id == request.CurriculumVersionId);
                if (curriculumVersion is null)
                {
                    throw new CurriculumVersionException.NotFound(request.CurriculumVersionId.Value);
                }
            }

            var (section, sectionSeat, seatUsage) = MapViewModelToModel(request);

            var instructors = _employeeRepository.Query()
                                                 .Where(x => request.InstructorIds.Contains(x.Id))
                                                 .ToList();

            if (instructors is null)
            {
                section.Instructors = Enumerable.Empty<SectionInstructor>();
            }

            var (jointSections, jointCourses) = MapJointSectionToModel(request.JointSections, section);
            var sectionInstructors = MapSectionInstructorToModel(instructors, section);
            var classPeriods = MapClassPeriodsToModel(request.ClassPeriods, section);
            var examination1 = MapSectionExaminationToModel(request.Examination1, section);
            var examination2 = MapSectionExaminationToModel(request.Examination2, section);
            var schedules = GenerateSchedule(section, classPeriods);

            _uow.BeginTran();
            _sectionRepository.Add(section);
            _sectionSeatRepository.Add(sectionSeat);
            _sectionSeatUsageRepository.Add(seatUsage);

            if (jointSections.Any())
            {
                _sectionRepository.AddRange(jointSections);
            }

            if (sectionInstructors.Any())
            {
                _sectionInstructorRepository.AddRange(sectionInstructors);
            }

            if (classPeriods.Any())
            {
                _sectionClassPeriodRepository.AddRange(classPeriods);

                foreach (var cls in classPeriods)
                {
                    if (cls.Instructors.Any())
                    {
                        _sectionClassPeriodInstructorRepository.AddRange(cls.Instructors);
                    }
                }
            }

            if (examination1 != null)
            {
                _sectionExaminationRepository.Add(examination1);
            }

            if (examination2 != null)
            {
                _sectionExaminationRepository.Add(examination2);
            }

            _sectionScheduleRepository.AddRange(schedules);
            _uow.Complete();
            _uow.CommitTran();

            var response = MapSectionToViewModel(section, academicLevel, term, course, campus, faculty, department, curriculumVersion, jointCourses);

            return response;
        }

        public PagedViewModel<SectionViewModel> Search(SearchSectionCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var query = GenerateSearchQuery(parameters);

            var pagedSection = query.GetPagedViewModel(page, pageSize);

            var response = new PagedViewModel<SectionViewModel>
            {
                Page = pagedSection.Page,
                PageSize = pagedSection.PageSize,
                TotalPage = pagedSection.TotalPage,
                TotalItem = pagedSection.TotalItem,
                Items = (from section in pagedSection.Items
                         select MapSectionToViewModel(section, section.AcademicLevel, section.Term, section.Course, section.Campus, section.Faculty, section.Department, section.CurriculumVersion, section.JointSections.Select(x => x.Course)))
                        .ToList()
            };

            return response;
        }

        public IEnumerable<SectionViewModel> Search(SearchSectionCriteriaViewModel parameters)
        {
            var query = GenerateSearchQuery(parameters);

            var sections = query.ToList();

            var response = (from section in sections
                            select MapSectionToViewModel(section, section.AcademicLevel, section.Term, section.Course, section.Campus, section.Faculty, section.Department, section.CurriculumVersion, section.JointSections.Select(x => x.Course)))
                            .ToList();

            return response;
        }

        public IEnumerable<SectionViewModel> GetByIds(IEnumerable<Guid> ids)
        {
            var sections = _dbContext.Sections.Include(x => x.AcademicLevel)
                                                .ThenInclude(x => x.Localizations)
                                              .Include(x => x.Term)
                                              .Include(x => x.Course)
                                                .ThenInclude(x => x.Localizations)
                                              .Include(x => x.Campus)
                                                .ThenInclude(x => x.Localizations)
                                              .Include(x => x.JointSections)
                                                .ThenInclude(x => x.Course)
                                                    .ThenInclude(x => x.Localizations)
                                              .Where(x => ids.Contains(x.Id))
                                              .ToList();

            var faculties = (from section in sections
                             select _facultyRepository.Query()
                                                      .Include(x => x.Localizations)
                                                      .FirstOrDefault(x => x.Id == section.FacultyId))
                            .ToList();

            var departments = (from section in sections
                               select _departmentRepository.Query()
                                                           .Include(x => x.Localizations)
                                                           .FirstOrDefault(x => x.Id == section.DepartmentId))
                              .ToList();

            var curriculumVersions = (from section in sections
                                      select _curriculumVersionRepository.Query()
                                                                         .Include(x => x.Localizations)
                                                                         .FirstOrDefault(x => x.Id == section.CurriculumVersionId))
                                     .ToList();

            var response = (from section in sections
                            from faculty in faculties
                            from department in departments
                            from curriculumVersion in curriculumVersions
                            select MapSectionToViewModel(section, section.AcademicLevel, section.Term, section.Course, section.Campus, faculty, department, curriculumVersion, section.JointSections.Select(x => x.Course))
                           )
                           .ToList();

            return response;
        }

        public IEnumerable<SectionDropDownViewModel> GetDropDownList(SearchSectionCriteriaViewModel parameters)
        {
            var sections = Search(parameters);

            var courseIds = sections.Select(x => x.CourseId)
                                    .Distinct()
                                    .ToList();

            var courses = _dbContext.Courses.AsNoTracking()
                                            .Include(x => x.Localizations)
                                            .Where(x => courseIds.Contains(x.Id))
                                            .ToList();

            var response = (from section in sections
                            let course = courses.SingleOrDefault(x => x.Id == section.CourseId)
                            select new SectionDropDownViewModel
                            {
                                Id = section.Id.ToString(),
                                Name = $"{course.Name} ({section.SectionNo})",
                                Number = section.SectionNo,
                                CourseId = section.CourseId,
                                TermId = section.TermId
                            })
                           .ToList();

            return response;
        }

        public void UpdateStatus(Guid id)
        {
            var section = _sectionRepository.Query()
                                            .FirstOrDefault(x => x.Id == id);

            if (section is null)
            {
                throw new SectionException.NotFound(id);
            }

            var studyCourses = _studyCourseRepository.Query()
                                                     .Where(x => x.SectionId == section.Id
                                                                 && (x.Status == StudyCourseStatus.ACTIVE
                                                                     || x.Status == StudyCourseStatus.REGISTERED))
                                                     .ToList();

            if (studyCourses.Any())
            {
                throw new SectionException.NotAllowClose(id);
            }

            if (section.Status == SectionStatus.OPEN)
            {
                section.IsClosed = true;
                section.Status = SectionStatus.CLOSED;
            }
            else
            {
                section.IsClosed = false;
                section.Status = SectionStatus.OPEN;
            }

            _uow.BeginTran();
            _sectionRepository.Update(section);
            _uow.Complete();
            _uow.CommitTran();
        }

        public void Delete(Guid id)
        {
            var section = _dbContext.Sections.FirstOrDefault(x => x.Id == id);

            if (section is null)
            {
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Sections.Remove(section);

                transaction.Commit();
            }

            _dbContext.SaveChanges();
        }

        private IEnumerable<SectionSchedule> GenerateSchedule(SectionModel section, IEnumerable<SectionClassPeriod> periods)
        {
            List<SectionSchedule> schedules = new List<SectionSchedule>();

            // CALCULATE THE TOTAL NUMBER OF DAYS OF SECTION RANGE.
            int totalDays = (section.EndedAt.Value.Date - section.StartedAt.Value.Date).Days + 1;

            for (int i = 0; i < totalDays; i++)
            {
                // GET THE DATE AND DAY OF WEEK FOR THE SPECIFIC DATE IN THE RANGE.
                var currentDate = section.StartedAt.Value.Date.AddDays(i);
                var currentDayOfWeek = currentDate.DayOfWeek;

                // FIND CLASS PERIOD FOR EACH DAY OF WEEK.
                var classPeriodsForDay = periods.Where(x => x.Day == currentDayOfWeek)
                                                .ToList();

                // GENERATE SCHEDULE ENTRIES FOR EACH CLASS PERIOD.
                foreach (var classPeriod in classPeriodsForDay)
                {
                    DateTime startAt = currentDate.Date.Add(classPeriod.StartTime);
                    DateTime endAt = currentDate.Date.Add(classPeriod.EndTime);

                    // CREATE THE SCHEDULE ENTRY.
                    SectionSchedule scheduleEntry = new SectionSchedule
                    {
                        Section = section,
                        RoomId = classPeriod.RoomId,
                        Day = currentDayOfWeek,
                        StartAt = startAt,
                        EndAt = endAt
                    };

                    schedules.Add(scheduleEntry);
                }
            }

            return schedules;
        }

        public static (SectionModel, SectionSeat, SectionSeatUsage) MapViewModelToModel(CreateSectionViewModel request)
        {
            var model = new SectionModel
            {
                AcademicLevelId = request.AcademicLevelId,
                TermId = request.TermId,
                CourseId = request.CourseId,
                CampusId = request.CampusId,
                FacultyId = request.FacultyId,
                DepartmentId = request.DepartmentId,
                CurriculumVersionId = request.CurriculumVersionId,
                SectionNo = request.SectionNo,
                SeatLimit = request.SeatLimit,
                AvailableSeat = request.AvailableSeat,
                IsWithdrawable = request.IsWithdrawable,
                IsInvisibled = request.IsInvisibled,
                IsClosed = request.IsClosed,
                Remark = request.Remark,
                Batch = request.Batch,
                StudentCodes = request.StudentCodes,
                StartedAt = request.StartedAt,
                EndedAt = request.EndedAt,
                Status = SectionStatus.OPEN,
                Type = SectionType.MASTER,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "", // TODO : Add requester
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "", // TODO : Add requester
            };

            if (model.IsClosed)
            {
                model.Status = SectionStatus.CLOSED;
            }

            var seat = new SectionSeat
            {
                Section = model,
                Name = "Default",
                Type = SeatType.DEFAULT,
                TotalSeat = request.SeatLimit,
                SeatUsed = 0,
                Conditions = null,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "", // TODO : Add requester
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "", // TODO : Add requester
            };

            var usage = new SectionSeatUsage
            {
                Section = model,
                Seat = seat,
                Amount = request.SeatLimit,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "", // TODO : Add requester
            };

            return (model, seat, usage);
        }

        private SectionViewModel MapSectionToViewModel(SectionModel model, AcademicLevel academicLevel, Term term, Course course, Campus campus,
                                                       Faculty? faculty, Department? department, CurriculumVersion? curriculumVersion,
                                                       IEnumerable<Course> jointCourses)
        {
            return new SectionViewModel
            {
                Id = model.Id,
                AcademicLevelId = model.AcademicLevelId,
                AcademicLevelName = academicLevel.Name,
                TermId = model.TermId,
                TermName = $"{term.Year}/{term.Number}",
                CourseId = model.CourseId,
                CourseCode = course.Code,
                CourseName = course.Name,
                CampusId = model.CampusId,
                CampusName = campus.Name,
                SectionNo = model.SectionNo,
                SeatLimit = model.SeatLimit,
                AvailableSeat = model.AvailableSeat,
                Instructors = model.Instructors is null ? Enumerable.Empty<SectionInstructorViewModel>()
                                                        : (from instructor in model.Instructors
                                                           select MapInstructorsToViewModel(instructor.Instructor))
                                                          .ToList(),
                IsWithdrawable = model.IsWithdrawable,
                IsInvisibled = model.IsInvisibled,
                Status = model.Status,
                IsClosed = model.IsClosed,
                Remark = model.Remark,
                FacultyId = model.FacultyId,
                FacultyName = faculty?.Name,
                DepartmentId = model.DepartmentId,
                DepartmentName = department?.Name,
                CurriculumVersionId = model.CurriculumVersionId,
                CurriculumVersionName = curriculumVersion?.Name,
                Batch = model.Batch,
                StudentCodes = model.StudentCodes,
                JointSections = model.JointSections is null ? Enumerable.Empty<JointSectionViewModel>()
                                                            : (from joint in model.JointSections
                                                               from jointCourse in jointCourses
                                                               select MapJointSectionToViewModel(joint, jointCourse))
                                                              .ToList(),
                StartedAt = model.StartedAt,
                EndedAt = model.EndedAt,
                ClassPeriods = model.SectionClassPeriods is null ? Enumerable.Empty<SectionClassPeriodViewModel>()
                                                                 : (from cls in model.SectionClassPeriods
                                                                    select MapClassPeriodToViewModel(cls, cls.Room))
                                                                   .ToList(),
                Examinations = model.SectionExaminations is null ? Enumerable.Empty<SectionExaminationViewModel>()
                                                                 : (from examination in model.SectionExaminations
                                                                    select MapExaminationToViewModel(examination))
                                                                   .ToList()
            };
        }

        private static JointSectionViewModel MapJointSectionToViewModel(SectionModel joint, Course course)
        {
            return new JointSectionViewModel
            {
                Id = joint.Id,
                ParentSectionId = joint.ParentSectionId.Value,
                Type = joint.Type,
                CourseId = joint.CourseId,
                CourseCode = course.Code,
                CourseName = course.Name,
                SectionNo = joint.SectionNo,
                SeatLimit = joint.SeatLimit,
                AvailableSeat = joint.AvailableSeat,
                Remark = joint.Remark
            };
        }

        private static SectionClassPeriodViewModel MapClassPeriodToViewModel(SectionClassPeriod sectionClassPeriod, Room? room)
        {
            return new SectionClassPeriodViewModel
            {
                Id = sectionClassPeriod.Id,
                RoomId = sectionClassPeriod.RoomId,
                RoomName = room?.Name,
                Day = sectionClassPeriod.Day,
                StartTime = sectionClassPeriod.StartTime,
                EndTime = sectionClassPeriod.EndTime,
                InstructorNames = (from instructor in sectionClassPeriod.Instructors
                                   select MapInstructorsToViewModel(instructor.Instructor))
                                   .ToList()
            };
        }

        private static SectionInstructorViewModel MapInstructorsToViewModel(Employee? instructor)
        {
            if (instructor is null)
            {
                return null;
            }

            return new SectionInstructorViewModel
            {
                Id = instructor.Id,
                FirstName = instructor.FirstName,
                MiddleName = instructor.MiddleName,
                LastName = instructor.LastName
            };
        }

        private static SectionExaminationViewModel MapExaminationToViewModel(SectionExamination examination)
        {
            return new SectionExaminationViewModel
            {
                Id = examination.Id,
                RoomId = examination.RoomId,
                RoomName = examination.Room?.Name,
                Type = examination.ExamType,
                Date = examination.Date,
                StartTime = examination.StartTime,
                EndTime = examination.EndTime
            };
        }

        private static IEnumerable<SectionInstructor> MapSectionInstructorToModel(IEnumerable<Employee>? instructors, SectionModel model)
        {
            if (instructors is null)
            {
                return Enumerable.Empty<SectionInstructor>();
            }

            var response = (from instructor in instructors
                            select new SectionInstructor
                            {
                                Section = model,
                                Instructor = instructor
                            })
                            .ToList();

            return response;
        }

        private (IEnumerable<SectionModel>, IEnumerable<Course>) MapJointSectionToModel(IEnumerable<CreateJointSectionViewModel>? jointSections, SectionModel model)
        {
            if (jointSections is null)
            {
                return (Enumerable.Empty<SectionModel>(), Enumerable.Empty<Course>());
            }

            var courses = (from joint in jointSections
                           select _dbContext.Courses.AsNoTracking()
                                                    .Include(x => x.Localizations)
                                                    .FirstOrDefault(x => x.Id == joint.CourseId))
                           .ToList();

            var response = (from joint in jointSections
                            select new SectionModel
                            {
                                ParentSection = model,
                                AcademicLevelId = model.AcademicLevelId,
                                TermId = model.TermId,
                                CourseId = joint.CourseId,
                                CampusId = model.CampusId,
                                FacultyId = model.FacultyId,
                                DepartmentId = model.DepartmentId,
                                CurriculumVersionId = model.CurriculumVersionId,
                                SectionNo = joint.SectionNo,
                                SeatLimit = joint.SeatLimit,
                                AvailableSeat = joint.AvailableSeat,
                                Instructors = model.Instructors,
                                IsWithdrawable = model.IsWithdrawable,
                                IsInvisibled = model.IsInvisibled,
                                IsClosed = model.IsClosed,
                                Status = model.Status,
                                Type = SectionType.JOINT,
                                Remark = joint.Remark,
                                Batch = model.Batch,
                                StudentCodes = model.StudentCodes,
                                StartedAt = model.StartedAt,
                                EndedAt = model.EndedAt,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = "", // TODO : Add requester
                                UpdatedAt = DateTime.UtcNow,
                                UpdatedBy = "" // TODO : Add requester
                            })
                            .ToList();

            return (response, courses);
        }

        private IEnumerable<SectionClassPeriod> MapClassPeriodsToModel(IEnumerable<CreateSectionClassPeriodViewModel>? classes, SectionModel model)
        {
            if (classes is null)
            {
                return Enumerable.Empty<SectionClassPeriod>();
            }

            var response = new List<SectionClassPeriod>();

            foreach (var cls in classes)
            {
                var classPeriod = new SectionClassPeriod
                {
                    Section = model,
                    RoomId = cls.RoomId,
                    Day = cls.Day,
                    StartTime = cls.StartTime,
                    EndTime = cls.EndTime,
                };

                classPeriod.Instructors = MapClassPeriodInstructorsToModel(cls.InstructorIds, classPeriod);

                response.Add(classPeriod);
            }

            return response;
        }

        private IEnumerable<SectionClassPeriodInstructor> MapClassPeriodInstructorsToModel(IEnumerable<Guid>? instructorIds, SectionClassPeriod model)
        {
            if (instructorIds is null)
            {
                return Enumerable.Empty<SectionClassPeriodInstructor>();
            }

            var instructors = _dbContext.Employees.Include(x => x.Localizations)
                                                  .Where(x => instructorIds.Contains(x.Id))
                                                  .ToList();

            var response = (from instructor in instructors
                            select new SectionClassPeriodInstructor
                            {
                                SectionClassPeriod = model,
                                Instructor = instructor
                            })
                            .ToList();

            return response;
        }

        private static SectionExamination MapSectionExaminationToModel(CreateSectionExaminationViewModel examination, SectionModel model)
        {
            return new SectionExamination
            {
                Section = model,
                RoomId = examination.RoomId,
                ExamType = examination.Type,
                Date = examination.Date,
                StartTime = examination.StartTime,
                EndTime = examination.EndTime,
                IsActive = examination.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "", // TODO : Add requester.
            };
        }

        private IQueryable<SectionModel> GenerateSearchQuery(SearchSectionCriteriaViewModel? parameters = null)
        {
            var query = _dbContext.Sections.Include(x => x.AcademicLevel)
                                            .ThenInclude(x => x.Localizations)
                                           .Include(x => x.Term)
                                           .Include(x => x.Course)
                                            .ThenInclude(x => x.Localizations)
                                           .Include(x => x.Campus)
                                            .ThenInclude(x => x.Localizations)
                                           .Include(x => x.Faculty)
                                           .Include(x => x.JointSections)
                                            .ThenInclude(x => x.Course)
                                                .ThenInclude(x => x.Localizations)
                                           .AsNoTracking();

            if (parameters is not null)
            {
                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.Course.Name.Contains(parameters.Keyword)
                                        || x.Course.Code.Contains(parameters.Keyword));
                }

                if (parameters.AcademicLevelId.HasValue)
                {
                    query = query.Where(x => x.AcademicLevelId == parameters.AcademicLevelId);
                }

                if (parameters.TermId.HasValue)
                {
                    query = query.Where(x => x.TermId == parameters.TermId);
                }

                if (parameters.FacultyId.HasValue)
                {
                    query = query.Where(x => x.FacultyId == parameters.FacultyId);
                }

                if (parameters.DepartmentId.HasValue)
                {
                    query = query.Where(x => x.DepartmentId == parameters.DepartmentId);
                }

                if (parameters.InstructorId.HasValue)
                {
                    query = query.Where(x => x.Instructors.Any(instructor => instructor.InstructorId == parameters.InstructorId));
                }

                if (parameters.SectionStatus is not null)
                {
                    query = query.Where(x => x.Status == parameters.SectionStatus);
                }

                if (parameters.SectionType is not null)
                {
                    query = query.Where(x => x.Type == parameters.SectionType);
                }
            }

            query = query.OrderBy(x => x.UpdatedAt);

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