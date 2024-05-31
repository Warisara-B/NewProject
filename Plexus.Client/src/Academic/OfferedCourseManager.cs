// using Plexus.Client.src.Academic.Section;
// using Plexus.Client.ViewModel;
// using Plexus.Client.ViewModel.Academic;
// using Plexus.Client.ViewModel.Academic.Section;
// using Plexus.Database.Enum.Academic.Section;
// using Plexus.Database.Model.Academic.Curriculum;
// using Plexus.Database.Model.Academic.Faculty;
// using Plexus.Entity.DTO;
// using Plexus.Entity.DTO.Academic;
// using Plexus.Entity.DTO.Academic.Curriculum;
// using Plexus.Entity.DTO.Academic.Section;
// using Plexus.Entity.DTO.Facility;
// using Plexus.Entity.Exception;
// using Plexus.Entity.Provider;
// using Plexus.Utility.Extensions;
// using Plexus.Utility.ViewModel;

// namespace Plexus.Client.src.Academic
// {
//     public class OfferedCourseManager : IOfferedCourseManager
//     {
//         private ISectionProvider _sectionProvider;
//         private ISectionSeatProvider _sectionSeatProvider;
//         private IOfferedCourseProvider _offeredCourseProvider;
//         private ITermProvider _termProvider;
//         private ICurriculumProvider _curriculumProvider;
//         private ICurriculumVersionProvider _curriculumVersionProvider;
//         private IFacultyProvider _facultyProvider;
//         private IDepartmentProvider _departmentProvider;
//         private IRoomProvider _roomProvider;
//         private IEmployeeProvider _employeeProvider;
//         private ICourseProvider _courseProvider;
//         private IAcademicLevelProvider _academicLevelProvider;
//         private IStudentManager _studentManager;

//         public OfferedCourseManager(IOfferedCourseProvider offeredCourseProvider,
//                                     ICurriculumProvider curriculumProvider,
//                                     ICurriculumVersionProvider curriculumVersionProvider,
//                                     IFacultyProvider facultyProvider,
//                                     IDepartmentProvider departmentProvider,
//                                     ITermProvider termProvider,
//                                     IRoomProvider roomProvider,
//                                     IEmployeeProvider employeeProvider,
//                                     ICourseProvider courseProvider,
//                                     ISectionProvider sectionProvider,
//                                     ISectionSeatProvider sectionSeatProvider,
//                                     IAcademicLevelProvider academicLevelProvider,
//                                     IStudentManager studentManager)
//         {
//             _offeredCourseProvider = offeredCourseProvider;
//             _termProvider = termProvider;
//             _curriculumProvider = curriculumProvider;
//             _curriculumVersionProvider = curriculumVersionProvider;
//             _facultyProvider = facultyProvider;
//             _departmentProvider = departmentProvider;
//             _roomProvider = roomProvider;
//             _employeeProvider = employeeProvider;
//             _courseProvider = courseProvider;
//             _sectionProvider = sectionProvider;
//             _sectionSeatProvider = sectionSeatProvider;
//             _academicLevelProvider = academicLevelProvider;
//             _studentManager = studentManager;
//         }

//         public OfferedCourseViewModel Create(CreateOfferedCourseViewModel request, Guid userId)
//         {
//             // CHECK FOR NESTED JOINT
//             if (request.ParentSectionId.HasValue
//                 && request.JointSections is not null
//                 && request.JointSections.Any())
//             {
//                 throw new SectionException.NotAllowNestedJoint();
//             }

//             // CHECK SELF OVER RESERVED
//             if (request.Seats is not null && request.Seats.Any())
//             {
//                 var reservedSeats = request.Seats.Where(x => x.Type == SeatType.RESERVED)
//                                                  .Sum(x => x.TotalSeat);

//                 if (reservedSeats > request.SeatLimit)
//                 {
//                     throw new OfferedCourseException.NotEnoughSeats(request.SeatLimit);
//                 }

//                 if (request.Seats.Any(x => x.Type == SeatType.DEFAULT))
//                 {
//                     throw new SectionException.NotAllowCreateDefaultSeat();
//                 }
//             }

//             // CHECK JOINT SECTION OVERSEAT AND DUPLICATE COURSE
//             if (request.JointSections is not null && request.JointSections.Any())
//             {
//                 if (request.JointSections.Any(x => x.SeatLimit >= request.SeatLimit))
//                 {
//                     throw new OfferedCourseException.JointNotEnoughSeats(request.SeatLimit);
//                 }

//                 var duplicateJointCourse = request.JointSections.GroupBy(x => new { x.CourseId, x.Number })
//                                                                 .Where(x => x.Count() > 1)
//                                                                 .ToList();

//                 if (duplicateJointCourse.Any())
//                 {
//                     throw new OfferedCourseException.JointDuplicate();
//                 }
//             }

