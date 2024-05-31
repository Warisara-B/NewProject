using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Enum;
using Plexus.Service.ViewModel;
using Plexus.Database.Enum.Academic;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Advising;

namespace Plexus.Service.src
{
    public class AcademicCalendarService : IAcademicCalendarService
    {
        private readonly IAsyncRepository<AcademicCalendar> _academicCalendarRepo;
        private readonly IAsyncRepository<AdvisingSlot> _advisingSlotRepo;

        public AcademicCalendarService(IAsyncRepository<AdvisingSlot> advisingSlotRepo, IAsyncRepository<AcademicCalendar> academicCalendarRepo)
        {
            _advisingSlotRepo = advisingSlotRepo;
            _academicCalendarRepo = academicCalendarRepo;
        }

        public AcademicCalendarViewModel GetAcademicCalendarsByStudentId(Guid studentId, LanguageCode language, DateTime? date)
        {
            var queryDate = date is null ? DateTime.UtcNow : date.Value;

            var academic_calendars = GetAcademicCalendarViewModel(queryDate).ToList();
            var advising_calendars = GetAdvisingSlotsCalendarViewModel(queryDate, studentId).ToList();

            List<AcademicCalendarEventViewModel> events = new List<AcademicCalendarEventViewModel>();
            events.AddRange(academic_calendars);
            events.AddRange(advising_calendars);

            if (events != null && events.Count > 0)
            {
                events = events.OrderBy(x => x.IsAllDay).ThenBy(x => x.EndedAt).ThenBy(x => x.StartedAt).ToList();
                events.ElementAt(0).IsHighlighted = true;
            }

            return new AcademicCalendarViewModel
            {
                Date = DateOnly.FromDateTime(queryDate),
                Events = events
            };
        }

        private IEnumerable<AcademicCalendarEventViewModel> GetAcademicCalendarViewModel(DateTime date)
        {
            var academic_calendars = _academicCalendarRepo.Query()
               .Where(x => date.Date >= x.StartedAt.Value.Date
                           && date.Date < x.EndedAt.Value).ToList();

            var events = academic_calendars is null ?
                Enumerable.Empty<AcademicCalendarEventViewModel>()
                : (from calendar in academic_calendars
                   select new AcademicCalendarEventViewModel
                   {
                       Type = calendar.Type,
                       StartedAt = (calendar.StartedAt.Value.Date == date.Date) ? calendar.StartedAt : date.Date,
                       EndedAt = (calendar.EndedAt.Value.Date == date.Date) ? calendar.EndedAt : date.Date.AddHours(23).AddMinutes(59),
                       CardStartTime = (calendar.StartedAt.Value.Date == date.Date) ? calendar.StartedAt.Value.ToString("HH:mm") : date.Date.ToString("HH:mm"),
                       CardEndTime = (calendar.EndedAt.Value.Date == date.Date) ? calendar.EndedAt.Value.ToString("HH:mm") : date.Date.AddHours(23).AddMinutes(59).ToString("HH:mm"),
                       IsAllDay = CheckIsAllDay(date, calendar.StartedAt.Value, calendar.EndedAt.Value),
                       Title = calendar.Title,
                       Description = calendar.Description,
                       Instructors = Enumerable.Empty<AcademicCalendarInstructorViewModel>(),
                       Location = calendar.Location
                   }).ToList();

            return events;
        }

        private IEnumerable<AcademicCalendarEventViewModel> GetAdvisingSlotsCalendarViewModel(DateTime date, Guid studentId)
        {
            var advising_calendars = _advisingSlotRepo.Query()
               .Include(x => x.Instructor)
               .Where(x => x.StudentId == studentId
                           && date.Date >= x.StartedAt.Date
                           && date.Date < x.EndedAt)
               .ToList();

            var events = advising_calendars is null ?
                Enumerable.Empty<AcademicCalendarEventViewModel>()
                : (from calendar in advising_calendars
                   select new AcademicCalendarEventViewModel
                   {
                       Type = AcademicCalendarEventType.ADVISING,
                       StartedAt = (calendar.StartedAt.Date == date.Date) ? calendar.StartedAt : date.Date,
                       EndedAt = (calendar.EndedAt.Date == date.Date) ? calendar.EndedAt : date.Date.AddHours(23).AddMinutes(59),
                       CardStartTime = (calendar.StartedAt.Date == date.Date) ? calendar.StartedAt.ToString("HH:mm") : date.Date.ToString("HH:mm"),
                       CardEndTime = (calendar.EndedAt.Date == date.Date) ? calendar.EndedAt.ToString("HH:mm") : date.Date.AddHours(23).AddMinutes(59).ToString("HH:mm"),
                       IsAllDay = CheckIsAllDay(date, calendar.StartedAt, calendar.EndedAt),
                       Title = "Advising Appointment",
                       Description = null,
                       Instructors = Enumerable.Empty<AcademicCalendarInstructorViewModel>(),
                       Location = null
                   }).ToList();

            return events;
        }

        private bool CheckIsAllDay(DateTime cur, DateTime start, DateTime end)
        {
            bool result;
            bool IsSameStartDay = cur.Date == start.Date;
            bool IsSameEndDay = cur.Date == end.Date;

            if (IsSameStartDay && IsSameEndDay)
            {
                bool IsAllDay = (start == start.Date && end == end.Date.AddHours(23).AddMinutes(59));
                result = IsAllDay;
            }
            else if (IsSameStartDay)
            {
                bool IsAllDay = (start == start.Date);
                result = IsAllDay;
            }
            else if (IsSameEndDay)
            {
                bool IsAllDay = (end == end.Date.AddHours(23).AddMinutes(59));
                result = IsAllDay;
            }
            else
            {
                bool IsInProgress = (cur.Date >= start.Date);
                result = IsInProgress;
            }

            return result;
        }
    }
}
