using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Registration;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
	[Route(_pathPrefix + "[controller]")]
    public class RegistrationsController : BaseController
    {
        private readonly IRegistrationManager _registrationManager;
        private readonly IPrerequisiteManager _prerequisiteManager;

        public RegistrationsController(IRegistrationManager registrationManager,
                                       IPrerequisiteManager prerequisiteManager)
        {
            _registrationManager = registrationManager;
            _prerequisiteManager = prerequisiteManager;
        }

        [HttpGet("students/{id}")]
        public IActionResult GetByStudent(Guid id, Guid? termId)
        {
            var studyCourses = _registrationManager.GetByStudent(id, termId, false);

            if (studyCourses is null || !studyCourses.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, studyCourses));
        }

        [HttpGet("students/{id}/logs")]
        public IActionResult GetLogByStudent(Guid id, Guid termId, int page = 1, int pageSize = 25)
        {
            var pagedLog = _registrationManager.GetLogs(id, termId, page, pageSize);
            
            if (pagedLog is null || !pagedLog.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedLog));
        }

        [HttpPut("verifyPrerequisites")]
        public IActionResult VerifyPrerequisite([FromBody] RegistrationViewModel request)
        {
            _prerequisiteManager.VerifyPrerequisite(request);
            
            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpPut]
		public IActionResult Update([FromBody] RegistrationViewModel request)
        {
			_registrationManager.Update(request, Guid.Empty);

			return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpPut("verifyExaminations")]
        public IActionResult VerifySectionExaminations([FromBody] IEnumerable<RegistrationCourseViewModel>? requests)
        {
            _registrationManager.VerifySectionExaminations(requests);

			return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpPut("verifyClasses")]
        public IActionResult VerifyClassTimes([FromBody] IEnumerable<RegistrationCourseViewModel>? requests)
        {
            _registrationManager.VerifyClassTimes(requests);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}