//             // VALIDATE GIVEN TERM, COURSE AND MAIN INSTRUCTOR VALID
//             var (term, courses, instructors, courseIds) = ValidateRequest(request, request.JointSections, request.Details);

//             var sections = _sectionProvider.GetByTermIdAndCourseId(request.TermId, courseIds)
//                                            .ToList();

//             if (sections.Any(x => x.Number == request.Number))
//             {
//                 throw new SectionException.Duplicate(request.Number);
//             }

//             // VALIDATE SEAT CONDITIONS
//             var (curriculums, curriculumVersions, faculties, departments) = ValidateSeatConditions(request.Seats);

//             // SECTION SEATS
//             var sectionSeatDTO = request.Seats is null || !request.Seats.Any() ? Enumerable.Empty<UpsertSectionSeatDTO>()
//                                                                                : (from seat in request.Seats
//                                                                                   select SectionSeatManager.MapViewModelToDTO(seat))
//                                                                                  .ToList();

//             // SECTION DETAILS
//             var rooms = ValidateSectionDetails(request.Details, request.Examinations).ToList();

//             var lecturers = Enumerable.Empty<EmployeeDTO>();

//             var details = request.Details is null || !request.Details.Any() ? Enumerable.Empty<UpdateSectionDetailDTO>()
//                                                                             : SectionManager.MapDetailViewModelToDTO(request.Details);

//             var examinations = request.Examinations is null || !request.Examinations.Any() ? Enumerable.Empty<UpdateSectionExaminationDTO>()
//                                                                                            : SectionManager.MapExaminationViewModelToDTO(request.Examinations);

//             var dto = new CreateOfferedCourseDTO
//             {
//                 CourseId = request.CourseId,
//                 TermId = request.TermId,
//                 Number = request.Number,
//                 SeatLimit = request.SeatLimit,
//                 PlanningSeat = request.PlanningSeat,
//                 MinimumSeat = request.MinimumSeat,
//                 MainInstructorId = request.MainInstructorId,
//                 Status = request.Status,
//                 IsWithdrawable = request.IsWithdrawable,
//                 IsGhostSection = request.IsGhostSection,
//                 IsOutboundSection = request.IsOutboundSection,
//                 Remark = request.Remark,
//                 AvailableSeat = request.SeatLimit,
//                 StartedDate = request.StartedDate,
//                 TotalWeeks = request.TotalWeeks,
//                 ParentSectionId = request.ParentSectionId,
//                 IsClosed = request.IsClosed,
//                 Seats = sectionSeatDTO,
//                 Details = details,
//                 Examinations = examinations
//             };

//             var offeredCourse = _offeredCourseProvider.Create(dto, userId.ToString());

//             if (request.JointSections is not null && request.JointSections.Any())
//             {
//                 var jointSections = new List<JointSectionDTO>();

//                 foreach (var joint in request.JointSections)
//                 {
//                     var jointDTO = new CreateOfferedCourseDTO
//                     {
//                         CourseId = joint.CourseId,
//                         TermId = request.TermId,
//                         Number = joint.Number,
//                         SeatLimit = joint.SeatLimit,
//                         PlanningSeat = 0,
//                         MinimumSeat = 0,
//                         MainInstructorId = request.MainInstructorId,
//                         Status = request.Status,
//                         IsWithdrawable = request.IsWithdrawable,
//                         IsGhostSection = request.IsGhostSection,
//                         IsOutboundSection = request.IsOutboundSection,
//                         Remark = joint.Remark,
//                         AvailableSeat = joint.SeatLimit,
//                         StartedDate = request.StartedDate,
//                         TotalWeeks = request.TotalWeeks,
//                         ParentSectionId = offeredCourse.Id,
//                         IsClosed = request.IsClosed,
//                         Details = details,
//                         Examinations = examinations
//                     };

//                     var jointSection = _offeredCourseProvider.Create(jointDTO, userId.ToString());

//                     jointSections.Add(new JointSectionDTO
//                     {
//                         Id = jointSection.Id,
//                         CourseId = jointSection.CourseId,
//                         Number = jointSection.Number,
//                         SeatLimit = jointSection.SeatLimit,
//                         Remark = jointSection.Remark,
//                         CreatedAt = jointSection.CreatedAt,
//                         UpdatedAt = jointSection.UpdatedAt
//                     });
//                 }

//                 offeredCourse.JointSections = jointSections;
//             }

//             var response = MapDTOToViewModel(offeredCourse, term, courses, instructors, curriculums, curriculumVersions, faculties, departments, rooms);

//             return response;
//         }

//         public PagedViewModel<OfferedCourseViewModel> Search(SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
//         {
//             var pagedOfferedCourse = _offeredCourseProvider.Search(parameters, page, pageSize);

//             var courseIds = new List<Guid>();
//             var instructorIds = new List<Guid>();
//             var roomIds = new List<Guid>();

