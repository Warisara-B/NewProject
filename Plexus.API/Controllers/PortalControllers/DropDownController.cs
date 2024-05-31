using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.SearchFilter;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class DropDownController : BaseController
    {
        private readonly IAcademicLevelManager _academicLevelManager;
        private readonly IAcademicProgramManager _academicProgramManager;
        private readonly ITermManager _termManager;
        private readonly ICampusManager _campusManager;
        private readonly IBuildingManager _buildingManager;
        private readonly IRoomManager _roomManager;
        private readonly IFacilityManager _facilityManager;
        private readonly IFacultyManager _facultyManager;
        private readonly IDepartmentManager _departmentManager;
        private readonly ICourseManager _courseManager;
        private readonly ISectionManager _sectionManager;
        private readonly IAcademicSpecializationManager _academicSpecializationManager;
        private readonly ICurriculumManager _curriculumManager;
        private readonly ICurriculumVersionManager _curriculumVersionManager;
        private readonly IGradeManager _gradeManager;
        private readonly IFeeItemManager _feeItemManager;
        private readonly ITermFeePackageManager _termFeePackageManager;
        private readonly IRateTypeManager _rateTypeManager;
        private readonly IScholarshipTypeManager _scholarshipTypeManager;
        private readonly IScholarshipManager _scholarshipManager;
        private readonly ICourseTrackManager _courseTrackManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly IStudentScholarshipManager _studentScholarshipManager;
        private readonly IStudentFeeTypeManager _studentFeeTypeManager;
        private readonly IFeeGroupManager _feeGroupManager;
        private readonly IInstructorTypeManager _instructorTypeManager;
        private readonly IEmployeeGroupManager _employeeGroupManager;
        private readonly IArticleTypeManager _articleTypeManager;
        private readonly IInstructorRoleManager _instructorRoleManager;
        private readonly IAcademicPositionManager _academicPositionManager;
        private readonly ICareerPositionManager _careerPositionManager;
        private readonly ITeachingTypeManager _teachingTypeManager;

        public DropDownController(IAcademicLevelManager academicLevelManager,
                                  IAcademicProgramManager academicProgramManager,
                                  ITermManager termManager,
                                  ICampusManager campusManager,
                                  IBuildingManager buildingManager,
                                  IRoomManager roomManager,
                                  IFacilityManager facilityManager,
                                  IFacultyManager facultyManager,
                                  IDepartmentManager departmentManager,
                                  ICourseManager courseManager,
                                  ISectionManager sectionManager,
                                  IAcademicSpecializationManager academicSpecializationManager,
                                  ICurriculumManager curriculumManager,
                                  ICurriculumVersionManager curriculumVersionManager,
                                  IGradeManager gradeManager,
                                  IFeeItemManager feeTypeManager,
                                  ITermFeePackageManager termFeePackageManager,
                                  IRateTypeManager rateTypeManager,
                                  IScholarshipTypeManager scholarshipTypeManager,
                                  IScholarshipManager scholarshipManager,
                                  ICourseTrackManager courseTrackManager,
                                  IEmployeeManager employeeManager,
                                  IStudentScholarshipManager studentScholarshipManager,
                                  IStudentFeeTypeManager studentFeeTypeManager,
                                  IFeeGroupManager feeGroupManager,
                                  IInstructorTypeManager instructorTypeManager,
                                  IEmployeeGroupManager employeeGroupManager,
                                  IArticleTypeManager articleTypeManager,
                                  IInstructorRoleManager instructorRoleManager,
                                  IAcademicPositionManager academicPositionManager,
                                  ICareerPositionManager careerPositionManager,
                                  ITeachingTypeManager teachingTypeManager)
        {
            _academicLevelManager = academicLevelManager;
            _academicProgramManager = academicProgramManager;
            _termManager = termManager;
            _campusManager = campusManager;
            _facultyManager = facultyManager;
            _departmentManager = departmentManager;
            _buildingManager = buildingManager;
            _roomManager = roomManager;
            _facilityManager = facilityManager;
            _courseManager = courseManager;
            _sectionManager = sectionManager;
            _academicSpecializationManager = academicSpecializationManager;
            _curriculumManager = curriculumManager;
            _curriculumVersionManager = curriculumVersionManager;
            _gradeManager = gradeManager;
            _feeItemManager = feeTypeManager;
            _termFeePackageManager = termFeePackageManager;
            _rateTypeManager = rateTypeManager;
            _scholarshipTypeManager = scholarshipTypeManager;
            _scholarshipManager = scholarshipManager;
            _courseTrackManager = courseTrackManager;
            _employeeManager = employeeManager;
            _studentScholarshipManager = studentScholarshipManager;
            _studentFeeTypeManager = studentFeeTypeManager;
            _feeGroupManager = feeGroupManager;
            _instructorTypeManager = instructorTypeManager;
            _employeeGroupManager = employeeGroupManager;
            _articleTypeManager = articleTypeManager;
            _instructorRoleManager = instructorRoleManager;
            _academicPositionManager = academicPositionManager;
            _careerPositionManager = careerPositionManager;
            _teachingTypeManager = teachingTypeManager;
        }

        [HttpGet("academicLevels")]
        public IActionResult GetAcademicLevels([FromQuery] SearchAcademicLevelCriteriaViewModel parameters)
        {
            var academicLevels = _academicLevelManager.GetDropDownList(parameters)
                                                      .ToList();

            if (academicLevels is null || !academicLevels.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, academicLevels));
        }

        [HttpGet("terms")]
        public IActionResult GetTerms([FromQuery] SearchTermCriteriaViewModel parameters)
        {
            var terms = _termManager.GetDropDownList(parameters)
                                    .ToList();

            if (terms is null || !terms.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, terms));
        }

        [HttpGet("campuses")]
        public IActionResult GetCampuses([FromQuery] SearchCampusCriteriaViewModel parameters)
        {
            var campuses = _campusManager.GetDropdownList(parameters)
                                         .ToList();

            if (campuses is null || !campuses.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, campuses));
        }

        [HttpGet("buildings")]
        public IActionResult GetBuildings([FromQuery] SearchBuildingCriteriaViewModel parameters)
        {
            var buildings = _buildingManager.GetDropDownList(parameters)
                                            .ToList();

            if (buildings is null || !buildings.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, buildings));
        }

        [HttpGet("rooms")]
        public IActionResult GetRooms([FromQuery] SearchRoomCriteriaViewModel parameters)
        {
            var rooms = _roomManager.GetDropDownList(parameters)
                                    .ToList();

            if (rooms is null || !rooms.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, rooms));
        }

        [HttpGet("faculties")]
        public IActionResult GetFaculties([FromQuery] SearchFacultyCriteriaViewModel parameters)
        {
            var faculties = _facultyManager.GetDropdownList(parameters)
                                           .ToList();

            if (faculties is null || !faculties.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, faculties));
        }

        [HttpGet("departments")]
        public IActionResult GetDepartments([FromQuery] SearchDepartmentCriteriaViewModel parameters)
        {
            var departments = _departmentManager.GetDropdownList(parameters)
                                                .ToList();

            if (departments is null || !departments.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, departments));
        }

        [HttpGet("courses")]
        public IActionResult GetCourses([FromQuery] SearchCourseCriteriaViewModel parameters)
        {
            var courses = _courseManager.GetDropDownList(parameters)
                                        .ToList();

            if (courses is null || !courses.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, courses));
        }

        [HttpGet("sections")]
        public IActionResult GetSections([FromQuery] SearchSectionCriteriaViewModel parameters)
        {
            var sections = _sectionManager.GetDropDownList(parameters)
                                          .ToList();

            if (sections is null || !sections.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, sections));
        }

        [HttpGet("grades")]
        public IActionResult GetGrades()
        {
            var grades = _gradeManager.GetDropdownList()
                                      .ToList();

            if (grades is null || !grades.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, grades));
        }

        [HttpGet("curriculums/versions/{id}/courses")]
        public IActionResult GetCurriculumVersionCourses(Guid id)
        {
            var courses = _curriculumVersionManager.GetCurriculumVersionCourseDropdownLists(id)
                                                   .ToList();

            if (courses is null || !courses.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, courses));
        }

        [HttpGet("curriculums/versions")]
        public IActionResult GetCurriculumVersions([FromQuery] SearchCurriculumVersionCriteriaViewModel parameters)
        {
            var versions = _curriculumVersionManager.GetDropDownList(parameters)
                                                    .ToList();

            if (versions is null || !versions.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, versions));
        }

        [HttpGet("academicSpecializations")]
        public IActionResult GetAcademicSpecializations([FromQuery] SearchAcademicSpecializationCriteriaViewModel parameters)
        {
            var academicSpecializations = _academicSpecializationManager.GetDropdownList(parameters)
                                                                        .ToList();

            if (academicSpecializations is null || !academicSpecializations.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, academicSpecializations));
        }

        [HttpGet("feeItems")]
        public IActionResult GetFeeItems([FromQuery] SearchFeeItemCriteriaViewModel parameters)
        {
            var feeItems = _feeItemManager.GetDropDownList(parameters)
                                          .ToList();

            if (feeItems is null || !feeItems.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, feeItems));
        }

        [HttpGet("termFeePackages")]
        public IActionResult GetTermFeePackages([FromQuery] SearchTermFeePackageCriteriaViewModel parameters)
        {
            var packages = _termFeePackageManager.GetDropDownList(parameters)
                                                 .ToList();

            if (packages is null || !packages.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, packages));
        }

        [HttpGet("curriculums")]
        public IActionResult GetCurriculums([FromQuery] SearchCriteriaViewModel parameters)
        {
            var curriculums = _curriculumManager.GetDropDownList(parameters)
                                                .ToList();

            if (curriculums is null || !curriculums.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, curriculums));
        }

        [HttpGet("rateTypes")]
        public IActionResult GetRateTypes([FromQuery] SearchRateTypeCriteriaViewModel parameters)
        {
            var rateTypes = _rateTypeManager.GetDropDownList(parameters)
                                            .ToList();

            if (rateTypes is null || !rateTypes.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, rateTypes));
        }

        [HttpGet("scholarshipTypes")]
        public IActionResult GetScholarshipTypes([FromQuery] SearchCriteriaViewModel parameters)
        {
            var scholarshipTypes = _scholarshipTypeManager.GetDropDownList(parameters)
                                                          .ToList();

            if (scholarshipTypes is null || !scholarshipTypes.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, scholarshipTypes));
        }

        [HttpGet("scholarship")]
        public IActionResult GetScholarship([FromQuery] SearchCriteriaViewModel parameters)
        {
            var scholarshipList = _scholarshipManager.GetDropDownList(parameters)
                                                     .ToList();

            if (scholarshipList is null || !scholarshipList.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, scholarshipList));
        }

        [HttpGet("courseTracks")]
        public IActionResult GetCourseTracks([FromQuery] SearchCriteriaViewModel parameters)
        {
            var courseTracks = _courseTrackManager.GetDropDownList(parameters)
                                                  .ToList();

            if (courseTracks is null || !courseTracks.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, courseTracks));
        }

        [HttpGet("instructors")]
        public IActionResult GetInstructors([FromQuery] SearchCriteriaViewModel parameters)
        {
            var instructors = _employeeManager.GetDropDownList(parameters)
                                                .ToList();

            if (instructors is null || !instructors.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, instructors));
        }

        [HttpGet("instructorRoles")]
        public IActionResult GetInstructorRoles([FromQuery] SearchInstructorRoleCriteriaViewModel parameters)
        {
            var instructorRoles = _instructorRoleManager.GetDropDownList(parameters)
                                                        .ToList();

            if (instructorRoles is null || !instructorRoles.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, instructorRoles));
        }

        [HttpGet("academicPrograms")]
        public IActionResult GetAcademicPrograms([FromQuery] SearchAcademicProgramCriteriaViewModel parameters)
        {
            var academicPrograms = _academicProgramManager.GetDropDownList(parameters)
                                                          .ToList();

            if (academicPrograms is null || !academicPrograms.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, academicPrograms));
        }

        [HttpGet("facilities")]
        public IActionResult GetFacilities([FromQuery] SearchFacilityCriteriaViewModel parameters)
        {
            var facilities = _facilityManager.GetDropDownList(parameters)
                                             .ToList();

            if (facilities is null || !facilities.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, facilities));
        }

        [HttpGet("studentBudgets")]
        public IActionResult GetStudentBudgets([FromQuery] SearchCriteriaViewModel parameters)
        {
            var budgets = _studentScholarshipManager.GetBudgetDropDownList(parameters)
                                                    .ToList();

            if (budgets is null || !budgets.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, budgets));
        }

        [HttpGet("studentFeeTypes")]
        public IActionResult GetStudentFeeTypes([FromQuery] SearchCriteriaViewModel parameters)
        {
            var studentFeeTypes = _studentFeeTypeManager.GetDropDownList(parameters)
                                                        .ToList();

            if (studentFeeTypes is null || !studentFeeTypes.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, studentFeeTypes));
        }

        [HttpGet("feeGroups")]
        public IActionResult GetFeeGroups([FromQuery] SearchFeeGroupCriteriaViewModel parameters)
        {
            var feeGroups = _feeGroupManager.GetDropDownList(parameters)
                                            .ToList();

            if (feeGroups is null || !feeGroups.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, feeGroups));
        }

        [HttpGet("instructorTypes")]
        public IActionResult GetInstructorTypes([FromQuery] SearchCriteriaViewModel parameters)
        {
            var instructorTypes = _instructorTypeManager.GetDropDownList(parameters)
                                                        .ToList();

            if (instructorTypes is null || !instructorTypes.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, instructorTypes));
        }

        [HttpGet("employeeGroups")]
        public IActionResult GetEmployeeGroups([FromQuery] SearchEmployeeGroupCriteriaViewModel parameters)
        {
            var employeeGroups = _employeeGroupManager.GetDropDownList(parameters)
                                                        .ToList();

            if (employeeGroups is null || !employeeGroups.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, employeeGroups));
        }

        [HttpGet("articleTypes")]
        public IActionResult GetArticleTypes([FromQuery] SearchArticleTypeCriteriaViewModel parameters)
        {
            var articleTypes = _articleTypeManager.GetDropDownList(parameters)
                                                  .ToList();

            if (articleTypes is null || !articleTypes.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, articleTypes));
        }

        [HttpGet("academicPositions")]
        public IActionResult GetAcademicPositions([FromQuery] SearchAcademicPositionCriteriaViewModel parameters)
        {
            var academicPositions = _academicPositionManager.GetDropDownList(parameters)
                                                            .ToList();

            if (academicPositions is null || !academicPositions.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, academicPositions));
        }

        [HttpGet("careerPositions")]
        public IActionResult GetCareerPositions([FromQuery] SearchCareerPositionCriteriaViewModel parameters)
        {
            var careerPositions = _careerPositionManager.GetDropDownList(parameters);

            if (careerPositions is null || !careerPositions.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, careerPositions));
        }

        [HttpGet("teachingTypes")]
        public IActionResult GetTeachingTypes([FromQuery] SearchTeachingTypeCriteriaViewModel parameters)
        {
            var teachingTypes = _teachingTypeManager.GetDropDownList(parameters);

            if (teachingTypes is null || !teachingTypes.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, teachingTypes));
        }
    }
}
