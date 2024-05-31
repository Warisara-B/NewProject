using Microsoft.EntityFrameworkCore;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.Academic.Section;
using Plexus.Client.ViewModel.Registration;
using Plexus.Database;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic;
using Plexus.Database.Enum.Academic.Section;
using Plexus.Database.Model;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Database.Model.Registration;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO.Academic.Section;
using Plexus.Entity.DTO.Facility;
using Plexus.Entity.Exception;
using Plexus.Entity.Provider;
using Plexus.Entity.Provider.src.Academic;
using Plexus.Utility.Extensions;
using Plexus.Utility.ViewModel;
using Plexus.Database.Model.Academic.Section;

namespace Plexus.Client.src.Registration
{
    public class RegistrationManager : IRegistrationManager
    {
        private readonly IStudentProvider _studentProvider;
        private readonly ITermProvider _termProvider;
        private readonly ICourseProvider _courseProvider;
        private readonly ISectionProvider _sectionProvider;
        private readonly IStudyCourseProvider _studyCourseProvider;
        private readonly IGradeProvider _gradeProvider;
        private readonly IRoomProvider _roomProvider;
        private readonly IEmployeeProvider _employeeProvider;
        private readonly ICurriculumVersionProvider _curriculumVersionProvider;
        private readonly ISectionSeatProvider _sectionSeatProvider;
        private readonly ISectionManager _sectionManager;
        private readonly ISectionSeatManager _sectionSeatManager;
        private readonly IAsyncRepository<Student> _studentRepository;
        private readonly IAsyncRepository<Term> _termRepository;
        private readonly IAsyncRepository<StudyCourse> _studyCourseRepository;
        private readonly IAsyncRepository<Course> _courseRepository;
        private readonly IAsyncRepository<Section> _sectionRepository;
        private readonly IAsyncRepository<SectionSeat> _sectionSeatRepository;
        private readonly IAsyncRepository<CurriculumVersion> _curriculumVersionRepository;
        private readonly IAsyncRepository<SectionSeatUsage> _sectionSeatUsageRepository;
        private readonly IAsyncRepository<RegistrationLog> _registrationLogRepository;
        private readonly IAsyncRepository<RegistrationLogCourse> _registrationLogCourseRepository;
        private readonly IAsyncRepository<SectionExamination> _sectionExaminationRepository;
        private readonly IUnitOfWork _uow;
        private readonly DatabaseContext _dbContext;

        public RegistrationManager(IStudentProvider studentProvider,
                                   ITermProvider termProvider,
                                   ICourseProvider courseProvider,
                                   ISectionProvider sectionProvider,
                                   IStudyCourseProvider studyCourseProvider,
                                   IGradeProvider gradeProvider,
                                   IRoomProvider roomProvider,
                                   IEmployeeProvider employeeProvider,
                                   ICurriculumVersionProvider curriculumVersionProvider,
                                   ISectionSeatProvider sectionSeatProvider,
                                   ISectionManager sectionManager,
                                   ISectionSeatManager sectionSeatManager,
                                   IAsyncRepository<Student> studentRepository,
                                   IAsyncRepository<Term> termRepository,
                                   IAsyncRepository<StudyCourse> studyCourseRepository,
                                   IAsyncRepository<Course> courseRepository,
                                   IAsyncRepository<Section> sectionRepository,
                                   IAsyncRepository<SectionSeat> sectionSeatRepository,
                                   IAsyncRepository<CurriculumVersion> curriculumVersionRepository,
                                   IAsyncRepository<SectionSeatUsage> sectionSeatUsageRepository,
                                   IAsyncRepository<RegistrationLog> registrationLogRepository,
                                   IAsyncRepository<RegistrationLogCourse> registrationLogCourseRepository,
                                   IAsyncRepository<SectionExamination> sectionExaminationRepository,
                                   IUnitOfWork uow,
                                   DatabaseContext dbContext)
        {
            _studentProvider = studentProvider;
            _termProvider = termProvider;
            _courseProvider = courseProvider;
            _sectionProvider = sectionProvider;
            _studyCourseProvider = studyCourseProvider;
            _gradeProvider = gradeProvider;
            _roomProvider = roomProvider;
            _employeeProvider = employeeProvider;
            _curriculumVersionProvider = curriculumVersionProvider;
            _sectionSeatProvider = sectionSeatProvider;
            _sectionManager = sectionManager;
            _sectionSeatManager = sectionSeatManager;
            _studentRepository = studentRepository;
            _termRepository = termRepository;
            _studyCourseRepository = studyCourseRepository;
            _courseRepository = courseRepository;
            _sectionRepository = sectionRepository;
            _sectionSeatRepository = sectionSeatRepository;
            _curriculumVersionRepository = curriculumVersionRepository;
            _sectionSeatUsageRepository = sectionSeatUsageRepository;
            _registrationLogRepository = registrationLogRepository;
            _registrationLogCourseRepository = registrationLogCourseRepository;
            _sectionExaminationRepository = sectionExaminationRepository;
            _uow = uow;
            _dbContext = dbContext;
        }