//             var termIds = pagedOfferedCourse.Items.Select(x => x.TermId)
//                                                   .Distinct()
//                                                   .ToList();

//             var terms = _termProvider.GetById(termIds)
//                                      .ToList();

//             courseIds.AddRange(pagedOfferedCourse.Items.Select(x => x.CourseId)
//                                                        .Distinct());

//             instructorIds.AddRange(pagedOfferedCourse.Items.Where(x => x.MainInstructorId.HasValue)
//                                                            .Select(x => x.MainInstructorId!.Value)
//                                                            .Distinct());

//             // Section seats.
//             var conditions = pagedOfferedCourse.Items.All(x => x.Seats is null
//                                                                || !x.Seats.Any()) ? null
//                                                                                   : pagedOfferedCourse.Items.Where(x => x.Seats is not null && x.Seats.Any())
//                                                                                                             .SelectMany(x => x.Seats!.Where(x => x.Conditions is not null)
//                                                                                                                                      .SelectMany(x => x.Conditions!))
//                                                                                                             .ToList();

//             var curriculumIds = conditions is null ? Enumerable.Empty<Guid>()
//                                                    : conditions.Where(x => x.CurriculumId.HasValue)
//                                                                .Select(x => x.CurriculumId!.Value)
//                                                                .Distinct()
//                                                                .ToList();

//             var curriculumVersionIds = conditions is null ? Enumerable.Empty<Guid>()
//                                                           : conditions.Where(x => x.CurriculumVersionId.HasValue)
//                                                                       .Select(x => x.CurriculumVersionId!.Value)
//                                                                       .Distinct()
//                                                                       .ToList();

//             var facultyIds = conditions is null ? Enumerable.Empty<Guid>()
//                                                 : conditions.Where(x => x.FacultyId.HasValue)
//                                                             .Select(x => x.FacultyId!.Value)
//                                                             .Distinct()
//                                                             .ToList();

//             var departmentIds = conditions is null ? Enumerable.Empty<Guid>()
//                                                    : conditions.Where(x => x.DepartmentId.HasValue)
//                                                                .Select(x => x.DepartmentId!.Value)
//                                                                .Distinct()
//                                                                .ToList();

//             var curriculums = Enumerable.Empty<CurriculumDTO>();
//             var curriculumVersions = Enumerable.Empty<CurriculumVersionDTO>();
//             var faculties = Enumerable.Empty<FacultyDTO>();
//             var departments = Enumerable.Empty<DepartmentDTO>();

//             if (curriculumIds.Any())
//             {
//                 curriculums = _curriculumProvider.GetById(curriculumIds)
//                                                  .ToList();
//             }

//             if (curriculumVersionIds.Any())
//             {
//                 curriculumVersions = _curriculumVersionProvider.GetById(curriculumVersionIds)
//                                                                .ToList();
//             }

//             if (facultyIds.Any())
//             {
//                 faculties = _facultyProvider.GetById(facultyIds)
//                                             .ToList();
//             }

//             if (departmentIds.Any())
//             {
//                 departments = _departmentProvider.GetById(departmentIds)
//                                                  .ToList();
//             }

//             // Joint sections.
//             courseIds.AddRange(pagedOfferedCourse.Items.Where(x => x.JointSections is not null)
//                                                        .SelectMany(x => x.JointSections!.Select(x => x.CourseId)));

//             // Section details.
//             instructorIds.AddRange(pagedOfferedCourse.Items.SelectMany(x => x.Details.Where(x => x.InstructorId.HasValue)
//                                                                                      .Select(x => x.InstructorId!.Value)
//                                                                                      .Distinct()));

//             roomIds.AddRange(pagedOfferedCourse.Items.SelectMany(x => x.Details.Where(x => x.RoomId.HasValue)
//                                                                                .Select(x => x.RoomId!.Value)
//                                                                                .Distinct()));

//             // Section examinations.
//             roomIds.AddRange(pagedOfferedCourse.Items.SelectMany(x => x.Examinations.Where(x => x.RoomId.HasValue)
//                                                                                     .Select(x => x.RoomId!.Value)
//                                                                                     .Distinct()));

//             var courses = Enumerable.Empty<CourseDTO>();
//             var instructors = Enumerable.Empty<EmployeeDTO>();
//             var rooms = Enumerable.Empty<RoomDTO>();

//             if (courseIds.Any())
//             {
//                 courses = _courseProvider.GetById(courseIds.Distinct())
//                                          .ToList();
//             }

//             if (instructorIds.Any())
//             {
//                 instructors = _employeeProvider.GetById(instructorIds.Distinct())
//                                                  .ToList();
//             }

//             if (roomIds.Any())
//             {
//                 rooms = _roomProvider.GetById(roomIds)
//                                      .ToList();
//             }

