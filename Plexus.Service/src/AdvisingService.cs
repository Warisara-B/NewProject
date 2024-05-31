using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic;
using Plexus.Database.Enum.Academic.Advising;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Advising;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Integration;
using Plexus.Service.Exception;
using Plexus.Service.ViewModel.Advising;
using ServiceStack;

namespace Plexus.Service.src
{
    public class AdvisingService : IAdvisingService
    {
        private readonly DatabaseContext _dbContext;
        private readonly IBlobStorageProvider _storageProvider;

        public AdvisingService(DatabaseContext dbContext, IBlobStorageProvider storageProvider)
        {
            _dbContext = dbContext;
            _storageProvider = storageProvider;
        }

        public AdvisorProfileViewModel GetAdvisorProfile(Guid studentId, LanguageCode language)
        {
            // GET STUDENT & CURRENT TERM
            var student = _dbContext.Students.AsNoTracking()
                                             .Include(x => x.CurriculumVersion)
                                             .SingleOrDefault(x => x.Id == studentId);
            if (student is null)
            {
                throw new StudentException.NotFound();
            }

            var studentTerm = GetStudentCurrentTerm(student.Id, student.AcademicLevelId.Value, student.CurriculumVersion.CollegeCalendarType);

            if (!studentTerm.AdvisorId.HasValue)
            {
                throw new AdvisingException.AdvisorNotAssign();
            }

            var advisor = _dbContext.Employees.AsNoTracking()
                                                .Include(x => x.Localizations)
                                                .Include(x => x.WorkInformation)
                                                    .ThenInclude(x => x.Faculty)
                                                        .ThenInclude(x => x.Localizations)
                                                .Single(x => x.Id == studentTerm.AdvisorId.Value);

            var advisorLocale = advisor.Localizations.SingleOrDefault(x => x.Language == language);

            var faculty = advisor.WorkInformation?.Faculty;
            var facultyLocale = faculty?.Localizations.SingleOrDefault(x => x.Language == language);

            var response = new AdvisorProfileViewModel
            {
                Id = advisor.Id,
                Title = advisor.Title,
                FirstName = advisorLocale?.FirstName ?? advisor.FirstName,
                MiddleName = advisorLocale?.MiddleName ?? advisor.MiddleName,
                LastName = advisorLocale?.LastName ?? advisor.LastName,
                ProfileImageUrl = advisor.CardImageUrl,
                Faculty = new AdvisorFacultyViewModel
                {
                    LogoUrl = string.IsNullOrEmpty(faculty?.LogoImagePath) ? null
                                                                           : _storageProvider.GetBlobPublicUrl(faculty.LogoImagePath),
                    Name = facultyLocale?.Name ?? null,
                },
                AcademicContact = new AdvisorAcademicContactViewModel
                {
                    Address = null, // TODO : MAP INSTRUCTOR ACADEMIC ADDRESS
                    Email = advisor.UniversityEmail,
                    PhoneNumber = advisor.PhoneNumber1
                },
                PersonalContact = new AdvisorPersonalContactViewModel
                {
                    Email = advisor.PersonalEmail,
                    PhoneNumber = advisor.PhoneNumber2
                }
            };

            return response;
        }

        public IEnumerable<AdvisingAppointmentViewModel> GetUpcomingAppointmentSlots(Guid studentId, Guid instructorId)
        {
            // STUDENT ONLY GET AVAILABLE && SELF BOOKED SLOTS
            var advisingSlots = _dbContext.AdvisingSlots.AsNoTracking()
                                                        .Where(x => x.InstructorId == instructorId
                                                               && (x.Status == AdvisingSlotStatus.AVAILABLE
                                                                   || (x.Status == AdvisingSlotStatus.BOOKED
                                                                       && x.StudentId == studentId))
                                                               && x.EndedAt > DateTime.UtcNow
                                                               && x.EndedAt <= DateTime.UtcNow.AddMonths(3))
                                                        .ToList();

            // UPCOMING SLOT ORDERBY NEAREST TO FARTHEST
            var response = (from slot in advisingSlots
                            group slot by new { slot.EndedAt.Year, slot.EndedAt.Month } into slotbyMonth
                            orderby slotbyMonth.Key.Year, slotbyMonth.Key.Month
                            select new AdvisingAppointmentViewModel
                            {
                                Year = slotbyMonth.Key.Year,
                                Month = slotbyMonth.Key.Month,
                                Slots = (from slot in slotbyMonth
                                         orderby slot.EndedAt
                                         select new AdvisingAppointmentSlotViewModel
                                         {
                                             Id = slot.Id,
                                             StartedAt = slot.StartedAt,
                                             EndedAt = slot.EndedAt,
                                             Status = slot.Status
                                         })
                                        .ToList()
                            })
                           .ToList();

            return response;
        }