        public IEnumerable<StudyCourseViewModel> GetByStudent(Guid studentId, Guid? termId, bool isIncludedTransfer)
        {
            var activeStudyStatuses = isIncludedTransfer ? StudyCourseProvider.activeStudyCourseStatus
                                                         : StudyCourseProvider.activeStudyCourseStatus.Where(x => x != StudyCourseStatus.TRANSFERED);

            var studyCourses = _studyCourseProvider.GetByStudent(studentId, termId, activeStudyStatuses);

            var student = _studentProvider.GetById(studentId);

            var courseIds = studyCourses.Select(x => x.CourseId)
                                        .Distinct()
                                        .ToList();

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            var sectionIds = studyCourses.Where(x => x.SectionId.HasValue)
                                         .Select(x => x.SectionId!.Value)
                                         .Distinct()
                                         .ToList();

            var sections = _sectionProvider.GetById(sectionIds)
                                           .ToList();

            var termIds = studyCourses.Select(x => x.TermId)
                                      .Distinct()
                                      .ToList();

            var terms = _termProvider.GetById(termIds)
                                     .ToList();

            var gradeIds = studyCourses.Where(x => x.GradeId.HasValue)
                                       .Select(x => x.GradeId!.Value)
                                       .Distinct()
                                       .ToList();

            var grades = _gradeProvider.GetById(gradeIds)
                                       .ToList();

            var sectionDetails = _sectionProvider.GetDetailBySectionId(sectionIds)
                                                 .ToList();

            var roomIds = sectionDetails.Where(x => x.RoomId.HasValue)
                                        .Select(x => x.RoomId!.Value)
                                        .Distinct()
                                        .ToList();

            var rooms = _roomProvider.GetById(roomIds)
                                     .ToList();

            var mainInstructorIds = sections.Where(x => x.MainInstructorId.HasValue)
                                            .Select(x => x.MainInstructorId!.Value)
                                            .Distinct()
                                            .ToList();

            var lecturerIds = sectionDetails.Where(x => x.InstructorId.HasValue)
                                            .Select(x => x.InstructorId!.Value)
                                            .Distinct()
                                            .ToList();

            var instructors = _employeeProvider.GetById(mainInstructorIds.Union(lecturerIds))
                                                 .ToList();

            var response = (from studyCourse in studyCourses
                            let term = terms.SingleOrDefault(x => x.Id == studyCourse.TermId)
                            let course = courses.SingleOrDefault(x => x.Id == studyCourse.CourseId)
                            let section = studyCourse.SectionId.HasValue ? sections.SingleOrDefault(x => x.Id == studyCourse.SectionId.Value)
                                                                         : null
                            let grade = studyCourse.GradeId.HasValue ? grades.SingleOrDefault(x => x.Id == studyCourse.GradeId)
                                                                     : null
                            let details = studyCourse.SectionId.HasValue ? sectionDetails.Where(x => x.SectionId == studyCourse.SectionId!.Value)
                                                                                         .ToList()
                                                                         : null
                            orderby course.Code
                            select MapDTOToViewModel(studyCourse, term, student, course, section, grade, details, rooms, instructors))
                           .ToList();

            return response;

        }

        public PagedViewModel<RegistrationLogViewModel> GetLogs(Guid studentId, Guid termId, int page = 1, int pageSize = 25)
        {
            var logs = _dbContext.RegistrationLogs.AsNoTracking()
                                                  .Where(x => x.StudentId == studentId
                                                              && x.TermId == termId)
                                                  .OrderBy(x => x.CreatedAt);

            var pagedLog = logs.GetPagedViewModel(page, pageSize);

            var logIds = pagedLog.Items.Select(x => x.Id)
                                       .ToList();

            var logCourses = _dbContext.RegistrationLogCourses.Include(x => x.StudyCourse)
                                                              .AsNoTracking()
                                                              .Where(x => logIds.Contains(x.RegistrationLogId))
                                                              .ToList();

            var courseIds = logCourses.Select(x => x.StudyCourse.CourseId)
                                      .Distinct()
                                      .ToList();

            var courses = _courseProvider.GetById(courseIds)
                                         .ToList();

            var sectionIds = logCourses.Where(x => x.StudyCourse.SectionId.HasValue)
                                       .Select(x => x.StudyCourse.SectionId!.Value)
                                       .Distinct()
                                       .ToList();

            var sections = _sectionProvider.GetById(sectionIds)
                                           .ToList();

            var response = new PagedViewModel<RegistrationLogViewModel>
            {
                Page = pagedLog.Page,
                TotalPage = pagedLog.TotalPage,
                TotalItem = pagedLog.TotalItem,
                Items = (from log in pagedLog.Items
                         let registrationCourses = logCourses.Where(x => x.RegistrationLogId == log.Id)
                                                             .ToList()
                         select MapLogToViewModel(log, registrationCourses, courses, sections))
                        .ToList()
            };

            return response;
        }