//             var response = new PagedViewModel<OfferedCourseViewModel>
//             {
//                 Page = pagedOfferedCourse.Page,
//                 TotalPage = pagedOfferedCourse.TotalPage,
//                 TotalItem = pagedOfferedCourse.TotalItem,
//                 Items = (from section in pagedOfferedCourse.Items
//                          let term = terms.SingleOrDefault(x => x.Id == section.TermId)
//                          select MapDTOToViewModel(section, term, courses, instructors, curriculums, curriculumVersions, faculties, departments, rooms))
//                         .ToList()
//             };

//             return response;
//         }

//         public PagedViewModel<StudentViewModel> SearchStudents(Guid id, SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
//         {
//             var pagedStudent = _offeredCourseProvider.SearchStudents(id, parameters, page, pageSize);

//             var academicLevelIds = pagedStudent.Items.Select(x => x.AcademicLevelId)
//                                                      .Distinct()
//                                                      .ToList();

//             var academicLevels = _academicLevelProvider.GetById(academicLevelIds)
//                                                        .ToList();

//             var facultyIds = pagedStudent.Items.Select(x => x.FacultyId)
//                                                .Distinct()
//                                                .ToList();

//             var faculties = _facultyProvider.GetById(facultyIds)
//                                             .ToList();

//             var departmentIds = pagedStudent.Items.Where(x => x.DepartmentId.HasValue)
//                                                   .Select(x => x.DepartmentId!.Value)
//                                                   .Distinct()
//                                                   .ToList();

//             var departments = _departmentProvider.GetById(departmentIds)
//                                                  .ToList();

//             var versionIds = pagedStudent.Items.Select(x => x.CurriculumVersionId)
//                                                .Distinct()
//                                                .ToList();

//             var versions = _curriculumVersionProvider.GetById(versionIds)
//                                                      .ToList();

//             var response = new PagedViewModel<StudentViewModel>
//             {
//                 Page = pagedStudent.Page,
//                 TotalPage = pagedStudent.TotalPage,
//                 TotalItem = pagedStudent.TotalItem,
//                 Items = (from student in pagedStudent.Items
//                          let academicLevel = academicLevels.SingleOrDefault(x => x.Id == student.AcademicLevelId)
//                          let faculty = faculties.SingleOrDefault(x => x.Id == student.FacultyId)
//                          let department = student.DepartmentId.HasValue ? departments.SingleOrDefault(x => x.Id == student.DepartmentId)
//                                                                         : null
//                          let version = versions.SingleOrDefault(x => x.Id == student.CurriculumVersionId)
//                          select _studentManager.MapDTOToViewModel(student, academicLevel, faculty, department, version))
//                         .ToList()
//             };

//             return response;
//         }

//         public OfferedCourseViewModel GetById(Guid id)
//         {
//             var courseIds = new List<Guid>();
//             var instructorIds = new List<Guid>();
//             var roomIds = new List<Guid>();

//             // Section.
//             var offeredCourse = _offeredCourseProvider.GetById(id);

//             var term = _termProvider.GetById(offeredCourse.TermId);

//             courseIds.Add(offeredCourse.CourseId);

//             if (offeredCourse.MainInstructorId.HasValue)
//             {
//                 instructorIds.Add(offeredCourse.MainInstructorId.Value);
//             }

//             // Section seats.
//             var conditions = offeredCourse.Seats is null || !offeredCourse.Seats.Any() ? null
//                                                                                        : offeredCourse.Seats.Where(x => x.Conditions is not null)
//                                                                                                             .SelectMany(x => x.Conditions!)
//                                                                                                             .ToList();

//             var curriculumIds = conditions is null ? Enumerable.Empty<Guid>()
//                                                    : conditions.Where(x => x.CurriculumId.HasValue)
//                                                                .Select(x => x.CurriculumId!.Value)
//                                                                .Distinct()
//                                                                .ToList();

//             var curriculumVersionIds = conditions is null ? Enumerable.Empty<Guid>()
//                                                           : conditions.Where(x => x.CurriculumVersionId.HasValue)
//                                                                       .Select(x => x.CurriculumVersionId!.Value)
//                                                                       .Distinct()
//                                                                       .ToList();

//             var facultyIds = conditions is null ? Enumerable.Empty<Guid>()
//                                                 : conditions.Where(x => x.FacultyId.HasValue)
//                                                             .Select(x => x.FacultyId!.Value)
//                                                             .Distinct()
//                                                             .ToList();

//             var departmentIds = conditions is null ? Enumerable.Empty<Guid>()
//                                                    : conditions.Where(x => x.DepartmentId.HasValue)
//                                                                .Select(x => x.DepartmentId!.Value)
//                                                                .Distinct()
//                                                                .ToList();

//             var curriculums = Enumerable.Empty<CurriculumDTO>();
//             var curriculumVersions = Enumerable.Empty<CurriculumVersionDTO>();
//             var faculties = Enumerable.Empty<FacultyDTO>();
//             var departments = Enumerable.Empty<DepartmentDTO>();

