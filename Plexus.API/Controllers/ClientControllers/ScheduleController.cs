using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plexus.Service;
using Plexus.Service.Exception;
using Plexus.Service.src;
using Plexus.Utility;
using System.Globalization;
using System.Net;

namespace Plexus.API.Controllers.ClientControllers
{
    [Authorize]
    [ApiController]
    [Route(_clientPathPrefix + "[controller]")]
    public class ScheduleController : BaseController
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet("class")]
        [Authorize(Roles = "STUDENT")]
        public IActionResult GetClassScheduleByDate(DateTime? startdate, DateTime? enddate)
        {
            var language = GetRequestLanguage();
            var user = GetRequesterInformation();
            var schedules = _scheduleService.GetClassScheduleByDate(user.StudentId.Value, language,startdate, enddate);
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, schedules));
        }

        [HttpGet("term/{id}")]
        [Authorize(Roles = "STUDENT")]
        public IActionResult GetClassScheduleByTerm(Guid id)
        {
            var language = GetRequestLanguage();
            var user = GetRequesterInformation();
            var schedules = _scheduleService.GetClassScheduleByTerm(user.StudentId.Value, language, id);
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, schedules));
        }

        [HttpGet("class/{id}/detail")]
        [Authorize(Roles = "STUDENT")]
        public IActionResult GetClassScheduleDetailById(Guid id)
        {
            var language = GetRequestLanguage();
            var user = GetRequesterInformation();
            var schedules = _scheduleService.GetClassScheduleDetailById(user.StudentId.Value, language, id);
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, schedules));
        }

        [HttpGet("term/{term_id}/class/{class_id}/detail")]
        [Authorize(Roles = "STUDENT")]
        public IActionResult GetClassScheduleDetailByTermIdAndClassId(Guid term_id, Guid class_id)
        {
            var language = GetRequestLanguage();
            var user = GetRequesterInformation();
            var schedules = _scheduleService.GetClassScheduleDetailByTermAndClassId(user.StudentId.Value, language, term_id, class_id);
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, schedules));
        }

        [HttpGet("examination")]
        [Authorize(Roles = "STUDENT")]
        public IActionResult GetClassScheduleDetailByTermIdAndClassId(string startmonth, string endmonth,Guid termid)
        {
            string format = "yyyy-MM";
            DateTime startDate;
            DateTime endDate;

            bool isCovertStartDate = DateTime
                .TryParseExact(
                    startmonth,
                    format, 
                    CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, 
                    out startDate);

            bool isCovertEndDate = DateTime
               .TryParseExact(
                   endmonth,
                   format,
                   CultureInfo.InvariantCulture,
                   DateTimeStyles.None,
                   out endDate);

            if (!isCovertStartDate || !isCovertEndDate) 
            {
                throw new ScheduleException.InvalidDateFormat();
            }


            var language = GetRequestLanguage();
            var user = GetRequesterInformation();
            var schedules = _scheduleService.GetExamScheduleByDate(user.StudentId.Value, language, termid, startDate, endDate);
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, schedules));
        }
    }
}
