using System.Net;
using Plexus.Database.Enum.Academic;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class RegistrationException : BaseCustomException
    {
        public RegistrationException(string message, HttpStatusCode statusCode) : base(message, "REG", statusCode) { }

        public class InvalidSections : RegistrationException
        {
            public InvalidSections() : base("Given section list is invalid.", HttpStatusCode.Forbidden) { }
        }

        public class DuplicateCourse : RegistrationException
        {
            public DuplicateCourse() : base("Given course list has duplicate course, please check the data.", HttpStatusCode.Conflict) { }
        }

        public class DuplicateSection : RegistrationException
        {
            public DuplicateSection() : base("Given section list has duplicate section, please check the data.", HttpStatusCode.Conflict) { }
        }

        public class NoAvailableSeat : RegistrationException
        {
            public NoAvailableSeat(Guid sectionId) : base($"No available seat for given section id ({sectionId}).", HttpStatusCode.Forbidden) { }
        }

        public class CloseSection : RegistrationException
        {
            public CloseSection(Guid sectionId) : base($"Section with given id ({sectionId}) is closed.", HttpStatusCode.Forbidden) { }
        }

        public class ExamTimeConflict : RegistrationException
        {
            public ExamTimeConflict(ExamType type, string baseCourse, string baseSection, string compareCourse, string compareSection
                                    , DateTime date, TimeSpan startTime, TimeSpan endTime)
                : base($"{type.ToString()} : {baseCourse} ({baseSection}) & {compareCourse} ({compareSection}) on {date.ToString("dd/MM/yyyy")} at {startTime.ToString(@"hh\:mm")} - {endTime.ToString(@"hh\:mm")}"
                       , HttpStatusCode.BadRequest)
            { }
        }

        public class ClassTimeConflict : RegistrationException
        {
            public ClassTimeConflict(string baseCourse, string baseSection, string compareCourse, string compareSection
                                     , DayOfWeek day, TimeSpan startTime, TimeSpan endTime)
                : base($"{baseCourse} ({baseSection}) & {compareCourse} ({compareSection}) on {day.ToString()} at {startTime.ToString(@"hh\:mm")} - {endTime.ToString(@"hh\:mm")}"
                       , HttpStatusCode.BadRequest)
            { }
        }
    }
}