//             if (curriculumIds.Any())
//             {
//                 curriculums = _curriculumProvider.GetById(curriculumIds)
//                                                  .ToList();
//             }

//             if (curriculumVersionIds.Any())
//             {
//                 curriculumVersions = _curriculumVersionProvider.GetById(curriculumVersionIds)
//                                                                .ToList();
//             }

//             if (facultyIds.Any())
//             {
//                 faculties = _facultyProvider.GetById(facultyIds)
//                                             .ToList();
//             }

//             if (departmentIds.Any())
//             {
//                 departments = _departmentProvider.GetById(departmentIds)
//                                                  .ToList();
//             }

//             // Joint sections.
//             if (offeredCourse.JointSections is not null && offeredCourse.JointSections.Any())
//             {
//                 var jointCourseIds = offeredCourse.JointSections.Select(x => x.CourseId)
//                                                                 .Distinct()
//                                                                 .ToList();

//                 courseIds.AddRange(jointCourseIds);
//             }

//             // Section details.
//             var lecturerIds = offeredCourse.Details.Where(x => x.InstructorId.HasValue)
//                                                    .Select(x => x.InstructorId!.Value)
//                                                    .Distinct()
//                                                    .ToList();

//             if (lecturerIds.Any())
//             {
//                 instructorIds.AddRange(lecturerIds);
//             }

//             var studyRoomIds = offeredCourse.Details.Where(x => x.RoomId.HasValue)
//                                                     .Select(x => x.RoomId!.Value)
//                                                     .Distinct()
//                                                     .ToList();

//             if (studyRoomIds.Any())
//             {
//                 roomIds.AddRange(studyRoomIds);
//             }

//             // Section examinations.
//             var examRoomIds = offeredCourse.Examinations.Where(x => x.RoomId.HasValue)
//                                                         .Select(x => x.RoomId!.Value)
//                                                         .Distinct()
//                                                         .ToList();

//             if (examRoomIds.Any())
//             {
//                 roomIds.AddRange(examRoomIds);
//             }

//             var courses = Enumerable.Empty<CourseDTO>();
//             var instructors = Enumerable.Empty<EmployeeDTO>();
//             var rooms = Enumerable.Empty<RoomDTO>();

//             if (courseIds.Any())
//             {
//                 courses = _courseProvider.GetById(courseIds.Distinct())
//                                          .ToList();
//             }

//             if (instructorIds.Any())
//             {
//                 instructors = _employeeProvider.GetById(instructorIds.Distinct())
//                                                  .ToList();
//             }

//             if (roomIds.Any())
//             {
//                 rooms = _roomProvider.GetById(roomIds)
//                                      .ToList();
//             }

//             var response = MapDTOToViewModel(offeredCourse, term, courses, instructors, curriculums, curriculumVersions, faculties, departments, rooms);

//             return response;
//         }

//         public OfferedCourseViewModel Update(OfferedCourseViewModel request, Guid userId)
//         {
//             // VALIDATE SECTION
//             var sectionDTO = _sectionProvider.GetById(request.Id);

//             if (sectionDTO.ParentSectionId.HasValue)
//             {
//                 throw new SectionException.NotAllowUpdateJointSection();
//             }

//             var (term, courses, instructors, courseIds) = ValidateRequest(request, details: request.Details);

//             var sections = _sectionProvider.GetByTermIdAndCourseId(request.TermId, courseIds)
//                                            .ToList();

//             if (sections.Any(x => x.Id != request.Id
//                                   && x.Number == request.Number))
//             {
//                 throw new SectionException.Duplicate(request.Number);
//             }

//             if (request.Seats is not null && request.Seats.Any())
//             {
//                 if (request.Seats.Any(x => x.Type == SeatType.DEFAULT))
//                 {
//                     throw new SectionException.NotAllowCreateDefaultSeat();
//                 }
//             }

//             // SECTION
//             sectionDTO.CourseId = request.CourseId;
//             sectionDTO.TermId = request.TermId;
//             sectionDTO.Number = request.Number;
//             sectionDTO.SeatLimit = request.SeatLimit;
//             sectionDTO.PlanningSeat = request.PlanningSeat;
//             sectionDTO.MinimumSeat = request.MinimumSeat;
//             sectionDTO.MainInstructorId = request.MainInstructorId;
//             sectionDTO.Status = request.Status;
//             sectionDTO.IsWithdrawable = request.IsWithdrawable;
//             sectionDTO.IsGhostSection = request.IsGhostSection;
//             sectionDTO.IsOutboundSection = request.IsOutboundSection;
//             sectionDTO.Remark = request.Remark;
//             sectionDTO.StartedDate = request.StartedDate;
//             sectionDTO.TotalWeeks = request.TotalWeeks;
//             sectionDTO.ParentSectionId = request.ParentSectionId;
//             sectionDTO.IsClosed = request.IsClosed;

