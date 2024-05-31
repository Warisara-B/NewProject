using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Academic.Curriculum;
using Plexus.Client.ViewModel.Registration;
using Plexus.Entity.DTO;
using Plexus.Utility;
using Azure;
using ServiceStack.Text;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class CurriculumsController : BaseController
    {
        private readonly ICurriculumManager _curriculumManager;
        private readonly ICurriculumVersionManager _curriculumVersionManager;
        private readonly ICurriculumCourseGroupManager _curriculumCourseGroupManager;
        private readonly IPrerequisiteManager _prerequisiteManager;
        private readonly IStudyPlanManager _studyPlanManager;
        private readonly ICurriculumInstructorManager _curriculumInstructorManager;

        public CurriculumsController(ICurriculumManager curriculumManager,
                                     ICurriculumVersionManager curriculumVersionManager,
                                     ICurriculumCourseGroupManager curriculumCourseGroupManager,
                                     IPrerequisiteManager prerequisiteManager,
                                     IStudyPlanManager studyPlanManager,
                                     ICurriculumInstructorManager curriculumInstructorManager)
        {
            _curriculumManager = curriculumManager;
            _curriculumVersionManager = curriculumVersionManager;
            _curriculumCourseGroupManager = curriculumCourseGroupManager;
            _prerequisiteManager = prerequisiteManager;
            _studyPlanManager = studyPlanManager;
            _curriculumInstructorManager = curriculumInstructorManager;
        }

        // TODO: Get user id
        [HttpPost]
        public IActionResult Create([FromBody] CreateCurriculumViewModel request)
        {
            var curriculum = _curriculumManager.Create(request, Guid.Empty);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, curriculum));
        }

        [HttpGet("search")]
        public IActionResult SearchCurriculum([FromQuery] SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedCurriculum = _curriculumManager.Search(parameters, page, pageSize);

            if (pagedCurriculum is null || !pagedCurriculum.Items.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, pagedCurriculum));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var curriculum = _curriculumManager.GetById(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, curriculum));
        }

        // TODO: Get user id
        [HttpPut]
        public IActionResult Update([FromBody] CurriculumViewModel request)
        {
            var curriculum = _curriculumManager.Update(request, Guid.Empty);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, curriculum));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _curriculumManager.Delete(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpPost("versions")]
        public IActionResult CreateVersion([FromBody] CreateCurriculumVersionViewModel request)
        {
            var curriculumVersion = _curriculumVersionManager.Create(request, Guid.Empty);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, curriculumVersion));
        }

        [HttpGet("versions/{id}")]
        public IActionResult GetVersionById(Guid id)
        {
            var curriculumVersion = _curriculumVersionManager.GetById(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, curriculumVersion));
        }

        [HttpPost("versions/{id}/copy")]
        public IActionResult CopyVersion(Guid id, [FromBody] CopyCurriculumVersionViewModel request)
        {
            var curriculumVersion = _curriculumVersionManager.Copy(id, request, Guid.Empty);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, curriculumVersion));
        }

        [HttpGet("versions/search")]
        public IActionResult SearchVersion([FromQuery] SearchCurriculumVersionCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedVersions = _curriculumVersionManager.Search(parameters, page, pageSize);

            if (pagedVersions is null || !pagedVersions.Items.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, pagedVersions));
        }

        [HttpPut("versions/{id}")]
        public IActionResult UpdateVersion(Guid id, [FromBody] CreateCurriculumVersionViewModel request)
        {
            var curriculumVersion = _curriculumVersionManager.Update(id, request, Guid.Empty);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, curriculumVersion));
        }

        [HttpDelete("versions/{id}")]
        public IActionResult DeleteVersionById(Guid id)
        {
            _curriculumVersionManager.Delete(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpGet("versions/{id}/blacklist")]
        public IActionResult GetVersionBlackListCourses(Guid id)
        {
            var blackListCourses = _curriculumVersionManager.GetBlackListCourses(id)
                                                            .ToList();

            if (!blackListCourses.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, blackListCourses));
        }

        [HttpPut("versions/{id}/blacklist")]
        public IActionResult UpdateVersionBlackListCourses(Guid id, IEnumerable<Guid> blackListCourseIds)
        {
            var blackListCourses = _curriculumVersionManager.UpdateBlackListCourses(id, blackListCourseIds)
                                                            .ToList();

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, blackListCourses));
        }

        [HttpGet("versions/{id}/academicSpecializations")]
        public IActionResult GetVersionSpecializations(Guid id)
        {
            var academicSpecializations = _curriculumVersionManager.GetAcademicSpecializations(id)
                                                                   .ToList();

            if (!academicSpecializations.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, academicSpecializations));
        }

        [HttpPut("versions/{id}/academicSpecializations")]
        public IActionResult UpdateVersionSpecializations(Guid id, IEnumerable<Guid> specializationIds)
        {
            var academicSpecializations = _curriculumVersionManager.UpdateAcademicSpecializations(id, specializationIds)
                                                                   .ToList();

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, academicSpecializations));
        }

        [HttpGet("versions/{id}/corequisites")]
        public IActionResult GetCorequisites(Guid id)
        {
            var corequisites = _curriculumVersionManager.GetCorequisites(id)
                                                        .ToList();

            if (!corequisites.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, corequisites));
        }

        [HttpPut("versions/{id}/corequisites")]
        public IActionResult UpdateCorequisites(Guid id, IEnumerable<CreateCorequisiteViewModel> corequisites)
        {
            var versionCorequisites = _curriculumVersionManager.UpdateCorequisites(id, corequisites)
                                                               .ToList();

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, versionCorequisites));
        }

        [HttpGet("versions/{id}/equivalentCourses")]
        public IActionResult GetEquivalentCourses(Guid id)
        {
            var corequisites = _curriculumVersionManager.GetEquivalentCourses(id)
                                                        .ToList();

            if (!corequisites.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, corequisites));
        }

        [HttpPut("versions/{id}/equivalentCourses")]
        public IActionResult UpdateEquivalentCourses(Guid id, IEnumerable<CreateEquivalentCourseViewModel> equivalences)
        {
            var versionCorequisites = _curriculumVersionManager.UpdateEquivalentCourses(id, equivalences)
                                                               .ToList();

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, versionCorequisites));
        }

        [HttpPost("coursegroups")]
        public IActionResult CreateCourseGroup([FromBody] CreateCurriculumCourseGroupViewModel request)
        {
            var courseGroup = _curriculumCourseGroupManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, courseGroup));
        }

        [HttpGet("versions/{id}/coursegroups")]
        public IActionResult GetByCourseGroupVersionId(Guid id)
        {
            var courseGroups = _curriculumCourseGroupManager.GetByCurriculumVersionId(id)
                                                            .ToList();

            if (!courseGroups.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courseGroups));
        }

        [HttpGet("versions/{id}/prerequisites/courses")]
        public IActionResult GetCurriculumPrerequisiteCourses(Guid id)
        {
            var prerequisites = _prerequisiteManager.GetCurriculumVersionPrerequisites(id)
                                                    .ToList();

            if (!prerequisites.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, prerequisites));
        }

        [HttpGet("coursegroups/{id}")]
        public IActionResult GetCourseGroupById(Guid id)
        {
            var courseGroup = _curriculumCourseGroupManager.GetById(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courseGroup));
        }

        [HttpPut("coursegroups/{id}")]
        public IActionResult Update(Guid id, [FromBody] CreateCurriculumCourseGroupViewModel request)
        {
            var courseGroup = _curriculumCourseGroupManager.Update(id, request, Guid.Empty);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courseGroup));
        }

        [HttpDelete("coursegroups/{id}")]
        public IActionResult DeleteCourseGroupById(Guid id)
        {
            _curriculumCourseGroupManager.Delete(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpGet("coursegroups/{id}/courses")]
        public IActionResult GetCourseGroupCurriculumCourseList(Guid id)
        {
            var courses = _curriculumCourseGroupManager.GetCourses(id);

            if (!courses.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courses));
        }

        [HttpPut("coursegroups/{id}/courses")]
        public IActionResult UpdateCoursegroupCurriculumCourses(Guid id, IEnumerable<CreateCurriculumCourseViewModel> requests)
        {
            var courses = _curriculumCourseGroupManager.UpdateCourses(id, requests)
                                                       .ToList();

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courses));
        }

        [HttpGet("coursegroups/{id}/ignoreCourses")]
        public IActionResult GetCourseGroupIgnoreCourses(Guid id)
        {
            var ignoreCourses = _curriculumCourseGroupManager.GetIgnoreCourses(id)
                                                             .ToList();

            if (!ignoreCourses.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, ignoreCourses));
        }

        [HttpPut("coursegroups/{id}/ignoreCourses")]
        public IActionResult UpdateCourseGroupIgnoreCourses(Guid id, IEnumerable<Guid> courseIds)
        {
            var academicSpecializations = _curriculumCourseGroupManager.UpdateIgnoreCourses(id, courseIds)
                                                                       .ToList();

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, academicSpecializations));
        }

        [HttpPost("studyplans")]
        public IActionResult CreateStudyPlan(CreateStudyPlanViewModel request)
        {
            var studyPlan = _studyPlanManager.CreateStudyPlan(request);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, studyPlan));
        }

        [HttpPost("studyplans/{id}/details")]
        public IActionResult AddStudyPlanDetails(Guid id, CreateStudyPlanDetailViewModel request)
        {
            var studyPlan = _studyPlanManager.AddStudyPlanDetail(id, request);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, studyPlan));
        }

        [HttpGet("studyplans")]
        public IActionResult GetStudyPlans()
        {
            var studyPlans = _studyPlanManager.GetStudyPlans();

            if (!studyPlans.Any())
            {
                NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, studyPlans));
        }

        [HttpPut("studyplans/{id}")]
        public IActionResult UpdateStudyPlan(Guid id, UpdateStudyPlanViewModel request)
        {
            var studyPlan = _studyPlanManager.UpdateStudyPlan(id, request);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, studyPlan));
        }

        [HttpPut("studyplans/{id}/details/{year}/{term}")]
        public IActionResult UpdateStudyPlanCourse(Guid id, int year, string term, CreateStudyPlanDetailViewModel request)
        {
            var studyPlan = _studyPlanManager.UpdateStudyPlanDetail(id, year, term, request);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, studyPlan));
        }

        [HttpDelete("studyplans/{id}")]
        public IActionResult DeleteStudyPlan(Guid id)
        {
            _studyPlanManager.Delete(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpDelete("studyplans/{id}/{year}")]
        public IActionResult DeleteStudyPlanByYear(Guid id, int year)
        {
            _studyPlanManager.DeleteByYear(id, year);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpDelete("studyplans/{id}/{year}/{term}")]
        public IActionResult DeleteStudyPlanByTerm(Guid id, int year, string term)
        {
            _studyPlanManager.DeleteByTerm(id, year, term);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpPost("versions/{id}/instructors")]
        public IActionResult CreateCurriculumInstructor(Guid id, CreateCurriculumInstructorViewModel request)
        {
            var instructor = _curriculumInstructorManager.Create(id, request);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, instructor));
        }

        [HttpGet("versions/{id}/instructors")]
        public IActionResult GetCurriculumInstructors(Guid id, int page = 1, int pageSize = 25)
        {
            var pagedInstructors = _curriculumInstructorManager.GetList(id, page, pageSize);

            if (pagedInstructors is null || !pagedInstructors.Items.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, pagedInstructors));
        }

        [HttpPut("versions/instructors/{id}")]
        public IActionResult UpdateCurriculumInstructors(Guid id, CreateCurriculumInstructorViewModel request)
        {
            var instructor = _curriculumInstructorManager.Update(id, request);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, instructor));
        }

        [HttpDelete("versions/instructors/{id}")]
        public IActionResult DeleteCurriculumInstructor(Guid id)
        {
            _curriculumInstructorManager.Delete(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}