        public void Update(RegistrationViewModel request, Guid userId)
        {
            var term = _termRepository.Query()
                                      .FirstOrDefault(x => x.Id == request.TermId);

            if (term is null)
            {
                throw new TermException.NotFound(request.TermId);
            }

            // SECTION NULL MEAN, REMOVE ALL REGISTRATION COURSES
            if (request.Sections is null)
            {
                request.Sections = Enumerable.Empty<RegistrationCourseViewModel>();
            }

            // CHECK DUPLICATE SECTION IN REQUEST
            var duplicateSections = request.Sections.GroupBy(x => new { x.CourseId, x.SectionId });

            if (duplicateSections.Any(x => x.Count() > 1))
            {
                throw new RegistrationException.DuplicateSection();
            }

            var courseIds = request.Sections.Select(x => x.CourseId)
                                            .ToList();

            // STUDENT SHOULD ONLY REGISTER 1 SECTION PER COURSES            
            var duplicateCourses = courseIds.GroupBy(x => x)
                                            .Where(x => x.Count() > 1);

            if (duplicateCourses.Any())
            {
                throw new RegistrationException.DuplicateCourse();
            }

            var courses = _courseRepository.Query()
                                           .Include(x => x.Localizations)
                                           .Where(x => courseIds.Contains(x.Id))
                                           .ToList();

            foreach (var courseId in courseIds)
            {
                var matchedCourse = courses.SingleOrDefault(x => x.Id == courseId);

                if (matchedCourse is null)
                {
                    throw new CourseException.NotFound(courseId);
                }
            }

            // GET STUDY COURSES
            foreach (var studentId in request.StudentIds)
            {
                var student = _studentRepository.Query()
                                                .Include(x => x.Localizations)
                                                .FirstOrDefault(x => x.Id == studentId);

                if (student is null)
                {
                    throw new StudentException.NotFound(studentId);
                }

                var registeredStudyCourses = _studyCourseRepository.Query()
                                                                   .Where(x => x.StudentId == student.Id
                                                                          && x.TermId == term.Id
                                                                          && (x.Status == StudyCourseStatus.ACTIVE
                                                                              || x.Status == StudyCourseStatus.REGISTERED))
                                                                   .ToList();

                // GET ALL SECTION INFO, BOTH REQUEST AND ALREADY REGISTERED
                var registerSectionIds = registeredStudyCourses.Where(x => x.SectionId.HasValue)
                                                               .Select(x => x.SectionId!.Value)
                                                               .Distinct()
                                                               .ToList();

                var sectionIds = request.Sections.Where(x => x.SectionId.HasValue)
                                                 .Select(x => x.SectionId!.Value)
                                                 .ToList();

                var closedSections = _sectionRepository.Query()
                                                       .Where(x => registerSectionIds.Contains(x.Id)
                                                              && (x.Status == SectionStatus.CLOSED
                                                                  || x.Status == SectionStatus.CANCELLED))
                                                       .ToList();

                if (closedSections.Any())
                {
                    foreach (var sec in closedSections)
                    {
                        throw new RegistrationException.CloseSection(sec.Id);
                    }
                }

                sectionIds.AddRange(registerSectionIds);

                var sections = _sectionRepository.Query()
                                                 .Where(x => sectionIds.Contains(x.Id))
                                                 .ToList();

                foreach (var sectionId in sectionIds)
                {
                    var matchedSection = sections.SingleOrDefault(x => x.Id == sectionId);

                    if (matchedSection is null)
                    {
                        throw new SectionException.NotFound(sectionId);
                    }
                }

                // GET ALL SECTION SEAT, SELF + PARENT
                var parentSectionIds = sections.Where(x => x.ParentSectionId.HasValue)
                                               .Select(x => x.ParentSectionId!.Value)
                                               .Distinct()
                                               .ToList();

                sectionIds.AddRange(parentSectionIds);

                var sectionSeats = _sectionSeatRepository.Query()
                                                         .Where(x => sectionIds.Contains(x.SectionId))
                                                         .ToList();

                // NEW SECTIONS = SECTION NOT IN CURRENT REGISTER STUDY COURSES
                var newSections = (from data in request.Sections
                                   where data.SectionId.HasValue
                                   let registerdSection = registeredStudyCourses.SingleOrDefault(x => x.CourseId == data.CourseId
                                                                                                      && x.SectionId == data.SectionId)
                                   where registerdSection is null
                                   select data)
                                  .ToList();

                // DROP SECTION = SECTION NOT IN GIVEN REQUEST
                var dropStudyCourses = (from data in registeredStudyCourses
                                        let requestSection = request.Sections.SingleOrDefault(x => x.CourseId == data.CourseId
                                                                                                   && x.SectionId == data.SectionId)
                                        where requestSection is null
                                        select data)
                                       .ToList();

                // RETAIN SECTIONS = SECTION IN CURRENT STUDY COURSES AND REQUEST
                var retainStudyCourses = (from data in registeredStudyCourses
                                          let requestSection = request.Sections.SingleOrDefault(x => x.CourseId == data.CourseId
                                                                                                     && x.SectionId == data.SectionId)
                                          where requestSection is not null
                                          select data)
                                         .ToList();

                // NO UPDATE STUDY / REGISTRATION COURSES
                if (!newSections.Any() && !dropStudyCourses.Any())
                {
                    return;
                }

                var newStudyCourses = new List<StudyCourse>();
                var newSeatLogs = new List<SectionSeatUsage>();
                var dropSeatLogs = new List<SectionSeatUsage>();

                // HANDLE SEAT CALCULATION FOR NEW SECTION
                if (newSections.Any())
                {
                    var curriculumVersion = _curriculumVersionRepository.Query()
                                                                        .Include(x => x.Localizations)
                                                                        .FirstOrDefault(x => x.Id == student.CurriculumVersionId);

                    if (curriculumVersion is null)
                    {
                        throw new CurriculumVersionException.NotFound(student.CurriculumVersionId);
                    }

                    var addSectionIds = newSections.Select(x => x.SectionId!.Value)
                                                   .ToList();

                    var addSectionParentIds = sections.Where(x => addSectionIds.Contains(x.Id)
                                                               && x.ParentSectionId.HasValue)
                                                      .Select(x => x.ParentSectionId!.Value)
                                                      .ToList();

                    addSectionIds.AddRange(addSectionParentIds);

                    // GET LIST OF SECTION SEATS (INCLUDE PARENT FOR CALCULATION LOGIC)
                    var addSectionSeats = _sectionSeatManager.GetBySectionId(addSectionIds)
                                                             .ToList();

                    foreach (var section in newSections)
                    {
                        var matchedCourse = courses.Single(x => x.Id == section.CourseId);

                        if (section.SectionId.HasValue)
                        {
                            var matchedSection = sections.Single(x => x.Id == section.SectionId.Value);

                            var seats = addSectionSeats.Where(x => x.SectionId == section.SectionId.Value)
                                                       .ToList();

                            // ONLY REGISTRA CHANNEL ALLOW OVERCAPPED
                            // SO IF NOT REGISTRA, WE CAN FILTER ALL USED SEAT EXCEP LIMIT OVER HERE
                            if (request.RegistrationChannel != RegistrationChannel.REGISTRA)
                            {
                                seats = seats.Where(x => x.Type != SeatType.LIMIT
                                                         && x.TotalSeat - x.SeatUsed > 0)
                                             .ToList();
                            }

                            var registrationSeat = (from seat in seats
                                                    let score = CalculateSeatScore(seat, student, curriculumVersion)
                                                    where score >= 0
                                                    orderby score descending
                                                    select seat)
                                                   .FirstOrDefault();

                            if (registrationSeat is null)
                            {
                                throw new RegistrationException.NoAvailableSeat(matchedSection.Id);
                            }

                            var sectionDefaultSeat = addSectionSeats.Single(x => x.SectionId == section.SectionId.Value
                                                                                   && x.Type == SeatType.DEFAULT);

                            // CHECK IS RESERVE OVERCAP, IF OVER CAP REQUIRED DEDUCT DEFAULT SEAT
                            var isReservedOvercapped = registrationSeat.Type == SeatType.RESERVED
                                                       && registrationSeat.TotalSeat - registrationSeat.SeatUsed <= 0;

                            // IF SEAT IS OVER CAP OR IS OVER LIMIT
                            if (request.RegistrationChannel != RegistrationChannel.REGISTRA)
                            {
                                // SELF SEAT OVER CAPPED
                                if (registrationSeat.TotalSeat - registrationSeat.SeatUsed <= 0)
                                    throw new RegistrationException.NoAvailableSeat(matchedSection.Id);

                                // CHECK SECTION AVAILABLE (SOFT CAPPED) SEAT LIMIT
                                if ((registrationSeat.Type != SeatType.RESERVED || isReservedOvercapped)
                                    && matchedSection.AvailableSeat <= sectionDefaultSeat.SeatUsed)
                                    throw new RegistrationException.NoAvailableSeat(matchedSection.Id);
                            }

                            // GENERATE NEW STUDY COURSES
                            var registrationStudyCourse = new StudyCourse
                            {
                                TermId = term.Id,
                                StudentId = student.Id,
                                CourseId = matchedSection.CourseId,
                                SectionId = matchedSection.Id,
                                SectionSeatId = registrationSeat.Id,
                                Credit = matchedCourse.Credit,
                                RegistrationCredit = matchedCourse.RegistrationCredit,
                                Status = StudyCourseStatus.REGISTERED,
                                RegistrationChannel = request.RegistrationChannel,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = userId.ToString(),
                                UpdatedAt = DateTime.UtcNow,
                                UpdatedBy = userId.ToString()
                            };

                            newStudyCourses.Add(registrationStudyCourse);

                            // GENERATE SEAT USAGE LOGS
                            // ACTUAL REGISTRATION SEAT LOG
                            var registrationSeatUsage = new SectionSeatUsage
                            {
                                StudentId = student.Id,
                                SectionId = section.SectionId.Value,
                                SeatId = registrationSeat.Id,
                                Amount = -1,
                                StudyCourse = registrationStudyCourse,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = userId.ToString()
                            };

                            newSeatLogs.Add(registrationSeatUsage);

                            // HANDLE SELF SECTION DEFAULT SEAT
                            if (registrationSeat.Type == SeatType.LIMIT
                                || isReservedOvercapped)
                            {
                                // IF DEFAULT SEAT IS OVER CAP OR IS OVER LIMIT
                                if (request.RegistrationChannel != RegistrationChannel.REGISTRA
                                    && sectionDefaultSeat.TotalSeat - sectionDefaultSeat.SeatUsed <= 0)
                                {
                                    throw new RegistrationException.NoAvailableSeat(matchedSection.Id);
                                }

                                var defaultSeatUsage = new SectionSeatUsage
                                {
                                    StudentId = student.Id,
                                    SectionId = section.SectionId.Value,
                                    SeatId = sectionDefaultSeat.Id,
                                    Amount = -1,
                                    StudyCourse = registrationStudyCourse,
                                    ReferenceSeatId = registrationSeat.Id,
                                    CreatedAt = DateTime.UtcNow,
                                    CreatedBy = userId.ToString()
                                };

                                newSeatLogs.Add(defaultSeatUsage);
                            }

                            // IF SECTION HAS PARENT SECTION, REQUIRED DEDUCT PARENT SECTION
                            if (matchedSection.ParentSectionId.HasValue
                                && (registrationSeat.Type != SeatType.RESERVED
                                    || isReservedOvercapped))
                            {
                                var parentSection = _dbContext.Sections.AsNoTracking()
                                                                       .Single(x => x.Id == matchedSection.ParentSectionId.Value);

                                var parentDefaultSeat = addSectionSeats.Single(x => x.SectionId == matchedSection.ParentSectionId.Value
                                                                                    && x.Type == SeatType.DEFAULT);

                                // IF DEFAULT SEAT IS OVER CAP OR IS OVER SOFT CAP
                                if (request.RegistrationChannel != RegistrationChannel.REGISTRA)
                                {
                                    if (parentDefaultSeat.TotalSeat - parentDefaultSeat.SeatUsed <= 0
                                        || parentSection.AvailableSeat <= parentDefaultSeat.SeatUsed)
                                        throw new RegistrationException.NoAvailableSeat(matchedSection.Id);
                                }

                                var parentSeatUsage = new SectionSeatUsage
                                {
                                    StudentId = student.Id,
                                    SectionId = section.SectionId.Value,
                                    SeatId = parentDefaultSeat.Id,
                                    Amount = -1,
                                    StudyCourse = registrationStudyCourse,
                                    ReferenceSeatId = registrationSeat.Id,
                                    CreatedAt = DateTime.UtcNow,
                                    CreatedBy = userId.ToString()
                                };

                                newSeatLogs.Add(parentSeatUsage);
                            }
                        }
                    }
                }

                // RETURN SEAT FOR DROP SECTION
                if (dropStudyCourses.Any())
                {
                    var dropSectionIds = dropStudyCourses.Where(x => x.SectionId.HasValue)
                                                         .Select(x => x.SectionId!.Value)
                                                         .Distinct()
                                                         .ToList();

                    var dropSections = sections.Where(x => dropSectionIds.Contains(x.Id))
                                               .ToList();

                    var dropSectionParentIds = dropSections.Where(x => x.ParentSectionId.HasValue)
                                                           .Select(x => x.ParentSectionId!.Value)
                                                           .ToList();

                    dropSectionIds.AddRange(dropSectionParentIds);

                    var dropSectionSeats = sectionSeats.Where(x => dropSectionIds.Contains(x.SectionId))
                                                       .ToList();

                    foreach (var studyCourse in dropStudyCourses)
                    {
                        studyCourse.Status = StudyCourseStatus.DROP;

                        var matchingSection = dropSections.Single(x => x.Id == studyCourse.SectionId!.Value);
                        var matchingSeat = dropSectionSeats.Single(x => x.Id == studyCourse.SectionSeatId!.Value);

                        var isReturnReserveSeatOverCapped = matchingSeat.Type == SeatType.RESERVED
                                                            && matchingSeat.TotalSeat - matchingSeat.SeatUsed < 0;

                        var returnSeatUsage = new SectionSeatUsage
                        {
                            StudentId = student.Id,
                            SectionId = matchingSection.Id,
                            StudyCourseId = studyCourse.Id,
                            SeatId = matchingSeat.Id,
                            ReferenceSeatId = null,
                            Amount = 1,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = userId.ToString()
                        };

                        dropSeatLogs.Add(returnSeatUsage);

                        if (matchingSeat.Type == SeatType.LIMIT
                            || isReturnReserveSeatOverCapped)
                        {
                            var sectionDefaultSeat = dropSectionSeats.Single(x => x.SectionId == studyCourse.SectionId
                                                                                  && x.Type == SeatType.DEFAULT);

                            var returnDefaultSeatUsage = new SectionSeatUsage
                            {
                                StudentId = student.Id,
                                SectionId = matchingSection.Id,
                                StudyCourseId = studyCourse.Id,
                                SeatId = sectionDefaultSeat.Id,
                                ReferenceSeatId = matchingSeat.Id,
                                Amount = 1,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = userId.ToString()
                            };

                            dropSeatLogs.Add(returnDefaultSeatUsage);
                        }

                        if (matchingSection.ParentSectionId.HasValue
                            && (matchingSeat.Type != SeatType.RESERVED
                                || isReturnReserveSeatOverCapped))
                        {
                            var parentSectionDefaultSeat = dropSectionSeats.Single(x => x.SectionId == matchingSection.ParentSectionId.Value
                                                                                        && x.Type == SeatType.DEFAULT);

                            var returnParentDefaultSeatUsage = new SectionSeatUsage
                            {
                                StudentId = student.Id,
                                SectionId = matchingSection.Id,
                                StudyCourseId = studyCourse.Id,
                                SeatId = parentSectionDefaultSeat.Id,
                                ReferenceSeatId = matchingSeat.Id,
                                Amount = 1,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = userId.ToString()
                            };

                            dropSeatLogs.Add(returnParentDefaultSeatUsage);
                        }
                    }
                }

                // ADD REGISTRATION LOG
                var registrationLog = new RegistrationLog
                {
                    StudentId = student.Id,
                    TermId = term.Id,
                    RegistrationChannel = request.RegistrationChannel,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId.ToString()
                };

                var registrationLogAddCourses = (from studyCourse in newStudyCourses
                                                 select new RegistrationLogCourse
                                                 {
                                                     Log = registrationLog,
                                                     Action = RegistrationLogAction.ADDED,
                                                     StudyCourse = studyCourse
                                                 })
                                                .ToList();

                var registrationLogDropCourses = (from studyCourse in dropStudyCourses
                                                  select new RegistrationLogCourse
                                                  {
                                                      Log = registrationLog,
                                                      Action = RegistrationLogAction.DROPPED,
                                                      StudyCourseId = studyCourse.Id
                                                  })
                                                 .ToList();


                var registrationLogRetainCourses = (from studyCourse in retainStudyCourses
                                                    select new RegistrationLogCourse
                                                    {
                                                        Log = registrationLog,
                                                        Action = RegistrationLogAction.RETAINED,
                                                        StudyCourseId = studyCourse.Id
                                                    })
                                                   .ToList();

                var seatLogSummary = dropSeatLogs.Concat(newSeatLogs)
                                                 .ToList();

                var seatDiff = (from summary in seatLogSummary
                                group summary by summary.SeatId into groupBySeatId
                                select new
                                {
                                    SeatId = groupBySeatId.Key,
                                    Diff = groupBySeatId.Sum(x => x.Amount)
                                })
                               .ToList();

                _uow.BeginTran();

                // INSERT STUDY COURSE
                if (newStudyCourses.Any())
                {
                    _studyCourseRepository.AddRange(newStudyCourses);
                }

                // ADD SEAT LOGS
                if (newSeatLogs.Any())
                {
                    _sectionSeatUsageRepository.AddRange(newSeatLogs);
                }

                // ADD DROP SEAT LOGS
                if (dropSeatLogs.Any())
                {
                    _sectionSeatUsageRepository.AddRange(dropSeatLogs);
                }

                // UPDATE SECTION SEAT USED SNAPSHOT
                foreach (var seat in seatDiff)
                {
                    var matchingSectionSeat = sectionSeats.Single(x => x.Id == seat.SeatId);

                    matchingSectionSeat.SeatUsed += (seat.Diff * -1);
                    matchingSectionSeat.UpdatedAt = DateTime.UtcNow;
                }

                // ADD REGISTRATION LOG
                _registrationLogRepository.Add(registrationLog);

                if (registrationLogAddCourses.Any())
                {
                    _registrationLogCourseRepository.AddRange(registrationLogAddCourses);
                }

                if (registrationLogDropCourses.Any())
                {
                    _registrationLogCourseRepository.AddRange(registrationLogDropCourses);
                }

                if (registrationLogRetainCourses.Any())
                {
                    _registrationLogCourseRepository.AddRange(registrationLogRetainCourses);
                }

                _uow.Complete();
                _uow.CommitTran();
            }
        }

