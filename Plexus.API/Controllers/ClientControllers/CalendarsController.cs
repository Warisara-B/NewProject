using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plexus.Database.Enum;
using Plexus.Service;
using Plexus.Utility;

namespace Plexus.API.Controllers.ClientControllers
{
    [Authorize]
    [ApiController]
    [Route(_clientPathPrefix + "[controller]")]
    public class CalendarsController : BaseController
    {
        private readonly IAcademicCalendarService _calendarService;

        public CalendarsController(IAcademicCalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [HttpGet]
        [Authorize(Roles = "STUDENT")]
        public IActionResult GetCalendarEvents([FromQuery] DateTime? date)
        {
            var language = GetRequestLanguage();
            var tokenInformation = GetRequesterInformation();
            var calendars = _calendarService.GetAcademicCalendarsByStudentId(tokenInformation.StudentId.Value, language, date);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, calendars));
        }
    }
}