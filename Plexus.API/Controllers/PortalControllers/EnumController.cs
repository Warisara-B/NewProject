using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic;
using Plexus.Database.Enum.Academic.Curriculum;
using Plexus.Database.Enum.Academic.Section;
using Plexus.Database.Enum.Facility.Reservation;
using Plexus.Database.Enum.Payment;
using Plexus.Database.Enum.Registration;
using Plexus.Database.Enum.Research;
using Plexus.Database.Enum.Student;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class EnumController : BaseController
    {
        public EnumController() { }

        [HttpGet("termTypes")]
        public IActionResult GetTermTypes()
        {
            var termTypes = Enum.GetNames(typeof(TermType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, termTypes));
        }

        [HttpGet("periodTypes")]
        public IActionResult GetPeriodTypes()
        {
            var periodTypes = Enum.GetNames(typeof(PeriodType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, periodTypes));
        }

        [HttpGet("courseGroupTypes")]
        public IActionResult GetCourseGroupTypes()
        {
            var courseGroupTypes = Enum.GetNames(typeof(CourseGroupType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courseGroupTypes));
        }

        [HttpGet("languages")]
        public IActionResult GetSupportLanguage()
        {
            var languages = Enum.GetNames(typeof(LanguageCode));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, languages));
        }

        [HttpGet("specializationTypes")]
        public IActionResult GetSpecializationTypes()
        {
            var specializationTypes = Enum.GetNames(typeof(SpecializationType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, specializationTypes));
        }

        [HttpGet("collegeCalendarTypes")]
        public IActionResult GetCollegeCalendarTypes()
        {
            var collegeCalendarTypes = Enum.GetNames(typeof(CollegeCalendarType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, collegeCalendarTypes));
        }

        [HttpGet("relationships")]
        public IActionResult GetRelationships()
        {
            var relationships = Enum.GetNames(typeof(Relationship));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, relationships));
        }

        [HttpGet("examTypes")]
        public IActionResult GetExamTypes()
        {
            var examTypes = Enum.GetNames(typeof(ExamType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, examTypes));
        }

        [HttpGet("dayOfWeeks")]
        public IActionResult GetDayOfWeek()
        {
            var dayOfWeeks = Enum.GetNames(typeof(DayOfWeek));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, dayOfWeeks));
        }

        [HttpGet("studyCourseStatus")]
        public IActionResult GetStudyCourseStatus()
        {
            var stutuses = Enum.GetNames(typeof(StudyCourseStatus));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, stutuses));
        }

        [HttpGet("recurringTypes")]
        public IActionResult GetRecurringType()
        {
            var types = Enum.GetNames(typeof(RecurringType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, types));
        }

        [HttpGet("termFeePackageTypes")]
        public IActionResult GetTermFeePackageTypes()
        {
            var types = Enum.GetNames(typeof(TermFeePackageType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, types));
        }

        [HttpGet("genders")]
        public IActionResult GetGenders()
        {
            var genders = Enum.GetNames(typeof(Gender));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, genders));
        }

        [HttpGet("seatTypes")]
        public IActionResult GetSeatTypes()
        {
            var types = Enum.GetNames(typeof(SeatType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, types));
        }

        [HttpGet("sectionStatuses")]
        public IActionResult GetSectionStatuses()
        {
            var statuses = Enum.GetNames(typeof(SectionStatus));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, statuses));
        }

        [HttpGet("senderTypes")]
        public IActionResult GetSenderTypes()
        {
            var types = Enum.GetNames(typeof(SenderType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, types));
        }

        [HttpGet("usageTypes")]
        public IActionResult GetUsageTypes()
        {
            var types = Enum.GetNames(typeof(UsageType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, types));
        }

        [HttpGet("reservationStatuses")]
        public IActionResult GetReservationStatuss()
        {
            var statuses = Enum.GetNames(typeof(ReservationStatus));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, statuses));
        }

        [HttpGet("registrationChannels")]
        public IActionResult GetRegistrationChannels()
        {
            var channel = Enum.GetNames(typeof(RegistrationChannel));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, channel));
        }

        [HttpGet("prerequisiteConditions")]
        public IActionResult GetPrerequisiteConditions()
        {
            var conditions = Enum.GetNames(typeof(PrerequisiteCondition));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, conditions));
        }

        [HttpGet("prerequisiteConditionTypes")]
        public IActionResult GetPrerequisiteConditionTypes()
        {
            var types = Enum.GetNames(typeof(PrerequisiteConditionType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, types));
        }

        [HttpGet("calculationTypes")]
        public IActionResult GetCalculationTypes()
        {
            var types = Enum.GetNames(typeof(CalculationType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, types));
        }

        [HttpGet("addressTypes")]
        public IActionResult GetAddressTypes()
        {
            var types = Enum.GetNames(typeof(AddressType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, types));
        }

        [HttpGet("researchSequenceTypes")]
        public IActionResult GetResearchSequenceType()
        {
            var types = Enum.GetNames(typeof(ResearchSequenceType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, types));
        }

        [HttpGet("committeePositions")]
        public IActionResult GetCommitteePosition()
        {
            var positions = Enum.GetNames(typeof(CommitteePosition));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, positions));
        }

        [HttpGet("researchDefenseStatuses")]
        public IActionResult GetResearchDefenseStatus()
        {
            var statuses = Enum.GetNames(typeof(ResearchDefenseStatus));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, statuses));
        }

        [HttpGet("appointmentTypes")]
        public IActionResult GetAppointmentType()
        {
            var types = Enum.GetNames(typeof(ResearchAppointmentType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, types));
        }

        [HttpGet("researchStatuses")]
        public IActionResult GetResearchStatus()
        {
            var statuses = Enum.GetNames(typeof(ResearchStatus));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, statuses));
        }

        [HttpGet("academicStatuses")]
        public IActionResult GetAcademicStatus()
        {
            var statuses = Enum.GetNames(typeof(AcademicStatus));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, statuses));
        }

        [HttpGet("expertiseTypes")]
        public IActionResult GetExpertiseType()
        {
            var types = Enum.GetNames(typeof(ExpertiseType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, types));
        }

        [HttpGet("employeeTypes")]
        public IActionResult GetEmployeeType()
        {
            var types = Enum.GetNames(typeof(EmployeeType));

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, types));
        }
    }
}