        public IEnumerable<SectionViewModel> VerifySectionExaminations(IEnumerable<RegistrationCourseViewModel>? sections)
        {
            if (sections is null || !sections.Any(x => x.SectionId.HasValue))
            {
                return Enumerable.Empty<SectionViewModel>();
            }

            var sectionIds = sections.Where(x => x.SectionId.HasValue)
                                     .Select(x => x.SectionId!.Value)
                                     .Distinct()
                                     .ToList();

            var sectionExaminations = _sectionExaminationRepository.Query()
                                                                   .Include(x => x.Section)
                                                                    .ThenInclude(x => x.Course)
                                                                   .Where(x => sectionIds.Contains(x.SectionId))
                                                                   .ToList();

            var grouppedExaminations = sectionExaminations.GroupBy(x => x.ExamType)
                                                          .ToList();

            var conflictSectionIds = new List<Guid>();

            foreach (var examinations in grouppedExaminations)
            {
                var examinationDates = examinations.OrderBy(x => x.Date)
                                                   .ThenBy(x => x.StartTime)
                                                   .ToList();

                var examinationDateCount = examinationDates.Count();

                for (int i = 0; i < examinationDateCount - 1; i++)
                {
                    var baseDate = examinationDates.ElementAt(i);

                    for (int j = i + 1; j < examinationDateCount; j++)
                    {
                        var compareDate = examinationDates.ElementAt(j);

                        if (baseDate.Date != compareDate.Date)
                        {
                            continue;
                        }

                        // OVERLAPPED
                        if (baseDate.StartTime < compareDate.EndTime
                            && compareDate.StartTime < baseDate.EndTime)
                        {
                            conflictSectionIds.Add(baseDate.SectionId);
                        }
                    }
                }
            }

            if (!conflictSectionIds.Any())
            {
                return Enumerable.Empty<SectionViewModel>();
            }

            var conflictSections = _sectionManager.GetByIds(conflictSectionIds);

            return conflictSections;
        }