//             // VALIDATE SEAT CONDITIONS
//             var (curriculums, curriculumVersions, faculties, departments) = ValidateSeatConditions(request.Seats);

//             // SECTION SEATS
//             var sectionSeatDTO = request.Seats is null || !request.Seats.Any() ? Enumerable.Empty<UpsertSectionSeatDTO>()
//                                                                                : (from seat in request.Seats
//                                                                                   select SectionSeatManager.MapViewModelToDTO(seat))
//                                                                                  .ToList();

//             // SECTION DETAILS
//             var rooms = ValidateSectionDetails(request.Details, request.Examinations).ToList();

//             var lecturers = Enumerable.Empty<EmployeeDTO>();

//             var details = request.Details is null || !request.Details.Any() ? Enumerable.Empty<UpdateSectionDetailDTO>()
//                                                                             : SectionManager.MapDetailViewModelToDTO(request.Details);

//             var examinations = request.Examinations is null || !request.Examinations.Any() ? Enumerable.Empty<UpdateSectionExaminationDTO>()
//                                                                                            : SectionManager.MapExaminationViewModelToDTO(request.Examinations);

//             var updatedSection = _offeredCourseProvider.Update(sectionDTO, sectionSeatDTO, details, examinations, userId.ToString());

//             // JOINT SECTIONS
//             if (updatedSection.JointSections is not null
//                 && updatedSection.JointSections.Any())
//             {
//                 // UPDATE DETAILS AND EXAMINATIONS
//                 foreach (var jointSection in updatedSection.JointSections)
//                 {
//                     _sectionProvider.Update(jointSection.Id, details, examinations, userId.ToString());
//                 }

//                 // GET COURSES
//                 var jointCourseIds = updatedSection.JointSections.Where(x => !courseIds.Contains(x.CourseId))
//                                                                  .Select(x => x.CourseId)
//                                                                  .Distinct()
//                                                                  .ToList();

//                 var jointCourses = _courseProvider.GetById(jointCourseIds)
//                                                   .ToList();

//                 courses.AddRange(jointCourses);
//             }

//             var response = MapDTOToViewModel(updatedSection, term, courses, instructors, curriculums, curriculumVersions, faculties, departments, rooms);

//             return response;
//         }

//         public IEnumerable<SectionSeatViewModel> UpdateSeats(Guid sectionId, IEnumerable<UpsertSectionSeatViewModel>? requests, Guid userId)
//         {
//             var section = _sectionProvider.GetById(sectionId);

//             if (requests is null || !requests.Any())
//             {
//                 return Enumerable.Empty<SectionSeatViewModel>();
//             }

//             if (requests.Any(x => x.Type == SeatType.DEFAULT))
//             {
//                 throw new SectionException.NotAllowCreateDefaultSeat();
//             }

//             // VALIDATE SEAT CONDITIONS
//             var (curriculums, curriculumVersions, faculties, departments) = ValidateSeatConditions(requests);

//             // SECTION SEATS
//             var sectionSeatDTO = requests is null || !requests.Any() ? Enumerable.Empty<UpsertSectionSeatDTO>()
//                                                                      : (from seat in requests
//                                                                         select SectionSeatManager.MapViewModelToDTO(seat))
//                                                                        .ToList();

//             var sectionSeats = _offeredCourseProvider.UpdateSeats(sectionId, sectionSeatDTO, userId.ToString());

//             var response = (from seat in sectionSeats
//                             select SectionSeatManager.MapDTOToViewModel(seat, curriculums, curriculumVersions, faculties, departments))
//                            .ToList();

//             return response;
//         }

//         public void Delete(Guid id)
//         {
//             _offeredCourseProvider.Delete(id);
//         }

//         private (TermDTO, List<CourseDTO>, List<EmployeeDTO>, List<Guid>) ValidateRequest(CreateSectionViewModel request,
//             IEnumerable<CreateJointSectionViewModel>? jointSections = null,
//             IEnumerable<UpdateSectionDetailViewModel>? details = null)
//         {
//             var term = _termProvider.GetById(request.TermId);

//             // VALIDATE GIVEN COURSE EXISTS
//             var courseIds = new List<Guid> { request.CourseId };

//             if (jointSections is not null && jointSections.Any())
//             {
//                 var jointCourseIds = jointSections.Select(x => x.CourseId)
//                                                   .Distinct()
//                                                   .ToList();

//                 courseIds = courseIds.Concat(jointCourseIds)
//                                      .ToList();
//             }

//             var courses = _courseProvider.GetById(courseIds)
//                                          .ToList();

//             foreach (var id in courseIds)
//             {
//                 if (!courses.Any(x => x.Id == id))
//                 {
//                     throw new CourseException.NotFound(id);
//                 }
//             }

