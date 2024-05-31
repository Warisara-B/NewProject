using Plexus.Database.Enum;
using Plexus.Service.ViewModel;

namespace Plexus.Service
{
    public interface IAcademicCalendarService
    {
        /// <summary>
        /// Get list of academic calendars by studentId.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="language"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        AcademicCalendarViewModel GetAcademicCalendarsByStudentId(Guid studentId, LanguageCode language, DateTime? date);
    }
}