        public void VerifyClassTimes(IEnumerable<RegistrationCourseViewModel>? sections)
        {
            if (sections is null || !sections.Any(x => x.SectionId.HasValue))
            {
                return;
            }

            var sectionIds = sections.Where(x => x.SectionId.HasValue)
                                     .Select(x => x.SectionId!.Value)
                                     .Distinct()
                                     .ToList();

            var details = _dbContext.SectionDetails.Include(x => x.Section)
                                                   .ThenInclude(x => x.Course)
                                                   .AsNoTracking()
                                                   .Where(x => sectionIds.Contains(x.SectionId))
                                                   .OrderBy(x => x.Day)
                                                   .ThenBy(x => x.StartTime)
                                                   .ToList();

            var detailsCount = details.Count();

            for (int i = 0; i < detailsCount - 1; i++)
            {
                var baseDetail = details.ElementAt(i);

                for (int j = i + 1; j < detailsCount; j++)
                {
                    var compareDetail = details.ElementAt(j);

                    if (baseDetail.Day != compareDetail.Day
                        || baseDetail.SectionId == compareDetail.SectionId)
                    {
                        continue;
                    }

                    // OVERLAPPED
                    if (baseDetail.StartTime < compareDetail.EndTime
                        && compareDetail.StartTime < baseDetail.EndTime)
                    {
                        throw new RegistrationException.ClassTimeConflict(baseDetail.Section.Course.Code, baseDetail.Section.SectionNo
                                                                          , compareDetail.Section.Course.Code, compareDetail.Section.SectionNo
                                                                          , baseDetail.Day, baseDetail.StartTime, baseDetail.EndTime);
                    }
                }
            }
        }