        public IEnumerable<AdvisingAppointmentViewModel> GetAppointmentSlotHistory(Guid studentId, Guid instructorId)
        {
            // ONLY GET SELF BOOKED SLOT HISTORY
            var advisingSlots = _dbContext.AdvisingSlots.AsNoTracking()
                                                        .Where(x => x.InstructorId == instructorId
                                                               && x.StudentId == studentId
                                                               && x.EndedAt <= DateTime.UtcNow
                                                               && x.EndedAt > DateTime.UtcNow.AddMonths(-3))
                                                        .ToList();

            var response = (from slot in advisingSlots
                            group slot by new { slot.EndedAt.Year, slot.EndedAt.Month } into slotbyMonth
                            orderby slotbyMonth.Key.Year descending, slotbyMonth.Key.Month descending
                            select new AdvisingAppointmentViewModel
                            {
                                Year = slotbyMonth.Key.Year,
                                Month = slotbyMonth.Key.Month,
                                Slots = (from slot in slotbyMonth
                                         orderby slot.EndedAt descending
                                         select new AdvisingAppointmentSlotViewModel
                                         {
                                             Id = slot.Id,
                                             StartedAt = slot.StartedAt,
                                             EndedAt = slot.EndedAt,
                                             Status = slot.Status
                                         })
                                        .ToList()
                            })
                            .ToList();

            return response;
        }

        public void BookAdvisingSlot(Guid slotId, Guid studentId)
        {
            // VALIDATE SLOT IS EXISTS & NOT BOOKED
            var advisingSlot = _dbContext.AdvisingSlots.SingleOrDefault(x => x.Id == slotId);
            if (advisingSlot is null)
            {
                throw new AdvisingException.SlotNotFound();
            }

            // VALIDATE FUTURE SLOT
            var now = DateTime.UtcNow;
            if (advisingSlot.StartedAt < now)
            {
                throw new AdvisingException.NotAllowBookAdvisingSlot();
            }

            // CHECK CONDITION IF BOOKED OR NOT AVAILABLE STATUS
            if (advisingSlot.Status != AdvisingSlotStatus.AVAILABLE
                || advisingSlot.StudentId.HasValue)
            {
                // VALIDATE SLOT NOT CANCELLED
                if (advisingSlot.Status == AdvisingSlotStatus.CANCELLED)
                {
                    throw new AdvisingException.NotAllowBookAdvisingSlot();
                }

                // VALIDATE BOOKED SLOT NOT BOOK BY OTHER STUDENT
                if (advisingSlot.StudentId.HasValue && advisingSlot.StudentId.Value != studentId)
                {
                    throw new AdvisingException.OtherBookSlot();
                }
            }

            // VALIDATE STUDENT AND GET STUDENT TERM
            var student = _dbContext.Students.AsNoTracking()
                                             .Include(x => x.CurriculumVersion)
                                             .SingleOrDefault(x => x.Id == studentId);
            if (student is null)
            {
                throw new StudentException.NotFound();
            }

            var studentTerm = GetStudentCurrentTerm(student.Id, student.AcademicLevelId.Value, student.CurriculumVersion.CollegeCalendarType);
            if (!studentTerm.AdvisorId.HasValue || advisingSlot.InstructorId != studentTerm.AdvisorId.Value)
            {
                throw new AdvisingException.AdvisorNotAssign();
            }

            advisingSlot.StudentId = student.Id;
            advisingSlot.TermId = studentTerm.TermId;
            advisingSlot.Status = AdvisingSlotStatus.BOOKED;

            _dbContext.SaveChanges();
        }

        public void UnbookAdvisingSlot(Guid slotId, Guid studentId)
        {
            // VALIDATE SLOT IS EXISTS
            var advisingSlot = _dbContext.AdvisingSlots.SingleOrDefault(x => x.Id == slotId);
            if (advisingSlot is null)
            {
                throw new AdvisingException.SlotNotFound();
            }

            // CHECK FOLLOWING CONDITION
            // 1) SLOT IS BOOKED BY REQUESTER
            // 2) SLOT STATUS IS BOOKED ONLY
            // 3) SLOT IS NOT STARTED YET
            if (!advisingSlot.StudentId.HasValue ||
                 advisingSlot.StudentId.Value != studentId ||
                 advisingSlot.Status != AdvisingSlotStatus.BOOKED ||
                 advisingSlot.StartedAt < DateTime.UtcNow)
            {
                throw new AdvisingException.NotAllowUpdateAdvisingSlot();
            }

            // UNBOOKED SLOT
            advisingSlot.Status = AdvisingSlotStatus.AVAILABLE;
            advisingSlot.StudentId = null;
            advisingSlot.TermId = null;
            _dbContext.SaveChanges();
        }

