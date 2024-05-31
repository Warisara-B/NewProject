using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.src.Academic;
using Plexus.Client.ViewModel.Academic;
using Plexus.Entity.DTO;
using Plexus.Utility;
using System.Net;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class GradeMaintenanceController : BaseController
    {
        private readonly IGradeMaintenanceManager _gradeMaintenanceManager;

        public GradeMaintenanceController(IGradeMaintenanceManager gradeMaintenanceManager)
        {
            _gradeMaintenanceManager = gradeMaintenanceManager;
        }

        [HttpPost]
        public IActionResult Create([FromForm] CreateGradeMaintenanceViewModel request)
        {
            var grade = _gradeMaintenanceManager.Create(request,Guid.Empty);
            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, grade));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var grade = _gradeMaintenanceManager.GetById(id);
            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, grade));
        }


        [HttpPut]
        public IActionResult Update([FromForm] GradeMaintenanceViewModel request)
        {
            var grade = _gradeMaintenanceManager.Update(request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, grade));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _gradeMaintenanceManager.Delete(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchGradeMaintenanceViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedGrades = _gradeMaintenanceManager.Search(parameters, page, pageSize);

            if (pagedGrades is null || !pagedGrades.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedGrades));
        }
        [HttpGet("searchCouse")]
        public IActionResult SearchCouse([FromQuery] SearchGradeMaintenanceViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedGrades = _gradeMaintenanceManager.SearchCouse(parameters, page, pageSize);

            if (pagedGrades is null || !pagedGrades.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedGrades));
        }
    }
}
