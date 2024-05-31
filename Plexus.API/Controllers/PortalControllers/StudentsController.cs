using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.Payment.Scholarship;
using Plexus.Client.ViewModel.Registration;
using Plexus.Entity.DTO;
using Plexus.Utility;
using ServiceStack.Text;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class StudentsController : BaseController
    {
        private readonly IStudentManager _studentManager;
        private readonly IStudentGuardianManager _guardianManager;
        private readonly IStudentAddressManager _addressManager;
        private readonly IStudyCourseManager _studyCourseManager;
        private readonly IStudentTermManager _studentTermManager;
        private readonly IStudentScholarshipManager _studentScholarshipManager;
        private readonly IStudentCourseTrackManager _studentCourseTrackManager;
        private readonly IRegistrationManager _registrationManager;
        private readonly ITransferCourseManager _transferCourseManager;

        public StudentsController(IStudentManager studentManager,
                                  IStudentAddressManager addressManager,
                                  IStudentGuardianManager guardianManager,
                                  IStudyCourseManager studyCourseManager,
                                  IStudentTermManager studentTermManager,
                                  IStudentScholarshipManager studentScholarshipManager,
                                  IStudentCourseTrackManager studentCourseTrackManager,
                                  IRegistrationManager registrationManager,
                                  ITransferCourseManager transferCourseManager)
        {
            _studentManager = studentManager;
            _guardianManager = guardianManager;
            _addressManager = addressManager;
            _studyCourseManager = studyCourseManager;
            _studentTermManager = studentTermManager;
            _studentScholarshipManager = studentScholarshipManager;
            _studentCourseTrackManager = studentCourseTrackManager;
            _registrationManager = registrationManager;
            _transferCourseManager = transferCourseManager;
        }

        // TODO: Get user id
        [HttpPost]
        public IActionResult Create([FromBody] CreateStudentViewModel request)
        {
            var student = _studentManager.Create(request, Guid.Empty);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, student));
        }

        [HttpGet("search")]
        public IActionResult SearchStudent([FromQuery] SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedStudent = _studentManager.Search(parameters, page, pageSize);

            if (pagedStudent is null || !pagedStudent.Items.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, pagedStudent));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var student = _studentManager.GetById(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, student));
        }

        [HttpGet("code/{code}")]
        public IActionResult GetByCode(string code)
        {
            var student = _studentManager.GetByCode(code);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, student));
        }

        [HttpPost("files")]
        public IActionResult GetByFiles([FromForm] IFormFile file)
        {
            var students = _studentManager.GetByFile(file);

            if (students is null || !students.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, students));
        }

        // TODO: Get user id
        [HttpPut]
        public IActionResult Update([FromBody] StudentViewModel request)
        {
            var student = _studentManager.Update(request, Guid.Empty);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, student));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _studentManager.Delete(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpPut("{id}/cardImage")]
        public async Task<IActionResult> UploadCardImageAsync(Guid id, [FromForm] IFormFile cardImage)
        {
            await _studentManager.UploadCardImageAsync(id, cardImage);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK));
        }

        // TODO: Get user id
        [HttpPost("{studentId}/guardians")]
        public IActionResult CreateGuardian(Guid studentId, [FromBody] CreateStudentGuardianViewModel request)
        {
            var guardian = _guardianManager.Create(studentId, request, Guid.Empty);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, guardian));
        }

        [HttpGet("{id}/guardians")]
        public IActionResult GetGuardianByStudentId(Guid id)
        {
            var guardians = _guardianManager.GetByStudentId(id);

            if (guardians is null || !guardians.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, guardians));
        }

        [HttpGet("guardians/{id}")]
        public IActionResult GetGuardianById(Guid id)
        {
            var guardian = _guardianManager.GetById(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, guardian));
        }

        // TODO: Get user id
        [HttpPut("guardians/{id}")]
        public IActionResult UpdateGuardian(Guid id, [FromBody] StudentGuardianViewModel request)
        {
            var guardian = _guardianManager.Update(id, request, Guid.Empty);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, guardian));
        }

        [HttpDelete("guardians/{id}")]
        public IActionResult DeleteGuardian(Guid id)
        {
            _guardianManager.Delete(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK));
        }

        // TODO: Get user id
        [HttpPost("{studentId}/addresses")]
        public IActionResult CreateStudentAddress(Guid studentId, [FromBody] CreateStudentAddressViewModel request)
        {
            var address = _addressManager.Create(studentId, request, Guid.Empty);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, address));
        }

        [HttpGet("{id}/addresses")]
        public IActionResult GetStudentAddressByStudentId(Guid id)
        {
            var addresses = _addressManager.GetByStudentId(id);

            if (addresses is null || !addresses.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, addresses));
        }

        [HttpGet("addresses/{id}")]
        public IActionResult GetStudentAddressById(Guid id)
        {
            var address = _addressManager.GetById(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, address));
        }

        // TODO: Get user id
        [HttpPut("addresses/{id}")]
        public IActionResult UpdateContactAddress(Guid id, [FromBody] CreateStudentAddressViewModel request)
        {
            var address = _addressManager.Update(id, request, Guid.Empty);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, address));
        }

        [HttpDelete("addresses/{id}")]
        public IActionResult DeleteContactAddress(Guid id)
        {
            _addressManager.Delete(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpPost("{id}/studyCourses")]
        public IActionResult CreateStudyCourses(Guid id, Guid termId, IEnumerable<CreateStudyCourseViewModel> request)
        {
            var studyCourses = _studyCourseManager.Create(id, termId, request, Guid.Empty)
                                                  .ToList();

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, studyCourses));
        }

        [HttpGet("{id}/studyCourses")]
        public IActionResult GetStudyCourses(Guid id, Guid? termId = null)
        {
            var studyCourses = _registrationManager.GetByStudent(id, termId, true);

            if (studyCourses is null || !studyCourses.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, studyCourses));
        }

        [HttpPut("{id}/studyCourses")]
        public IActionResult UpdateStudyCourses(Guid id, IEnumerable<UpdateStudyCourseViewModel> requests)
        {
            var studyCourses = _studyCourseManager.Update(requests, Guid.Empty, studentId: id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, studyCourses));
        }

        [HttpPost("{id}/terms")]
        public IActionResult CreateStudentTerm(Guid id, [FromBody] UpdateStudentTermViewModel request)
        {
            var studentTerm = _studentTermManager.Create(id, request, Guid.Empty);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, studentTerm));
        }

        [HttpGet("{id}/terms")]
        public IActionResult GetStudentTerms(Guid id)
        {
            var studentTerms = _studentTermManager.GetByStudentId(id);

            if (studentTerms is null || !studentTerms.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, studentTerms));
        }

        [HttpGet("{id}/terms/{termId}")]
        public IActionResult GetStudentTerms(Guid id, Guid termId)
        {
            var studentTerm = _studentTermManager.GetByStudentIdAndTermId(id, termId);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, studentTerm));
        }

        [HttpPut("{id}/terms")]
        public IActionResult UpdateStudentTerm(Guid id, [FromBody] UpdateStudentTermViewModel request)
        {
            var studentTerm = _studentTermManager.Update(id, request, Guid.Empty);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, studentTerm));
        }

        [HttpPost("{id}/scholarships")]
        public IActionResult AddStudentScholarship(Guid id, [FromBody] CreateStudentScholarshipViewModel request)
        {
            var studentScholarship = _studentScholarshipManager.Create(id, request, Guid.Empty);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, studentScholarship));
        }

        [HttpGet("{id}/scholarships")]
        public IActionResult GetStudentScholarships(Guid id)
        {
            var studentScholarships = _studentScholarshipManager.GetByStudentId(id)
                                                                .ToList();

            if (studentScholarships is null || !studentScholarships.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, studentScholarships));
        }

        [HttpGet("{id}/scholarshipUsages")]
        public IActionResult GetStudentScholarshipUsages(Guid id, [FromQuery] SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var usages = _studentScholarshipManager.SearchUsages(id, parameters, page, pageSize);

            if (usages is null || !usages.Items.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, usages));
        }

        [HttpGet("{id}/courseTracks")]
        public IActionResult GetStudentCourseTracks(Guid id)
        {
            var courseTracks = _studentCourseTrackManager.GetByStudentId(id)
                                                         .ToList();

            if (courseTracks is null || !courseTracks.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, courseTracks));
        }

        [HttpPut("{id}/courseTracks")]
        public IActionResult UpdateStudentCourseTracks(Guid id, [FromBody] IEnumerable<Guid> courseTrackIds)
        {
            var courseTracks = _studentCourseTrackManager.Update(id, courseTrackIds);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, courseTracks));
        }

        [HttpPost("{id}/transferCourses")]
        public IActionResult TransferCourses(Guid id, [FromBody] CreateTransferViewModel request)
        {
            var transferCourses = _transferCourseManager.Create(id, request, Guid.Empty);

            if (transferCourses is null || !transferCourses.Any())
            {
                return NoContent();
            }

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, transferCourses));
        }

        [HttpGet("{id}/transferCourses")]
        public IActionResult GetTransferCourses(Guid id)
        {
            var transferCourses = _transferCourseManager.GetByStudent(id);

            if (transferCourses is null || !transferCourses.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, transferCourses));
        }

        [HttpGet("{id}/profile")]
        public IActionResult GetStudentCard(Guid id)
        {
            var student = _studentManager.GetStudentCard(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, student));
        }

        [HttpGet("{id}/general")]
        public IActionResult GetStudentGeneralInfo(Guid id)
        {
            var student = _studentManager.GetStudentGeneralInfo(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, student));
        }

        [HttpPut("{id}/general")]
        public IActionResult UpdateGeneralInfo(Guid id, CreateStudentGeneralInfoViewModel request)
        {
            var student = _studentManager.UpdateStudentGeneralInfo(id, request);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, student));
        }

        [HttpGet("{id}/contacts")]
        public IActionResult GetStudentContact(Guid id)
        {
            var student = _studentManager.GetStudentContact(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, student));
        }

        [HttpPut("{id}/contacts")]
        public IActionResult UpdateStudentContact(Guid id, CreateStudentContactViewModel request)
        {
            var student = _studentManager.UpdateStudentContact(id, request);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, student));
        }

        [HttpGet("{id}/academics")]
        public IActionResult GetAcademicInfo(Guid id)
        {
            var student = _studentManager.GetStudentAcademicInfo(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, student));
        }

        [HttpPut("{id}/academics")]
        public IActionResult UpdateAcademicInfo(Guid id, CreateStudentAcademicInfoViewModel request)
        {
            var student = _studentManager.UpdateStudentAcademicInfo(id, request);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, student));
        }

        [HttpGet("{id}/curriculums")]
        public IActionResult GetStudentCurriculumInfo(Guid id)
        {
            var student = _studentManager.GetStudentCurriculumInfo(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, student));
        }

        [HttpPut("{id}/curriculums")]
        public IActionResult UpdateStudentCurriculumInfo(Guid id, CreateStudentCurriculumViewModel request)
        {
            var student = _studentManager.UpdateStudentCurriculumInfo(id, request);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, student));
        }

        [HttpGet("{id}/curriculums/log")]
        public IActionResult GetStudentCurriculumLog(Guid id)
        {
            var student = _studentManager.GetStudentCurriculumLog(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, student));
        }
    }
}