//             // VALIDATE INSTRUCTORS EXISTS
//             var instructorIds = details is null ? new List<Guid>()
//                                                 : details.Where(x => x.InstructorId.HasValue)
//                                                          .Select(x => x.InstructorId!.Value)
//                                                          .ToList();

//             if (request.MainInstructorId.HasValue)
//             {
//                 instructorIds.Add(request.MainInstructorId.Value);
//             }

//             var instrutors = !instructorIds.Any() ? new List<EmployeeDTO>()
//                                                   : _employeeProvider.GetById(instructorIds).ToList();

//             foreach (var id in instructorIds)
//             {
//                 if (!instrutors.Any(x => x.Id == id))
//                 {
//                     throw new EmployeeException.InstructorNotFound(id);
//                 }
//             }

//             return (term, courses, instrutors, courseIds);
//         }

//         private (List<CurriculumDTO>, List<CurriculumVersionDTO>,
//                  List<FacultyDTO>, List<DepartmentDTO>)
//             ValidateSeatConditions(IEnumerable<UpsertSectionSeatViewModel>? seats)
//         {
//             if (seats is null || !seats.Any())
//             {
//                 return (new List<CurriculumDTO>(), new List<CurriculumVersionDTO>(),
//                         new List<FacultyDTO>(), new List<DepartmentDTO>());
//             }

//             var conditions = seats.Where(x => x.Conditions is not null)
//                                   .SelectMany(x => x.Conditions!)
//                                   .ToList();

//             var curriculumIds = conditions.Where(x => x.CurriculumId.HasValue)
//                                           .Select(x => x.CurriculumId!.Value)
//                                           .Distinct()
//                                           .ToList();

//             var curriculumVersionIds = conditions.Where(x => x.CurriculumVersionId.HasValue)
//                                                  .Select(x => x.CurriculumVersionId!.Value)
//                                                  .Distinct()
//                                                  .ToList();

//             var facultyIds = conditions.Where(x => x.FacultyId.HasValue)
//                                        .Select(x => x.FacultyId!.Value)
//                                        .Distinct()
//                                        .ToList();

//             var departmentIds = conditions.Where(x => x.DepartmentId.HasValue)
//                                           .Select(x => x.DepartmentId!.Value)
//                                           .Distinct()
//                                           .ToList();

//             var curriculums = new List<CurriculumDTO>();
//             var curriculumVersions = new List<CurriculumVersionDTO>();
//             var faculties = new List<FacultyDTO>();
//             var departments = new List<DepartmentDTO>();

//             if (curriculumIds.Any())
//             {
//                 curriculums = _curriculumProvider.GetById(curriculumIds)
//                                                  .ToList();

//                 foreach (var id in curriculumIds)
//                 {
//                     if (!curriculums.Any(x => x.Id == id))
//                     {
//                         throw new CurriculumException.NotFound(id);
//                     }
//                 }
//             }

//             if (curriculumVersionIds.Any())
//             {
//                 curriculumVersions = _curriculumVersionProvider.GetById(curriculumVersionIds)
//                                                                .ToList();

//                 foreach (var id in curriculumVersionIds)
//                 {
//                     if (!curriculumVersions.Any(x => x.Id == id))
//                     {
//                         throw new CurriculumException.VersionNotFound(id);
//                     }
//                 }
//             }

//             if (facultyIds.Any())
//             {
//                 faculties = _facultyProvider.GetById(facultyIds)
//                                             .ToList();

//                 foreach (var id in facultyIds)
//                 {
//                     if (!faculties.Any(x => x.Id == id))
//                     {
//                         throw new CurriculumException.VersionNotFound(id);
//                     }
//                 }
//             }

//             if (departmentIds.Any())
//             {
//                 departments = _departmentProvider.GetById(departmentIds)
//                                                  .ToList();

//                 foreach (var id in departmentIds)
//                 {
//                     if (!departments.Any(x => x.Id == id))
//                     {
//                         throw new CurriculumException.VersionNotFound(id);
//                     }
//                 }
//             }

//             return (curriculums, curriculumVersions, faculties, departments);
//         }

//         private List<RoomDTO> ValidateSectionDetails(IEnumerable<UpdateSectionDetailViewModel> sectionDetails,
//                                                      IEnumerable<UpdateSectionExaminationViewModel> examinations)
//         {
//             if (sectionDetails is null)
//             {
//                 sectionDetails = Enumerable.Empty<UpdateSectionDetailViewModel>();
//             }

//             if (SectionManager.IsSectionDetailOverlapped(sectionDetails))
//             {
//                 throw new SectionException.DetailOverlapped();
//             }

//             if (SectionManager.IsExaminationOverlapped(examinations))
//             {
//                 throw new SectionException.ExamOverlapped();
//             }

//             var roomIds = sectionDetails.Where(x => x.RoomId.HasValue)
//                                         .Select(x => x.RoomId!.Value)
//                                         .ToList();

