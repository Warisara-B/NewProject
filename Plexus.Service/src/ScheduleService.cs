using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Plexus.Database;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic;
using Plexus.Database.Enum.Academic.Section;
using Plexus.Database.Model;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Section;
using Plexus.Service.Exception;
using Plexus.Service.ViewModel.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plexus.Service.src
{
    public class ScheduleService : IScheduleService
    {

        private static readonly string[] CourseColors = { "#FF5630", "#FF7B00", "#FF9D0D", "#8BC34A", "#00C061", "#467669", "#01B8AA", "#42A5F5", "#1A77F2", "#1223A5", "#6C3DCC", "#CA58FF", "#EE46BC", "#8F1449", "#943E10" };
        private readonly IAsyncRepository<StudyCourse> _studyCourseRepo;
        private readonly IAsyncRepository<Section> _sectionRepo;
        private readonly IAsyncRepository<SectionClassPeriod> _sectionClassPeriodRepo;
        private readonly IAsyncRepository<SectionSchedule> _sectionScheduleRepo;
        private readonly IAsyncRepository<Course> _courseRepo;
        private readonly IAsyncRepository<Employee> _employeeRepo;

        public ScheduleService(IAsyncRepository<StudyCourse> studyCourseRepo
            , IAsyncRepository<Section> sectionRepo
            , IAsyncRepository<SectionClassPeriod> sectionClassPeriodRepo
            , IAsyncRepository<SectionSchedule> sectionScheduleRepo
            , IAsyncRepository<Course> courseRepo
            , IAsyncRepository<Employee> employeeRepo)
        {
            _studyCourseRepo = studyCourseRepo;
            _sectionRepo = sectionRepo;
            _sectionClassPeriodRepo = sectionClassPeriodRepo;
            _sectionScheduleRepo = sectionScheduleRepo;
            _courseRepo = courseRepo;
            _employeeRepo = employeeRepo;
        }

        public List<ClassScheduleViewModel> GetClassScheduleByDate(Guid studentId, LanguageCode language, DateTime? startDate, DateTime? endDate)
        {
            var queryDateStart = startDate is null ? DateTime.UtcNow : startDate.Value;
            var queryDateEnd = endDate is null ? DateTime.UtcNow : endDate.Value;

            var studentRegisLists = _studyCourseRepo.Query()
                .Include(x => x.Section)
                .Include(x => x.Course)
                .ThenInclude(x => x.Localizations.Where(w => w.Language == language))
                .Where(x => x.StudentId == studentId && x.Status == StudyCourseStatus.ACTIVE
                            && x.Section != null && x.Section.Status == SectionStatus.OPEN)
                .ToList();

            var sectionRegisLists = studentRegisLists.Select(x => x.SectionId).Distinct();

            if (sectionRegisLists is null)
            {
                throw new ScheduleException.NotFound();
            }

            var scheduleLists = _sectionScheduleRepo.Query()
                .Include(x => x.Section)
                .Include(x => x.Room)
                .ThenInclude(x => x.Localizations.Where(w => w.Language == language))
                .Where(x => sectionRegisLists.Contains(x.SectionId)
                            && x.StartAt.Date >= queryDateStart.Date
                            && x.EndAt.Date < queryDateEnd.Date)
                .ToList();

            var classSchedules = MapClassScheduleToViewModel(studentRegisLists, scheduleLists);

            return classSchedules;
        }

        public List<ClassScheduleTermViewModel> GetClassScheduleByTerm(Guid studentId, LanguageCode language, Guid termId)
        {
            var studentRegisLists = _studyCourseRepo.Query()
                           .Include(x => x.Section)
                           .ThenInclude(x => x.SectionClassPeriods)
                           .ThenInclude(x => x.Room)
                           .ThenInclude(x => x.Localizations.Where(w => w.Language == language))
                           .Include(x => x.Course)
                           .ThenInclude(x => x.Localizations.Where(w => w.Language == language))
                           .Where(x => x.TermId == termId
                                       && x.StudentId == studentId
                                       && x.Status == StudyCourseStatus.ACTIVE
                                       && x.Section != null && x.Section.Status == SectionStatus.OPEN)
                           .OrderBy(x => x.CourseId)
                           .ToList();

            if (studentRegisLists is null)
            {
                throw new ScheduleException.NotFound();
            }

            var results = MapClassScheduleTermToViewModel(studentRegisLists);

            return results;
        }

        public ClassScheduleDetailViewModel GetClassScheduleDetailById(Guid studentId, LanguageCode language, Guid classId)
        {
            var schedule = _sectionScheduleRepo.Query()
                .Include(x => x.Section)
                .ThenInclude(x => x.Instructors)
                .Include(x => x.Section)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.Localizations.Where(w => w.Language == language))
                .Include(x => x.Room)
                .ThenInclude(x => x.Localizations.Where(w => w.Language == language))
                .FirstOrDefault(x => x.Id == classId);

            if (schedule is null)
            {
                throw new ScheduleException.NotFound();
            }

            var studentRegisLists = _studyCourseRepo.Query()
                .Include(x => x.Section)
                .Include(x => x.Course)
                .Where(x => x.StudentId == studentId && x.Status == StudyCourseStatus.ACTIVE
                            && x.Section != null && x.Section.Status == SectionStatus.OPEN)
                .OrderBy(x => x.CourseId)
                .Select(x => x.CourseId)
                .ToList();

            var sectionInstructors = schedule.Section.Instructors.Select(x => x.InstructorId);
            var instructors = _employeeRepo.Query()
                .Include(x => x.Localizations.Where(w => w.Language == language))
                .Where(x => sectionInstructors != null && sectionInstructors.Contains(x.Id))
                .ToList();

            var classScheduleDetail = MapClassScheduleDetailToViewModel(schedule, studentRegisLists, instructors);

            return classScheduleDetail;
        }

        public ClassScheduleTermDetailViewModel GetClassScheduleDetailByTermAndClassId(Guid studentId, LanguageCode language, Guid termId, Guid classId)
        {
            var section = _sectionRepo.Query()
                .Include(x => x.Course)
                .ThenInclude(x => x.Localizations.Where(w => w.Language == language))
                .Include(x => x.SectionClassPeriods)
                .ThenInclude(x => x.Room)
                .ThenInclude(x => x.Localizations.Where(w => w.Language == language))
                .Include(x => x.Instructors)
                .FirstOrDefault(x => x.Id == classId && x.TermId == termId && x.Status == SectionStatus.OPEN);

            if (section == null)
            {
                throw new ScheduleException.NotFound();
            }

            var studentRegisLists = _studyCourseRepo.Query()
               .Include(x => x.Section)
               .Include(x => x.Course)
               .Where(x => x.StudentId == studentId && x.Status == StudyCourseStatus.ACTIVE
                           && x.Section != null && x.Section.Status == SectionStatus.OPEN)
               .OrderBy(x => x.CourseId)
               .Select(x => x.CourseId)
               .ToList();

            var sectionInstructors = section.Instructors.Select(x => x.InstructorId);
            var instructors = _employeeRepo.Query()
                .Include(x => x.Localizations.Where(w => w.Language == language))
                .Where(x => sectionInstructors != null && sectionInstructors.Contains(x.Id))
                .ToList();

            var classScheduleTermDetail = MapClassScheduleTermDetailToViewModel(section, studentRegisLists, instructors);

            return classScheduleTermDetail;
        }

        public List<ExaminationScheduleViewModel> GetExamScheduleByDate(Guid studentId, LanguageCode language, Guid termId, DateTime? startDate, DateTime? endDate)
        {
            if (startDate is null || endDate is null)
            {
                throw new ScheduleException.NotFound();
            }

            var studentRegisLists = _studyCourseRepo.Query()
                                      .Include(x => x.Section)
                                      .ThenInclude(x => x.SectionExaminations
                                      .Where(w => w.Date.HasValue
                                                  && w.Date.Value.Date >= startDate
                                                  && w.Date.Value.Date < endDate))
                                      .ThenInclude(x => x.Room)
                                      .ThenInclude(x => x.Localizations.Where(w => w.Language == language))
                                      .Include(x => x.Section)
                                      .ThenInclude(x => x.Instructors)
                                      .Include(x => x.Course)
                                      .ThenInclude(x => x.Localizations.Where(w => w.Language == language))
                                      .Where(x => x.TermId == termId
                                                  && x.StudentId == studentId
                                                  && x.Status == StudyCourseStatus.ACTIVE
                                                  && x.Section != null && x.Section.Status == SectionStatus.OPEN)
                                      .OrderBy(x => x.CourseId)
                                      .ToList();

            if (studentRegisLists is null)
            {
                throw new ScheduleException.NotFound();
            }

            var examInPeriods = MapExamScheduleToViewModel(studentRegisLists, language);

            return examInPeriods;
        }

        #region Private Function
        private List<ClassScheduleViewModel> MapClassScheduleToViewModel(List<StudyCourse> studyCourses, List<SectionSchedule> sectionSchedules)
        {
            studyCourses = studyCourses.OrderBy(x => x.CourseId).ToList();

            var classSchedules = new List<ClassScheduleViewModel>();
            int indexColor = 0;

            foreach (var data in studyCourses)
            {
                var localizeCourse = data.Course?.Localizations?.FirstOrDefault();
                var schedules = sectionSchedules.Where(x => x.SectionId == data.SectionId);

                foreach (var schedule in schedules)
                {
                    var locations = schedule.Room?.Localizations?.FirstOrDefault();
                    ClassScheduleViewModel model = new ClassScheduleViewModel
                    {
                        Id = schedule.Id,
                        StartAt = schedule.StartAt,
                        EndAt = schedule.EndAt,
                        StartTime = schedule.StartAt.ToString("HH:mm"),
                        EndTime = schedule.EndAt.ToString("HH:mm"),
                        Color = CourseColors[indexColor],
                        CourseCode = data.Course?.Code,
                        CourseName = localizeCourse != null ? localizeCourse.Name : data.Course?.Name,
                        Section = schedule.Section.SectionNo,
                        Location = locations != null ? locations.Name : schedule.Room?.Name
                    };
                    classSchedules.Add(model);
                }

                indexColor++;
            }

            return classSchedules;
        }

        private List<ClassScheduleTermViewModel> MapClassScheduleTermToViewModel(List<StudyCourse> studyCourses)
        {
            List<ClassScheduleTermViewModel> results = new List<ClassScheduleTermViewModel>();
            int indexColor = 0;
            foreach (var data in studyCourses)
            {
                if (!data.SectionId.HasValue) { continue; }

                var countMember = _studyCourseRepo.Query()
                    .Where(x => x.SectionId == data.SectionId
                                && x.TermId == data.TermId
                                && x.Status == StudyCourseStatus.ACTIVE)
                    .Count();
                var courseName = data.Course?.Localizations?.FirstOrDefault();
                var locations = data?.Section?.SectionClassPeriods
                    .DistinctBy(x => x.RoomId)
                    .Select(x => x?.Room?.Localizations?.FirstOrDefault()?.Name)
                    .ToList();

                ClassScheduleTermViewModel model = new ClassScheduleTermViewModel
                {
                    Id = data.SectionId.Value,
                    Color = CourseColors[indexColor],
                    CourseCode = data.Course?.Code,
                    CourseName = courseName != null ? courseName.Name : data.Course?.Name,
                    CountMember = countMember,
                    Credit = data.Credit,
                    Section = data.Section != null ? data.Section.SectionNo : null,
                    Location = locations != null ? string.Join(",", locations) : null
                };

                results.Add(model);
                indexColor++;
            }

            return results;
        }

        private ClassScheduleDetailViewModel MapClassScheduleDetailToViewModel(SectionSchedule schedule, List<Guid>? studentRegisLists, List<Employee> instructors)
        {
            int indexColor = studentRegisLists != null ? studentRegisLists.FindIndex(x => x.Equals(schedule.Section.CourseId)) : 0;
            var courseLocalize = schedule.Section.Course?.Localizations?.FirstOrDefault();
            var locations = schedule.Room?.Localizations?.FirstOrDefault();

            var result = new ClassScheduleDetailViewModel
            {
                Id = schedule.Id,
                StartAt = schedule.StartAt,
                EndAt = schedule.EndAt,
                StartTime = schedule.StartAt.ToString("HH:mm"),
                EndTime = schedule.EndAt.ToString("HH:mm"),
                FromDate = schedule.Section.StartedAt,
                ToDate = schedule.Section.EndedAt,
                Color = CourseColors[indexColor],
                CourseCode = schedule.Section.Course?.Code,
                CourseName = courseLocalize != null ? courseLocalize.Name : schedule.Section.Course?.Name,
                CourseDescription = courseLocalize != null ? courseLocalize.Description : schedule.Section.Course?.Description,
                Credit = schedule.Section.Course?.Credit,
                Section = schedule.Section.SectionNo,
                Location = locations != null ? locations.Name : schedule.Room?.Name,
                ClassLink = null, // TO DO: Waiting Plexus Develop
                Instructors = (from data in instructors
                               let localizeInstructor = data.Localizations?.FirstOrDefault()
                               select new InstructorScheduleViewModel
                               {
                                   Id = data.Id,
                                   Title = data.Title,
                                   FirstName = localizeInstructor != null ? localizeInstructor.FirstName : data.FirstName,
                                   MiddleName = localizeInstructor != null ? localizeInstructor.MiddleName : data.MiddleName,
                                   LastName = localizeInstructor != null ? localizeInstructor.LastName : data.LastName,
                                   ProfileImageUrl = data.CardImageUrl
                               }
                               ).ToList()
            };

            return result;
        }

        private ClassScheduleTermDetailViewModel MapClassScheduleTermDetailToViewModel(Section section, List<Guid> studentRegisLists, List<Employee> instructors)
        {
            int indexColor = studentRegisLists != null ? studentRegisLists.FindIndex(x => x.Equals(section.CourseId)) : 0;
            var courseLocalize = section.Course?.Localizations?.FirstOrDefault();
            var locations = section.SectionClassPeriods
                    .DistinctBy(x => x.RoomId)
                    .Select(x => x?.Room?.Localizations?.FirstOrDefault()?.Name)
                    .ToList();

            var result = new ClassScheduleTermDetailViewModel
            {
                Id = section.Id,
                FromDate = section.StartedAt,
                ToDate = section.EndedAt,
                TimeDetail = (from data in section.SectionClassPeriods
                              select new TimeDetail
                              {
                                  DayOfWeek = data.Day,
                                  StartTime = data.StartTime.ToString(@"hh\:mm"),
                                  EndTime = data.EndTime.ToString(@"hh\:mm")
                              }).ToList(),
                Color = CourseColors[indexColor],
                CourseCode = section.Course?.Code,
                CourseName = courseLocalize != null ? courseLocalize.Name : section.Course?.Name,
                CourseDescription = courseLocalize != null ? courseLocalize.Description : section.Course?.Description,
                Credit = section.Course != null ? section.Course.Credit : 0,
                Section = section.SectionNo,
                Location = locations != null ? string.Join(",", locations) : null,
                ClassLink = null, //TO DO : Waiting Plexus Develop
                Instructors = (from data in instructors
                               let localizeInstructor = data.Localizations?.FirstOrDefault()
                               select new InstructorScheduleViewModel
                               {
                                   Id = data.Id,
                                   Title = data.Title,
                                   FirstName = localizeInstructor != null ? localizeInstructor.FirstName : data.FirstName,
                                   MiddleName = localizeInstructor != null ? localizeInstructor.MiddleName : data.MiddleName,
                                   LastName = localizeInstructor != null ? localizeInstructor.LastName : data.LastName,
                                   ProfileImageUrl = data.CardImageUrl
                               }
                               ).ToList()
            };

            return result;
        }

        private List<ExaminationScheduleViewModel> MapExamScheduleToViewModel(List<StudyCourse> studentRegisLists, LanguageCode language)
        {
            List<ExaminationScheduleViewModel> result = new List<ExaminationScheduleViewModel>();

            var courseHavExam = studentRegisLists
                .Where(x => x.Section?.SectionExaminations != null
                            && x.Section.SectionExaminations.Count() > 0)
                .ToList();

            foreach (StudyCourse studyCourse in courseHavExam)
            {
                var localizeCourse = studyCourse.Course.Localizations?.FirstOrDefault();

                var sectionInstructors = studyCourse.Section?.Instructors.Select(x => x.InstructorId);
                var instructors = _employeeRepo.Query()
                    .Include(x => x.Localizations.Where(w => w.Language == language))
                    .Where(x => sectionInstructors != null && sectionInstructors.Contains(x.Id))
                    .ToList();

                var exam = (from data in studyCourse.Section?.SectionExaminations
                            let dateExam = data.Date.Value
                            let startTimeExam = data.StartTime.Value
                            let endTimeExam = data.EndTime.Value
                            let locations = data.Room?.Localizations?.FirstOrDefault()
                            select new ExaminationScheduleViewModel
                            {
                                Id = data.Id,
                                StartAt = dateExam.Date.Add(startTimeExam),
                                EndAt = dateExam.Date.Add(endTimeExam),
                                StartTime = startTimeExam.ToString(@"hh\:mm"),
                                EndTime = endTimeExam.ToString(@"hh\:mm"),
                                ExamType = data.ExamType,
                                CourseCode = studyCourse.Course.Code,
                                CourseName = localizeCourse != null ? localizeCourse.Name : studyCourse.Course.Name,
                                Credit = studyCourse.Credit,
                                Section = studyCourse.Section.SectionNo,
                                Location = locations != null ? locations.Name : data.Room?.Name,
                                Proctor = (from instructor in instructors
                                           let localizeInstructor = instructor.Localizations?.FirstOrDefault()
                                           select new ScheduleProctorViewModel
                                           {
                                               Id = instructor.Id,
                                               Title = instructor.Title,
                                               FirstName = localizeInstructor != null ? localizeInstructor.FirstName : instructor.FirstName,
                                               MiddleName = localizeInstructor != null ? localizeInstructor.MiddleName : instructor.MiddleName,
                                               LastName = localizeInstructor != null ? localizeInstructor.LastName : instructor.LastName,
                                               ProfileImageUrl = instructor.CardImageUrl
                                           }).ToList()
                            }
                            ).ToList();

                result.AddRange(exam);
            }

            return result;
        }
        #endregion
    }
}
