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
    public class StudentsController : BaseController
    {
        private readonly IStudentService _studentService;

        private readonly IGradeService _gradeService;
        public StudentsController(IStudentService studentService, IGradeService gradeService)
        {
            _studentService = studentService;
            _gradeService = gradeService;
        }

        [Authorize(Roles = "STUDENT")]
        [HttpGet("me/profile/card")]
        public IActionResult GetSelfStudentCard()
        {
            var language = GetRequestLanguage();
            var tokenInformation = GetRequesterInformation();
            var student = _studentService.GetStudentCardById(tokenInformation.StudentId.Value, language);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, student));
        }

        [HttpGet("{studentId}/profile/card")]
        public IActionResult GetStudentCardById(Guid studentId)
        {
            var language = GetRequestLanguage();
            var student = _studentService.GetStudentCardById(studentId, language);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, student));
        }

        [Authorize(Roles = "STUDENT")]
        [HttpGet("me/profile/detail")]
        public IActionResult GetSelfStudentFullProfile()
        {
            var language = GetRequestLanguage();
            var tokenInformation = GetRequesterInformation();
            var student = _studentService.GetStudentFullProfileById(tokenInformation.StudentId.Value, language);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, student));
        }

        [HttpGet("{studentId}/profile/detail")]
        public IActionResult GetStudentFullProfileById(Guid studentId)
        {
            var language = GetRequestLanguage();
            var student = _studentService.GetStudentFullProfileById(studentId, language);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, student));
        }

        [Authorize(Roles = "STUDENT")]
        [HttpGet("me/grades/term")]
        public IActionResult GetSelfGradeByTerm()
        {
            var language = GetRequestLanguage();
            var tokenInformation = GetRequesterInformation();
            var student = _gradeService.GetGradeByTerm(tokenInformation.StudentId.Value, language);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, student));
        }

        [HttpGet("{studentId}/grades/term")]
        public IActionResult GetGradeByTerm(Guid studentId)
        {
            var language = GetRequestLanguage();
            var grades = _gradeService.GetGradeByTerm(studentId, language);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, grades));
        }

        [HttpGet("{studentId}/grades/curriculum")]
        public IActionResult GetGradeByCurriculum(Guid studentId)
        {
            var language = GetRequestLanguage();
            var grades = _gradeService.GetGradeByCurriculum(studentId, language);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, grades));
        }

        [Authorize(Roles = "STUDENT")]
        [HttpGet("me/grades/curriculum")]
        public IActionResult GetSelfGradeByCurriculum()
        {
            var language = GetRequestLanguage();
            var tokenInformation = GetRequesterInformation();
            var grades = _gradeService.GetGradeByCurriculum(tokenInformation.StudentId.Value, language);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, grades));
        }
    }
}