        private static StudyCourseViewModel MapDTOToViewModel(StudyCourseDTO dto, TermDTO term, StudentDTO student, CourseDTO course,
            SectionDTO? section = null, GradeDTO? grade = null, IEnumerable<SectionDetailDTO>? details = null, IEnumerable<RoomDTO>? rooms = null,
            IEnumerable<EmployeeDTO>? instructors = null)
        {
            var mainInstructor = section is null
                                 || !section.MainInstructorId.HasValue
                                 || instructors is null ? null
                                                        : instructors.SingleOrDefault(x => x.Id == section.MainInstructorId.Value);

            var response = new StudyCourseViewModel
            {
                Id = dto.Id,
                TermId = dto.TermId,
                StudentId = dto.StudentId,
                StudentCode = student.Code,
                FirstName = student.FirstName,
                MiddleName = student.MiddleName,
                LastName = student.LastName,
                CourseId = dto.CourseId,
                CourseCode = course.Code,
                CourseName = course.Name,
                Credit = course.Credit,
                RegistrationCredit = course.RegistrationCredit,
                LectureCredit = course.LectureCredit,
                LabCredit = course.LabCredit,
                OtherCredit = course.OtherCredit,
                MainInstructorCode = mainInstructor?.Code,
                MainInstructorFirstName = mainInstructor?.FirstName,
                MainInstructorMiddleName = mainInstructor?.MiddleName,
                MainInstructorLastName = mainInstructor?.LastName,
                SectionId = dto.SectionId,
                SectionNumber = section?.Number,
                GradeId = dto.GradeId,
                GradeLetter = grade?.Letter,
                GradeWeight = grade?.Weight,
                PaidAt = dto.PaidAt,
                GradePublishedAt = dto.GradePublishedAt,
                Status = dto.Status,
                RegistrationChannel = dto.RegistrationChannel,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                Details = details is null || !details.Any() ? null
                                                            : (from detail in details
                                                               let room = detail.RoomId.HasValue ? rooms?.SingleOrDefault(x => x.Id == detail.RoomId.Value)
                                                                                                 : null
                                                               let instructor = detail.InstructorId.HasValue ? instructors?.SingleOrDefault(x => x.Id == detail.InstructorId.Value)
                                                                                                             : null
                                                               select MapSectionDetailDTOToViewModel(detail, room, instructor))
                                                              .ToList()
            };

            return response;
        }

