using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plexus.Service;
using Plexus.Utility;

namespace Plexus.API.Controllers.Client
{
    [Authorize]
    [ApiController]
    [Route(_clientPathPrefix + "[controller]")]
    public class AdvisingController : BaseController
    {
        private readonly IAdvisingService _advisingService;

        public AdvisingController(IAdvisingService advisingService)
        {
            _advisingService = advisingService;
        }

        [Authorize(Roles = "STUDENT")]
        [HttpGet("advisor")]
        public IActionResult GetAdvisorProfile()
        {
            var language = GetRequestLanguage();
            var tokenInformation = GetRequesterInformation();
            var advisor = _advisingService.GetAdvisorProfile(tokenInformation.StudentId.Value, language);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, advisor));
        }

        [Authorize(Roles = "STUDENT")]
        [HttpGet("advisor/{advisorId}/slots/current")]
        public IActionResult GetUpcomingAppointmentSlot(Guid advisorId)
        {
            var tokenInformation = GetRequesterInformation();
            var appointmentSlots = _advisingService.GetUpcomingAppointmentSlots(tokenInformation.StudentId.Value, advisorId);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, appointmentSlots));
        }

        [Authorize(Roles = "STUDENT")]
        [HttpGet("advisor/{advisorId}/slots/history")]
        public IActionResult GetAppointmentSlotHistory(Guid advisorId)
        {
            var tokenInformation = GetRequesterInformation();
            var appointmentSlots = _advisingService.GetAppointmentSlotHistory(tokenInformation.StudentId.Value, advisorId);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, appointmentSlots));
        }

        [Authorize(Roles = "STUDENT")]
        [HttpPost("slots/{slotId}/book")]
        public IActionResult RequestBookAdvisingSlots(Guid slotId)
        {
            var tokenInformation = GetRequesterInformation();
            _advisingService.BookAdvisingSlot(slotId, tokenInformation.StudentId.Value);
            return StatusCode(201,ResponseWrapper.Success(HttpStatusCode.Created));
        }

        [Authorize(Roles = "STUDENT")]
        [HttpDelete("slots/{slotId}/book")]
        public IActionResult ReleaseBookedAdvisingSlots(Guid slotId)
        {
            var tokenInformation = GetRequesterInformation();
            _advisingService.UnbookAdvisingSlot(slotId, tokenInformation.StudentId.Value);
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [Authorize(Roles = "STUDENT")]
        [HttpGet("me/status")]
        public IActionResult GetSelfAdvisingInformation()
        {
            var language = GetRequestLanguage();
            var tokenInformation = GetRequesterInformation();
            var advisingInformation = _advisingService.GetAdvisingInformation(tokenInformation.StudentId.Value, language);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, advisingInformation));
        }

        [HttpGet("{studentId}/status")]
        public IActionResult GetStudentAdvisingInformation(Guid studentId)
        {
            var language = GetRequestLanguage();
            var advisingInformation = _advisingService.GetAdvisingInformation(studentId, language);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, advisingInformation));
        }
    }
}