//             var examRoomIds = examinations.Where(x => x.RoomId.HasValue)
//                                           .Select(x => x.RoomId!.Value)
//                                           .ToList();

//             roomIds = roomIds.Concat(examRoomIds)
//                              .Distinct()
//                              .ToList();

//             var rooms = _roomProvider.GetById(roomIds)
//                                      .ToList();

//             foreach (var id in roomIds)
//             {
//                 if (!rooms.Any(x => x.Id == id))
//                 {
//                     throw new RoomException.NotFound(id);
//                 }
//             }

//             return rooms;
//         }

//         private static OfferedCourseViewModel MapDTOToViewModel(OfferedCourseDTO dto, TermDTO term, IEnumerable<CourseDTO> courses, IEnumerable<EmployeeDTO> instructors,
//             IEnumerable<CurriculumDTO> curriculums, IEnumerable<CurriculumVersionDTO> versions, IEnumerable<FacultyDTO> faculties, IEnumerable<DepartmentDTO> departments,
//             IEnumerable<RoomDTO> rooms)
//         {
//             var course = courses.SingleOrDefault(x => x.Id == dto.CourseId);

//             var instructor = dto.MainInstructorId.HasValue ? instructors.SingleOrDefault(x => x.Id == dto.MainInstructorId.Value)
//                                                            : null;

//             var response = new OfferedCourseViewModel
//             {
//                 Id = dto.Id,
//                 CourseId = dto.CourseId,
//                 TermId = dto.TermId,
//                 Number = dto.Number,
//                 SeatLimit = dto.SeatLimit,
//                 PlanningSeat = dto.PlanningSeat,
//                 MinimumSeat = dto.MinimumSeat,
//                 MainInstructorId = dto.MainInstructorId,
//                 Status = dto.Status,
//                 IsWithdrawable = dto.IsWithdrawable,
//                 IsGhostSection = dto.IsGhostSection,
//                 IsOutboundSection = dto.IsOutboundSection,
//                 AvailableSeat = dto.AvailableSeat,
//                 StartedDate = dto.StartedDate,
//                 TotalWeeks = dto.TotalWeeks,
//                 ParentSectionId = dto.ParentSectionId,
//                 IsClosed = dto.IsClosed,
//                 Remark = dto.Remark,
//                 CreatedAt = dto.CreatedAt,
//                 UpdatedAt = dto.UpdatedAt,
//                 TermNumber = term.Number,
//                 Year = term.Year,
//                 CourseCode = course?.Code,
//                 CourseName = course?.Name,
//                 MainInstructorCode = instructor?.Code,
//                 MainInstructorFirstName = instructor?.FirstName,
//                 MainInstructorMiddleName = instructor?.MiddleName,
//                 MainInstructorLastName = instructor?.LastName,
//                 Seats = dto.Seats is null || !dto.Seats.Any() ? null
//                                                               : (from seat in dto.Seats
//                                                                  select SectionSeatManager.MapDTOToViewModel(seat, curriculums, versions, faculties, departments))
//                                                                 .ToList(),
//                 JointSections = dto.JointSections is null || !dto.JointSections.Any() ? null
//                                                                                       : (from jointSection in dto.JointSections
//                                                                                          let jointCourse = courses.SingleOrDefault(x => x.Id == jointSection.CourseId)
//                                                                                          select new JointSectionViewModel
//                                                                                          {
//                                                                                              Id = jointSection.Id,
//                                                                                              CourseId = jointSection.CourseId,
//                                                                                              Number = jointSection.Number,
//                                                                                              SeatLimit = jointSection.SeatLimit,
//                                                                                              Remark = jointSection.Remark,
//                                                                                              CreatedAt = jointSection.CreatedAt,
//                                                                                              UpdatedAt = jointSection.UpdatedAt,
//                                                                                              CourseCode = jointCourse?.Code,
//                                                                                              CourseName = jointCourse?.Name
//                                                                                          })
//                                                                                         .ToList(),
//                 Details = (from detail in dto.Details
//                            let studyRoom = detail.RoomId.HasValue ? rooms.SingleOrDefault(x => x.Id == detail.RoomId.Value)
//                                                                   : null
//                            let lecturer = detail.InstructorId.HasValue ? instructors.SingleOrDefault(x => x.Id == detail.InstructorId.Value)
//                                                                        : null
//                            select SectionManager.MapSectionDetailDTOToViewModel(detail, studyRoom, lecturer))
//                           .ToList(),
//                 Examinations = (from examination in dto.Examinations
//                                 let examRoom = examination.RoomId.HasValue ? rooms.SingleOrDefault(x => x.Id == examination.RoomId)
//                                                                            : null
//                                 select SectionManager.MapExaminationDTOToViewModel(dto.Id, examination, examRoom))
//                                .ToList()
//             };

//             return response;
//         }
//     }
// }