        private static RegistrationLogViewModel MapLogToViewModel(RegistrationLog model, IEnumerable<RegistrationLogCourse> logCourses,
            IEnumerable<CourseDTO> courses, IEnumerable<SectionDTO> sections)
        {
            var response = new RegistrationLogViewModel
            {
                RegistrationChannel = model.RegistrationChannel,
                NewCourses = (from logCourse in logCourses
                              where logCourse.Action == RegistrationLogAction.ADDED
                              let course = courses.SingleOrDefault(x => x.Id == logCourse.StudyCourse.CourseId)
                              let section = logCourse.StudyCourse.SectionId.HasValue ? sections.SingleOrDefault(x => x.Id == logCourse.StudyCourse.SectionId.Value)
                                                                                     : null
                              orderby course.Code
                              select MapLogCourseToViewModel(logCourse, course, section))
                             .ToList(),
                DropCourses = (from logCourse in logCourses
                               where logCourse.Action == RegistrationLogAction.DROPPED
                               let course = courses.SingleOrDefault(x => x.Id == logCourse.StudyCourse.CourseId)
                               let section = logCourse.StudyCourse.SectionId.HasValue ? sections.SingleOrDefault(x => x.Id == logCourse.StudyCourse.SectionId.Value)
                                                                                      : null
                               orderby course.Code
                               select MapLogCourseToViewModel(logCourse, course, section))
                              .ToList(),
                Summary = (from logCourse in logCourses
                           where logCourse.Action == RegistrationLogAction.ADDED
                                 || logCourse.Action == RegistrationLogAction.RETAINED
                           let course = courses.SingleOrDefault(x => x.Id == logCourse.StudyCourse.CourseId)
                           let section = logCourse.StudyCourse.SectionId.HasValue ? sections.SingleOrDefault(x => x.Id == logCourse.StudyCourse.SectionId.Value)
                                                                                  : null
                           orderby course.Code
                           select MapLogCourseToViewModel(logCourse, course, section))
                              .ToList(),
                CreatedAt = model.CreatedAt,
                CreatedBy = model.CreatedBy
            };

            return response;
        }