        public AdvisingViewModel GetAdvisingInformation(Guid studentId, LanguageCode language)
        {
            // VALIDATE STUDENT & CURRENT TERM
            var student = _dbContext.Students.AsNoTracking()
                                             .Include(x => x.CurriculumVersion)
                                             .SingleOrDefault(x => x.Id == studentId);
            if (student is null)
            {
                throw new StudentException.NotFound();
            }
            var studentTerm = GetStudentCurrentTerm(student.Id, student.AcademicLevelId.Value, student.CurriculumVersion.CollegeCalendarType);

            // GET ADVISING COURSES
            var recommendCourses = Enumerable.Empty<CourseRecommendation>();
            if (studentTerm.AdvisorId.HasValue)
            {
                recommendCourses = _dbContext.CourseRecommendations.AsNoTracking()
                                                                   .Include(x => x.Course)
                                                                        .ThenInclude(x => x.Localizations)
                                                                   .Where(x => x.StudentId == student.Id
                                                                               && x.TermId == studentTerm.TermId
                                                                               // FILTER WHERE COURSES IS ADDED BY ADVISOR
                                                                               && x.InstructorId.HasValue
                                                                               && x.InstructorId.Value == studentTerm.AdvisorId.Value)
                                                                   // ORDERING BY CODE
                                                                   .OrderBy(x => x.Course.Code)
                                                                   .ToList();
            }

            // GET CURRICULUM COURSES
            var curriculumCourses = Enumerable.Empty<CurriculumCourse>();
            if (recommendCourses.Any())
            {
                var courseIds = recommendCourses.Select(x => x.CourseId)
                                                .Distinct()
                                                .ToList();

                curriculumCourses = _dbContext.CurriculumCourses.AsNoTracking()
                                                                .Include(x => x.RequiredGrade)
                                                                .Where(x => x.CourseGroup.CurriculumVersionId == student.CurriculumVersionId
                                                                            && courseIds.Contains(x.CourseId))
                                                                .ToList();
            }

            // MAP INFORMATION
            var response = new AdvisingViewModel
            {
                Term = new AdvisingTermViewModel
                {
                    AcademicTerm = studentTerm.Term.Number,
                    AcademicYear = studentTerm.Term.Year
                },
                Status = new AdvisingStatusViewModel
                {
                    IsAdvised = studentTerm.Status.HasFlag(AdvisingStatus.ADVISE),
                    IsAllowRegistration = studentTerm.Status.HasFlag(AdvisingStatus.REGISTRATION),
                    IsAllowPayment = studentTerm.Status.HasFlag(AdvisingStatus.PAYMENT)
                }
            };

            // MAP RECOMMENDATION COURSES
            if (recommendCourses.Any())
            {
                var advisingCourses = new List<AdvisingCourseViewModel>();
                foreach (var recommendCourse in recommendCourses)
                {
                    var matchingCurriculumCourse = curriculumCourses.SingleOrDefault(x => x.CourseId == recommendCourse.CourseId);

                    var course = recommendCourse.Course;
                    var localize = course.Localizations.SingleOrDefault(x => x.Language == language);

                    var viewModel = new AdvisingCourseViewModel
                    {
                        Id = recommendCourse.CourseId,
                        Code = course.Code,
                        Name = localize?.Name ?? course.Name,
                        Credit = course.Credit,
                        PassGrade = matchingCurriculumCourse?.RequiredGrade?.Letter,
                        IsRequired = recommendCourse.IsRequired
                    };

                    advisingCourses.Add(viewModel);
                }

                response.Courses = advisingCourses;
            }

            return response;
        }

        private StudentTerm GetStudentCurrentTerm(Guid studentId, Guid academicLevelId, CollegeCalendarType calendarType)
        {
            var studentTerm = _dbContext.StudentTerms.AsNoTracking()
                                                     .Include(x => x.Term)
                                                     .FirstOrDefault(x => x.StudentId == studentId
                                                                          && x.Term.AcademicLevelId == academicLevelId
                                                                          && x.Term.CollegeCalendarType == calendarType
                                                                          && x.Term.IsCurrent);

            return studentTerm;
        }
    }
}