        private static RegistrationLogCourseViewModel MapLogCourseToViewModel(RegistrationLogCourse model, CourseDTO course, SectionDTO? section = null)
        {
            var response = new RegistrationLogCourseViewModel
            {
                Code = course.Code,
                LectureCredit = course.LectureCredit,
                LabCredit = course.LabCredit,
                OtherCredit = course.OtherCredit,
                SectionNumber = section?.Number
            };

            return response;
        }

        private static int CalculateSeatScore(SectionSeatViewModel seat, Student student, CurriculumVersion curriculumVersion)
        {
            if (seat.Conditions is null || !seat.Conditions.Any())
            {
                return 0;
            }

            var seatScores = new List<int>();

            foreach (var condition in seat.Conditions)
            {
                int accumulateScore = 0;

                if (condition.Codes is not null && condition.Codes.Any())
                {
                    if (!condition.Codes.Contains(student.Code))
                    {
                        continue;
                    }

                    accumulateScore += 32;
                }

                if (condition.CurriculumVersionId.HasValue)
                {
                    if (condition.CurriculumVersionId.Value != student.CurriculumVersionId)
                    {
                        continue;
                    }

                    accumulateScore += 16;
                }

                if (condition.CurriculumId.HasValue)
                {
                    if (condition.CurriculumId.Value != curriculumVersion!.CurriculumId)
                    {
                        continue;
                    }

                    accumulateScore += 8;
                }

                if (condition.DepartmentId.HasValue)
                {
                    if (condition.DepartmentId.Value != student.DepartmentId)
                    {
                        continue;
                    }

                    accumulateScore += 4;
                }

                if (condition.FacultyId.HasValue)
                {
                    if (condition.FacultyId.Value != student.FacultyId)
                    {
                        continue;
                    }

                    accumulateScore += 2;
                }

                if (condition.Batches is not null && condition.Batches.Any())
                {
                    if (!condition.Batches.ToList().Contains(student.BatchCode.Value))
                    {
                        continue;
                    }

                    accumulateScore += 1;
                }

                seatScores.Add(accumulateScore);
            }

            return seatScores.Any() ? seatScores.Max() : -1;
        }

        public static SectionDetailViewModel MapSectionDetailDTOToViewModel(SectionDetailDTO dto, RoomDTO? room, EmployeeDTO? instructor)
        {
            var response = new SectionDetailViewModel
            {
                Id = dto.Id,
                Day = dto.Day,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                InstructorId = dto.InstructorId,
                TeachingTypeId = dto.TeachingTypeId,
                Remark = dto.Remark,
                CreatedAt = dto.CreatedAt,
                RoomId = dto.RoomId,
                RoomName = room?.Name,
                InstructorCode = instructor?.Code,
                InstructorFirstName = instructor?.FirstName,
                InstructorMiddleName = instructor?.MiddleName,
                InstructorLastName = instructor?.LastName
            };

            return response;
        }
    }
}